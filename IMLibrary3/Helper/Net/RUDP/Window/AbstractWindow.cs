//#define TRACE_MEMORY
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Diagnostics;

using Helper.ThreadingNET35;

namespace Helper.Net.RUDP
{

	#region SWSlot

	internal sealed class SWSlot
	{
		internal int StartPacketId;
		internal int EndPacketId;

		internal long StartByte;
		internal long EndByte;

		internal SWSlot(int startPacketId,
						int endPacketId,
						long startByte,
						long endByte)
		{
			StartPacketId = startPacketId;
			EndPacketId = endPacketId;
			StartByte = startByte;
			EndByte = endByte;
		}

		public override string ToString()
		{
			return "StartPacketId(" + StartPacketId + ") EndPacketId(" + EndPacketId + ") Length(" + (EndByte - StartByte) + ")";
		}
	}

	#endregion

	/// <summary>
	/// This window act as Congestions and Sliding window.
	/// 
	/// Because we have SACK and we handle ACK in a more efficient way, we do not really
	/// have a sliding window but another mechanism (slots) to handle ACKs.
	/// </summary>
	abstract internal class AbstractWindow
	{

		#region Variables

#if TRACE_MEMORY
		//private List<string> traces = new List<string>();
#endif

		internal RUDPSocket _rudp;

		//---- The slots : The AKCs we are waiting for
		private List<SWSlot> _sendSlots = new List<SWSlot>();
		private ReaderWriterLockSlim _sendSlotsLock = new ReaderWriterLockSlim();

		//---- Window sizes

		// Congestion Window Size
		internal double _cwnd;

		// Receive Window Size
		internal int _rwnd;

		// Advertised window
		internal int _awnd = Int32.MaxValue;

		// Amount of data that has been sent but not yet acknowledged (acked).
		internal int FlightSize;

		//---- Synchronization
		internal ManualResetEvent _event = new ManualResetEvent(true);

		//---- Pause / Resume of transmission
		internal bool _transmissionPaused = false;

		#endregion

		#region Constructor

		internal AbstractWindow(RUDPSocket rudp)
		{
			_rudp = rudp;
		}

		#endregion

		#region Reset

		internal virtual void Reset()
		{
			_sendSlotsLock.EnterWriteLock();
			_sendSlots.Clear();
			_sendSlotsLock.ExitWriteLock();

			// Initial = MTU
			_cwnd = 2 * PMTUDiscovery.MinMTU;
			_awnd = Int32.MaxValue;
			_rwnd = 0;

			// http://www.faqs.org/rfcs/rfc2988.html
			_rudp.SetRTT(0, 0, 3 * 1000000, (long)(15000000 + 4 * _rudp._rto));

			if (_event != null)
			{
				_event.Set();

				// Also allow fragments to be sent
				RUDPStack.ForceFragmentsSending(_rudp._controlThreadId);
			}
		}

		#endregion

		#region CanSend

		internal bool CanSend(int payloadLength)
		{
			if (_transmissionPaused)
				return true;

			_sendSlotsLock.EnterReadLock();

			if (_sendSlots.Count < 1)
			{
				_sendSlotsLock.ExitReadLock();
				return true;
			}

			// Used Window size
			long startByte = _sendSlots[0].StartByte;
			long endByte = _sendSlots[_sendSlots.Count - 1].EndByte;

			_sendSlotsLock.ExitReadLock();

			RUDPStack.Trace("Congestion => " + Math.Min(_rudp._sendSize, CWND) + ">" + (payloadLength + endByte - startByte));

			int advertizedWindow = Math.Max(_awnd, _rudp._mtu);
			return Math.Min(advertizedWindow, Math.Min(_rudp._sendSize, CWND)) >= (payloadLength + endByte - startByte);
		}

		#endregion

		#region CanReceive

		internal bool CanReceive(int packetId, int length)
		{
			// Check if it is not a previous packet to accept
			int lastPacketId = _rudp._incomingPackets.LastPacketId;
			if (packetId < lastPacketId)
				return true;

			// Check if it remain enough size
			return _rudp._receiveSize > (_rwnd + length);
		}

		#endregion

		#region PauseTransmission / ResumeTransmission

		internal void ResumeTransmission()
		{
			_transmissionPaused = false;
			if (CanSend(0))
			{
				_event.Set();

				// Also allow fragments to be sent
				RUDPStack.ForceFragmentsSending(_rudp._controlThreadId);
			}
		}

		internal void PauseTransmission()
		{
			_transmissionPaused = true;
			_event.Reset();

			// Also allow fragments to be sent
			RUDPStack.ForceFragmentsSending(_rudp._controlThreadId);
		}

		#endregion

		#region OnSend

		internal void OnSend(int packetId, long sequence, int payloadLength)
		{
			FlightSize += payloadLength + RUDPStack.RUDPHeaderLength + RUDPStack.UDPHeaderLength;

			//---- Update window (if needed)
			OnSend_UpdateWindow(payloadLength);

			//---- Sliding
			try
			{
				SWSlot slot;

				_sendSlotsLock.EnterWriteLock();

				//---- First slot
				if (_sendSlots.Count < 1)
				{
					slot = new SWSlot(packetId, -1, sequence, sequence + payloadLength);
					_sendSlots.Add(slot);
					return;
				}

				//---- Check the last slot only, because ordered
				slot = _sendSlots[_sendSlots.Count - 1];

				// Grow the slot
				if ((slot.EndPacketId < 0 && slot.StartPacketId == packetId - 1) ||
					(slot.EndPacketId > -1 && slot.EndPacketId == packetId - 1))
				{
					slot.EndPacketId = packetId;
					slot.EndByte += payloadLength;
					return;
				}

				// New slot
				slot = new SWSlot(packetId, -1, sequence, sequence + payloadLength);
				_sendSlots.Add(slot);
			}
			finally
			{
				_sendSlotsLock.ExitWriteLock();

				// Lock
				if (!CanSend(payloadLength))
					PauseTransmission();
			}
		}

		#endregion

		#region OnSend_UpdateWindow

		virtual internal void OnSend_UpdateWindow(int payloadLength)
		{
		}

		#endregion

		#region OnTimeOut

		/// <summary>
		/// Called when a packet is resended
		/// </summary>
		internal void OnTimeOut(RUDPOutgoingPacket packet)
		{
			//---- Congestion
			lock (this)
			{
				OnTimeOut_UpdateParameters(packet);
				OnTimeOut_UpdateWindow();
			}
		}

		#endregion

		#region OnTimeOut_UpdateParameters

		internal void OnTimeOut_UpdateParameters(RUDPOutgoingPacket packet)
		{
			// Karn's Algorithm : On successive retransmissions, set each timeout to twice the previous one.
			if (packet.Retransmission > 0)
			{
				//-- Calculate _rto
#if TRACE_MEMORY
				traces.Add("RTO x 2 : " + _rudp._rto + " new : " + _rudp._rto * 2);
#endif
				// Exponential RTO Backoff process
				_rudp.RTO = Math.Min(_rudp.RTO * 2, 60000000);

				//-- Calculate _sto
				_rudp.STO = (int)(15000000 + 4 * _rudp.RTT);
			}
		}

		#endregion

		#region OnTimeOut_UpdateWindow

		abstract internal void OnTimeOut_UpdateWindow();

		#endregion

		#region OnACK

		internal void OnACK(RUDPOutgoingPacket packet, double currentRTT)
		{
			// Duplicated ACK
			if (packet == null)
				return;

			FlightSize -= packet.Payload.Length + RUDPStack.UDPHeaderLength;

			//---- Congestion
			lock (this)
			{
				OnACK_UpdateParameters(packet, currentRTT);
				OnACK_UpdateWindow(packet);
			}

			//---- Sliding
			int packetId = packet.PacketId;

			_sendSlotsLock.EnterWriteLock();

			SWSlot slot = null;
			try
			{
				for (int index = 0; index < _sendSlots.Count; index++)
				{
					slot = _sendSlots[index];

					// Delete this slot
					if (slot.EndPacketId < 0 && slot.StartPacketId == packetId)
					{
						_sendSlots.RemoveAt(index);
						return;
					}

					// Decrease the slot
					if (slot.EndPacketId > -1 && slot.StartPacketId == packetId)
					{
						slot.StartPacketId++;
						slot.StartByte += packet.Payload.Length;
						if (slot.StartPacketId == slot.EndPacketId)
							slot.EndPacketId = -1;
						return;
					}

					// Decrease the slot
					if (slot.EndPacketId > -1 && slot.EndPacketId == packetId)
					{
						slot.EndPacketId--;
						slot.EndByte -= packet.Payload.Length;
						if (slot.StartPacketId == slot.EndPacketId)
							slot.EndPacketId = -1;
						return;
					}

					// Split the slot
					if (slot.EndPacketId > -1 && slot.StartPacketId <= packetId && slot.EndPacketId >= packetId)
					{
						// Right slot
						SWSlot newSlot = new SWSlot(packetId + 1,
													slot.EndPacketId,
													packet.Sequence + packet.Payload.Length + 1,
													slot.EndByte);
						if (newSlot.StartPacketId == newSlot.EndPacketId)
							newSlot.EndPacketId = -1;
						_sendSlots.Insert(index + 1, newSlot);

						// Left slot
						slot.EndPacketId = packetId - 1;
						slot.EndByte = packet.Sequence - 1;
						if (slot.StartPacketId == slot.EndPacketId)
							slot.EndPacketId = -1;

						return;
					}
				}

			}
			finally
			{
				_sendSlotsLock.ExitWriteLock();
				ResumeTransmission();
			}
		}

		#endregion

		#region OnACK_UpdateParameters

		internal void OnACK_UpdateParameters(RUDPOutgoingPacket packet, double currentRTT)
		{
			//---- TCP - (RFC 2988) : http://www.faqs.org/rfcs/rfc2988.html
			if (packet.Retransmission > 0)
				return;

			double rudpRTT;
			double rudpDeltaRTT;

			//-- Calculate _rtt
			if (_rudp._rtt == 0)
			{
				rudpRTT = currentRTT;
				rudpDeltaRTT = currentRTT / 2;
			}
			else
			{
				rudpRTT = _rudp.RTT;
				rudpDeltaRTT = _rudp.DeltaRTT;

				// Jacobson's algorithm
				rudpRTT = (0.875 * rudpRTT + 0.125 * currentRTT);
				double error = currentRTT - rudpRTT;
				rudpDeltaRTT = (0.75 * rudpDeltaRTT + 0.25 * Math.Abs(error));
			}

			//-- Calculate _rto
#if TRACE_MEMORY
			traces.Add("RTO ACK : " + _rudp._rto + " new : " + _rudp._rtt + Math.Max(1, 4 * _rudp._deltaRtt) + "RTT=" + _rudp._rtt);
#endif
			double Rto = rudpRTT + Math.Max(4, 4 * rudpDeltaRTT);

			//if (_rudp._rto < 1000000)
			//    _rudp._rto = 1000000;
			if (Rto > 60000000)
				Rto = 60000000;

			//-- Calculate _sto
			long Sto = (long)(15000000 + 4 * rudpRTT);

			//-- Update parameters
			_rudp.SetRTT(rudpRTT, rudpDeltaRTT, Rto, Sto);
		}

		#endregion

		#region OnACK_UpdateWindow

		abstract internal void OnACK_UpdateWindow(RUDPOutgoingPacket packet);

		#endregion

		#region OnReceive

		internal void OnReceive(RUDPIngoingPacket packet)
		{
			if (packet != null)
				Interlocked.Exchange(ref _rwnd, _rwnd + packet.Payload.Length);
		}

		#endregion

		#region OnReceiveProcessed

		internal void OnReceiveProcessed(RUDPIngoingPacket packet)
		{
			Interlocked.Exchange(ref _rwnd, _rwnd - packet.Payload.Length);
		}

		#endregion

		#region OnReceiveAdvertisedWindow

		internal void OnReceiveAdvertisedWindow(int windowSize)
		{
			_awnd = windowSize;
		}

		#endregion

		#region OnOutOfOrder

		virtual internal void OnOutOfOrder(int startPacketId, int endPacketId)
		{
		}

		#endregion

		#region OnEndFastRetransmit

		virtual internal void OnEndFastRetransmit()
		{
		}

		#endregion

		#region WaitOne

		internal void WaitOne()
		{
			_event.WaitOne(1, true);
		}

		#endregion

		#region Properties

		internal ManualResetEvent WaitObject
		{
			get
			{
				return _event;
			}
		}

		internal int AdvertisedWindow
		{
			get
			{
				// AdvertisedWindow = remaining size in the receive buffer
				return _rudp._receiveSize - _rwnd;
			}
		}

		internal double CWND
		{
			get
			{
				return Thread.VolatileRead(ref _cwnd);
			}
			set
			{
				Thread.VolatileWrite(ref _cwnd, Math.Max(_rudp._mtu, value));
			}
		}

		#endregion

	}

}

//#define CHECK_SACKSCOUNT

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace Helper.Net.RUDP
{
	#region SACKSlot

	internal sealed class SACKSlot
	{
		internal int StartPacketId;
		internal int EndPacketId;

		// To avoid to resend several times the same ACK
		internal int ACKsCount = 1;

		internal SACKSlot(int startPacketId, int endPacketId)
		{
			StartPacketId = startPacketId;
			EndPacketId = endPacketId;
		}

		internal SACKSlot(int startPacketId, int endPacketId, int acksCount)
		{
			StartPacketId = startPacketId;
			EndPacketId = endPacketId;
			ACKsCount = acksCount;
		}

		internal SACKSlot Clone()
		{
			return new SACKSlot(StartPacketId, EndPacketId, ACKsCount);
		}

		public override string ToString()
		{
			return "StartPacketId(" + StartPacketId + ") EndPacketId(" + EndPacketId + ") ACKsCount(" + ACKsCount + ")";
		}
	}

	#endregion

	sealed internal class SACKWindow
	{

		#region Variables

		private List<SACKSlot> _slots = new List<SACKSlot>();
		private int _acksCount = 0;

		#endregion

		#region Constructor

		internal SACKWindow()
		{
			SACKSlot slot = new SACKSlot(-1, -1);
			slot.ACKsCount = 0;
			_slots.Add(slot);
		}

		#endregion

		#region OnReceivePacket

		internal void OnReceivePacket(int packetId)
		{
			RUDPStack.Trace("SACK OnReceivePacket:" + packetId);

			Monitor.Enter(this);
			try
			{
				SACKSlot slot;
				SACKSlot newSlot;
				for (int index = 0; index < _slots.Count; index++)
				{
					slot = _slots[index];

					//-- Already in the slot
					if (packetId >= slot.StartPacketId && packetId <= slot.EndPacketId)
					{
						slot.ACKsCount++;
						return;
					}

					//-- Grow the slot
					if (slot.EndPacketId + 1 == packetId)
					{
						slot.EndPacketId++;
						slot.ACKsCount++;

						// First packet
						if (slot.StartPacketId < 0)
							slot.StartPacketId = 0;

						// Merge with next
						if ((index + 1) < _slots.Count && (slot.EndPacketId + 1) == _slots[index + 1].StartPacketId)
						{
							index++;
							slot.EndPacketId = _slots[index].EndPacketId;
							slot.ACKsCount += _slots[index].ACKsCount;

							_slots.RemoveAt(index);
						}

						return;
					}

					//-- Before this slot (and then after previous one !)
					if (packetId < slot.StartPacketId)
					{
						if (packetId == slot.StartPacketId - 1)
						{
							slot.StartPacketId--;
							slot.ACKsCount++;
						}
						else
						{
							newSlot = new SACKSlot(packetId, packetId);
							_slots.Insert(index, newSlot);
						}

						return;
					}
				}

				//-- New slot at the end
				newSlot = new SACKSlot(packetId, packetId);
				_slots.Add(newSlot);
			}
			finally
			{
				Interlocked.Increment(ref _acksCount);
				Monitor.Exit(this);
				CheckACKCount();
			}
		}

		#endregion

		#region PrepareACKList

		/// <summary>
		/// Used when "resending" or sending queued "ACKs"
		/// </summary>
		internal List<SACKSlot> PrepareACKList()
		{
			List<SACKSlot> list = new List<SACKSlot>();

			lock (this)
			{
				//---- Add all the slots, except the first one
				for (int index = 1; index < _slots.Count; index++)
				{
					SACKSlot slot = _slots[index];
					if (slot.ACKsCount > 0)
					{
						slot.ACKsCount = 0;
						list.Add(slot.Clone());
					}
				}

				//---- Add the first slot
				// Useful when some ACKs are loosed
				if (_slots[0].StartPacketId > -1)
					if (list.Count > 1 || _slots[0].ACKsCount > 0)
					{
						_slots[0].ACKsCount = 0;
						list.Insert(0, _slots[0].Clone());
					}

				Interlocked.Exchange(ref _acksCount, 0);
			}

			CheckACKCount();

			return list;
		}

		#endregion

		#region GetSLACKSlots

		/// <summary>
		/// Used when sending a packet.
		/// </summary>
		internal void GetSLACKSlots(out SACKSlot slot1, out SACKSlot slot2, out SACKSlot slot3, out SACKSlot slot4)
		{
			slot1 = null;
			slot2 = null;
			slot3 = null;
			slot4 = null;

			lock (this)
			{
				if (_slots[0].StartPacketId < 0)
					return;

				//---- Useful when some ACKs are loosed
				slot1 = _slots[0].Clone();
				_slots[0].ACKsCount = 0;
				Interlocked.Exchange(ref _acksCount, _acksCount - slot1.ACKsCount);

				CheckACKCount();

				//---- Add all the slots
				for (int index = 1; index < _slots.Count; index++)
				{
					SACKSlot slot = _slots[index];
					if (slot.ACKsCount > 0)
					{
						Interlocked.Exchange(ref _acksCount, _acksCount - slot.ACKsCount);
						slot.ACKsCount = 0;
						
						CheckACKCount();
						
						if (slot2 == null)
							slot2 = slot.Clone();
						else if (slot3 == null)
							slot3 = slot.Clone();
						else if (slot4 == null)
						{
							slot4 = slot.Clone();
							return;
						}
					}
				}
			}
		}

		#endregion

		#region ACKCount

		/// <summary>
		/// Calculate the number of ACKs to send.
		/// </summary>
		internal int ACKCount
		{
			get
			{
				return _acksCount;
			}
		}

		#endregion

		#region CheckACKCount

		[Conditional("CHECK_SACKSCOUNT")]
		private void CheckACKCount()
		{
			int count1 = 0;
			for (int index = 0; index < _slots.Count; index++)
				count1 += _slots[index].ACKsCount;

			if (count1 != _acksCount)
				Console.WriteLine("");
		}
		
		#endregion

	}
}
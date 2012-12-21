//#define CONSOLE_TRACE

using System;
using System.Collections.Generic;
using System.Text;
using System.Net.NetworkInformation;
using System.Diagnostics;
using System.Net;

namespace Helper.Net.RUDP
{
	/// <summary>
	/// An protocol for optimal MTU discovery and monitoring.
	/// </summary>
	internal sealed class PMTUDiscovery
	{
		#region Variables

		private int InterfaceMTU = -1;

		// Test the MTU each 3 minutes
		private const long DefaultProbeInterval = 3 * 60 * 1000 * 1000;

		// Next delayed probe
		private long _probeTS = -1;
		private int _probeMTU = 0;

		// Time out checking
		private long _lastProbeTS = -1;
		private int _mtuProbeResendCount = 0;

		// Min & Max
		static internal int MinMTU = 536;
		static internal int MaxMTU = 16 * 1024;

		// RUDP socket
		private RUDPSocket _rudp;

		#endregion

		#region PMTUDiscovery

		internal PMTUDiscovery(RUDPSocket rudp)
		{
			_rudp = rudp;

			// Start with minimum MTU to insure correct communication
			_rudp._mtu = MinMTU;
		}

		#endregion

		#region StartTuning

		internal void StartTuning()
		{
			// Start tuning with default MTU
			SendProbe(DiscoverInterfaceMTU());
		}

		#endregion

		#region DiscoverInterfaceMTU

		public int DiscoverInterfaceMTU()
		{
			if (InterfaceMTU > -1)
				return InterfaceMTU;

			int MTU = Int32.MaxValue;

			// Get MTU
			NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
			foreach (NetworkInterface adapter in nics)
			{
				if (!adapter.Supports(NetworkInterfaceComponent.IPv4))
					continue;

				IPInterfaceProperties adapterProperties = adapter.GetIPProperties();
				foreach (UnicastIPAddressInformation addrInfo in adapterProperties.UnicastAddresses)
				{
					if (addrInfo.Address.Equals(((IPEndPoint)_rudp._physical._socket.LocalEndPoint).Address))
					{
						IPv4InterfaceProperties p = adapterProperties.GetIPv4Properties();
						if (p != null)
							MTU = Math.Min(MTU, p.Mtu);
					}
				}
			}

			// IP header is already removed (20 bytes)

			InterfaceMTU = MTU;

			return InterfaceMTU;
		}

		#endregion

		#region SendProbe

		internal void SendProbe(int mtu)
		{
			//---- Avoid congestion
			_rudp._controlWindow.WaitOne();

			Trace("MTU:SendProbe:" + mtu);

			//---- Calculate full packet length
			byte[] buffer = new byte[mtu - RUDPStack.UDPHeaderLength - RUDPStack.RUDPHeaderLength];

			//---- Set the TS
			_lastProbeTS = HiResTimer.MicroSeconds;

			//---- Send a packet
			RUDPStack.PushPacketToSend(_rudp, false, RUDPPacketChannel.MTUTuning, buffer, 0, buffer.Length);
		}

		#endregion

		#region SendDelayedProbe

		internal void SendDelayedProbe(int mtu, long delayMicroSeconds)
		{
			_probeMTU = mtu;
			_probeTS = HiResTimer.MicroSeconds + delayMicroSeconds;
		}

		#endregion

		#region OnReceiveProbe

		internal void OnReceiveProbe(int payloadLength)
		{
			Trace("MTU:OnReceiveProbe:" + (payloadLength + RUDPStack.UDPHeaderLength + RUDPStack.RUDPHeaderLength));

			if (_rudp._status != RUDPSocketStatus.Connected)
				return;

			int mtu = payloadLength + RUDPStack.UDPHeaderLength + RUDPStack.RUDPHeaderLength;
			mtu = Math.Min(Math.Max(MinMTU, mtu), MaxMTU);

			//---- New possible MTU ?
			_rudp._mtu = Math.Max(_rudp._mtu, mtu);

			//---- Avoid congestion
			_rudp._controlWindow.WaitOne();

			//---- Send the ACK
			byte[] buffer = new byte[4];
			BinaryHelper.WriteInt(mtu, buffer, 0);

			// Send the ACK
			RUDPStack.PushPacketToSend(_rudp, false, RUDPPacketChannel.MTUTuningACK, buffer, 0, 4);
		}

		#endregion

		#region OnReceiveProbeACK

		internal void OnReceiveProbeACK(byte[] payload)
		{
			//---- We have a result for the packet
			_lastProbeTS = -1;
			_mtuProbeResendCount = 0;

			//---- Get the MTU of this probe
			int mtu = BinaryHelper.ReadInt(payload, 0);

			Trace("MTU:OnReceiveProbeACK:" + mtu);

			//---- Update MTU
			mtu = Math.Min(Math.Max(MinMTU, mtu), MaxMTU);
			_rudp._mtu = mtu;

			// Check that the window can handle at leat one packet
			if (_rudp._mtu > _rudp._controlWindow.CWND)
				_rudp._controlWindow.CWND = _rudp._mtu;

			//---- Continue to probe
			int probeMTU = (int)(_rudp._mtu * 1.25);
			if (probeMTU < MaxMTU)
				SendDelayedProbe(probeMTU, 5000000);
		}

		#endregion

		#region OnICMPError

		internal void OnICMPError(RUDPOutgoingPacket packet)
		{
			int errorMTU = packet.Payload.Length + RUDPStack.UDPHeaderLength + RUDPStack.RUDPHeaderLength;
			int nextMTU = (int)(errorMTU * 0.75);
			nextMTU = Math.Min(Math.Max(Math.Max(MinMTU, nextMTU),_rudp._mtu), MaxMTU);

			Trace("MTU:OnICMPError:" + errorMTU + " -> " + nextMTU);

			//---- We have a result for the packet
			_lastProbeTS = -1;
			_mtuProbeResendCount = 0;

			//---- User packet : URGENT
			if (packet.Channel == RUDPPacketChannel.UserPacket)
			{
				_rudp._mtu = nextMTU;
				SendProbe(_rudp._mtu);
				return;
			}

			//---- MTU probe
			if (packet.Channel == RUDPPacketChannel.MTUTuning)
			{
				if (errorMTU > _rudp._mtu && nextMTU < _rudp._mtu)
					nextMTU = _rudp._mtu;
				nextMTU = Math.Min(Math.Max(MinMTU, nextMTU), MaxMTU);
				SendDelayedProbe(nextMTU, DefaultProbeInterval);
				return;
			}
		}

		#endregion

		#region OnHeartBeat

		internal void OnHeartBeat(long now)
		{
			if (_rudp._status != RUDPSocketStatus.Connected)
				return;

			//---- Delayed send
			if (_probeTS > -1 && now >= _probeTS)
			{
				_probeTS = -1;
				SendProbe(_probeMTU);
			}

			//---- Check for time out
			if (_lastProbeTS > -1 && (now - _lastProbeTS) > _rudp._rto)
			{
				// Too much probe, current MTU is not correct
				if (_mtuProbeResendCount == 3)
				{
					_mtuProbeResendCount = 0;
					int probeMtu = (int)(_rudp._mtu * 0.75);
					probeMtu = Math.Min(Math.Max(MinMTU, probeMtu), MaxMTU);
					SendProbe(probeMtu);
					return;
				}

				// Check if the current MTU is still correct
				_mtuProbeResendCount++;
				SendProbe(_rudp._mtu);
			}
		}

		#endregion

		#region Trace

		[Conditional("CONSOLE_TRACE")]
		internal static void Trace(string text)
		{
			Console.WriteLine(text);
		}

		#endregion
	}
}

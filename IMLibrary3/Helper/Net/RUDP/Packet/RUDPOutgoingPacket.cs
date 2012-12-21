using System;
using System.Collections.Generic;
using System.Text;

namespace Helper.Net.RUDP
{
	sealed internal class RUDPOutgoingPacket
	{
		internal int PacketId;

		// Identify the first byte in the flow (like TCP)
		internal long Sequence;

		internal byte[] Payload;

		internal RUDPPacketChannel Channel;

		//---- State
		internal long TSFirstSend;	// Time Stamp : The first time we send the message
		internal long TSLastSend;	// The last time we send the message or resend it in milliseconds
		internal bool IsACKed;
		internal int Retransmission;

		//---- Information
		internal double CurrentCwnd;

		internal RUDPOutgoingPacket(int packetId, long sequence, byte[] payload, RUDPPacketChannel channel)
		{
			PacketId = packetId;
			Sequence = sequence;
			Payload = payload;
			Channel = channel;

			Reset();
		}

		internal void Reset()
		{
			TSFirstSend = -1; // Not yet sended
			TSLastSend = -1;
			IsACKed = false;
			Retransmission = 0;
			CurrentCwnd = 0;
		}

		public override string ToString()
		{
			return "PacketId(" + PacketId + ") Retransmission(" + Retransmission + ") IsACKed(" + IsACKed + ") Channel(" + Channel + ")";
		}
	}
}

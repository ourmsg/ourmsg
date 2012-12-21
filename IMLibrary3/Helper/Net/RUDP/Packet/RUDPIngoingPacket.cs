using System;
using System.Collections.Generic;
using System.Text;

namespace Helper.Net.RUDP
{
	sealed internal class RUDPIngoingPacket
	{
		internal RUDPSocket RUDP;
		internal int PacketId;
		internal RUDPPacketChannel Channel;
		internal byte[] Payload;
		internal long TSReceived; // Time Stamp : when message is received

		internal RUDPIngoingPacket(RUDPSocket rudp, int packetId, byte[] payload, RUDPPacketChannel channel, long tsReceived)
		{
			RUDP = rudp;
			PacketId = packetId;
			Channel = channel;
			Payload = payload;
			TSReceived = tsReceived;
		}

		public override string ToString()
		{
			return "PacketId(" + PacketId + ") Channel(" + Channel + ")";
		}
	}
}

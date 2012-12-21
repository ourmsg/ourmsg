using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace Helper.Net.RUDP
{
	/// <summary>
	/// An ACK slot (Can contains a range of AKCs)
	/// </summary>
	sealed internal class RUDPACKPacket
	{
		internal IPEndPoint RemoteEndPoint;
		internal RUDPPacketChannel Channel;
		internal int PacketId;
		internal bool IsConnectionAccepted;

		internal RUDPACKPacket(IPEndPoint remoteEndPoint, int packetId, RUDPPacketChannel channel, bool isConnectionAccepted)
		{
			RemoteEndPoint = remoteEndPoint;
			Channel = channel;
			IsConnectionAccepted = isConnectionAccepted;
			PacketId = packetId;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Helper.Net.RUDP
{
	internal enum RUDPPacketChannel
	{
		Undefined = 0,
		
		Ping = 10,
		PingRendezVous = 20,
		TearDown = 30,

		UserPacket = 40,
		ACK = 50,

		OutOfOrder = 70,

		KeepAlive = 100,

		MTUTuning = 120,
		MTUTuningACK = 121,

		Bandwidth01 = 150,
		BandwidthResponse01 = 151,
		Bandwidth02 = 155,
		BandwidthResponse02 = 156
	}
}
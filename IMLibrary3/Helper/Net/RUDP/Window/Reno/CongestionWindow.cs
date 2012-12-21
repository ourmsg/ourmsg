using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Helper.Net.RUDP.Reno
{
	sealed internal class CongestionWindow : Helper.Net.RUDP.Tahoe.CongestionWindow
	{

		#region Constructor

		internal CongestionWindow(RUDPSocket rudp)
			: base(rudp)
		{
		}

		#endregion

		#region OnEndFastRetransmit

		override internal void OnEndFastRetransmit()
		{
			//---- Fast recovery
			// After the fast retransmit algorithm sends what appears to be the
			// missing segment, the "fast recovery" algorithm governs the
			// transmission of new data until a non-duplicate ACK arrives.

			CWND = _ssthresh + _outOfOrderCount;
			_ssthresh = Math.Max(CWND / 2, 2 * _rudp._mtu);
			//_ssthresh = Math.Max(_awnd / 2, 2 * _rudp._mtu);
			
			_outOfOrderCount = 0;

			Phase = Helper.Net.RUDP.Tahoe.Phase.SlowStart;
		}

		#endregion

	}

}

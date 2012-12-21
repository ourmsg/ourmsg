using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Helper.Net.RUDP.Tahoe
{
	internal enum Phase
	{
		SlowStart,
		CongestionAvoidance,
		FastRetransmit
	}

	/// <summary>
	/// Based on RFC2581 : http://rfc.dotsrc.org/rfc/rfc2581.html
	/// & Jacobson88 : http://www.run.montefiore.ulg.ac.be/cours/srm/tp1/jacobson88.ps
	/// </summary>
	internal class CongestionWindow : AbstractWindow
	{

		#region Variables

		internal Phase Phase;

		// Slow-Start Threshold
		internal double _ssthresh;

		internal readonly int _MaxSSthresh = 64 * 1024;

		internal int _outOfOrderCount = 0;

		#endregion

		#region Constructor

		internal CongestionWindow(RUDPSocket rudp)
			: base(rudp)
		{
		}

		#endregion

		#region Reset

		internal override void Reset()
		{
			base.Reset();

			_cwnd = _rudp._mtu;

			Phase = Phase.SlowStart;
			_ssthresh = _MaxSSthresh;
		}

		#endregion

		#region OnACK_UpdateWindow

		internal override void OnACK_UpdateWindow(RUDPOutgoingPacket packet)
		{
			//---- Reset
			_outOfOrderCount = 0;

			//---- Check the phase
			if (CWND <= _ssthresh)
				Phase = Phase.SlowStart;
			else
				Phase = Phase.CongestionAvoidance;

			//---- Slow start
			if (Phase == Phase.SlowStart)
			{
				// Exponential grow
				CWND += _rudp.MTU;
			}

			//---- Congestion avoidance
			if (Phase == Phase.CongestionAvoidance)
			{
				// (increase of 1 packet every RTT)
				// This is a linear growth of cwnd.
				CWND += (_rudp.MTU * _rudp.MTU) / CWND;
			}

			//---- Check boundaries
			CWND = Math.Max(Math.Min(_awnd, _cwnd), _rudp._mtu);

			//---- Update slow start threshold
			if (_ssthresh < CWND)
				_ssthresh = Math.Min(64 * 1024, CWND);
		}

		#endregion

		#region OnTimeOut_UpdateWindow

		internal override void OnTimeOut_UpdateWindow()
		{
			//---- Reset
			_outOfOrderCount = 0;
			
			//---- Check the phase
			if (CWND <= _ssthresh)
				Phase = Phase.SlowStart;
			else
				Phase = Phase.CongestionAvoidance;

			//---- Slow start
			if (Phase == Phase.SlowStart)
			{
				CWND = _rudp._mtu;
				_ssthresh = Math.Max(2 * _rudp._mtu, FlightSize / 2);
			}

			//---- Congestion avoidance
			if (Phase == Phase.CongestionAvoidance)
			{
				_ssthresh = Math.Max(2 * _rudp._mtu, FlightSize / 2);
				CWND = _rudp._mtu;
			}
		}

		#endregion

		#region OnOutOfOrder

		internal override void OnOutOfOrder(int startPacketId, int endPacketId)
		{
			_outOfOrderCount++;

			if (_outOfOrderCount < 2)
				return;

			//---- Fast retransmit
			// Performs a retransmission of what appears to be
			// the missing segment, without waiting for the retransmission timer to
			// expire.
			Phase = Phase.FastRetransmit;

			_ssthresh = Math.Max(FlightSize / 2, 2 * _rudp._mtu);
			_ssthresh = Math.Min(_MaxSSthresh, _ssthresh);

			CWND = _ssthresh + _outOfOrderCount * _rudp._mtu;

			//---- Mark the packets for direct retransmission
			_rudp.StartFastRetransmit(startPacketId, endPacketId);
		}

		#endregion

		#region OnEndFastRetransmit

		override internal void OnEndFastRetransmit()
		{
			//---- Fast recovery
			// After the fast retransmit algorithm sends what appears to be the
			// missing segment, the "fast recovery" algorithm governs the
			// transmission of new data until a non-duplicate ACK arrives.
			_ssthresh = Math.Max(CWND / 2, 2 * _rudp._mtu);
			CWND = _rudp._mtu;
			_outOfOrderCount = 0;
			Phase = Phase.SlowStart;
		}

		#endregion

	}

}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace Helper.Net.RUDP.CUBIC
{

	//http://www4.ncsu.edu/~rhee/export/bitcp/index.htm
	// http://lwn.net/Articles/128627/
	sealed internal class CongestionWindow : AbstractWindow
	{
		#region Variables

		double last_max;
		double loss_cwnd;
		double epoch_start;
		double ssthresh;
		double b;
		double c;

		#endregion

		#region Constructor

		internal CongestionWindow(RUDPSocket rudp)
			: base(rudp)
		{
			_rudp = rudp;
		}

		#endregion

		#region Reset

		internal override void Reset()
		{
			base.Reset();

			last_max = 0;
			loss_cwnd = 0;
			epoch_start = 0;
			ssthresh = 64 * 1024;
			b = 2.5;
			c = 0.4;
		}

		#endregion

		#region OnACK_UpdateWindow

		internal override void OnACK_UpdateWindow(RUDPOutgoingPacket packet)
		{
			delay_min = Math.Min(_rudp._rtt, delay_min);
			if (CWND < ssthresh)
				CWND++; //slow start
			else
			{
				if (epoch_start == 0)
				{
					epoch_start = HiResTimer.MicroSeconds;
					K = Math.Max(0, Math.Pow(b * (last_max - CWND), 1.0 / 3));
					origin_point = Math.Max(CWND, last_max);
				}

				t = HiResTimer.MicroSeconds + delay_min - epoch_start;
				target = origin_point + c * Math.Pow(t - K, 1.0 / 3);

				if (target > CWND)
					cnt = CWND / (target - CWND);
				else
					cnt = 100 * CWND;

				if (delay_min > 0)
					cnt = Math.Max(cnt, 8 * CWND / (20 * delay_min)); //max AI rate

				if (loss_cwnd == 0)
					cnt = 50; // continue exponential increase before first backoff

				if (cwnd_cnt > cnt)
				{
					CWND++;
					cwnd_cnt = 0;
				}
				else
					cwnd_cnt++;
			}
		}

		#endregion

		#region OnTimeOut_UpdateWindow

		internal override void OnTimeOut_UpdateWindow()
		{
			epoch_start = 0;
			if (CWND < last_max)
				last_max = 0.9 * CWND;
			else
				last_max = CWND;

			loss_cwnd = CWND;
			CWND = 0.8 * CWND; // backoff cwnd by 0.8
		}

		#endregion

		double delay_min;
		double K;
		double origin_point;
		double cnt;
		double t;
		double target;
		double cwnd_cnt;

	}

}

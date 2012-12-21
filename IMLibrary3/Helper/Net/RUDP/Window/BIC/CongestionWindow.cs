using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Helper.Net.RUDP.BIC
{

	//http://www4.ncsu.edu/~rhee/export/bitcp/index.htm
	// http://lwn.net/Articles/128627/
	sealed internal class CongestionWindow : AbstractWindow
	{

		#region Variables

		double beta;

		double Smax;
		double Smin;

		// if the window size is larger than this threshold, BI-TCP engages; otherwise
		// normal TCP increase/decrease.
		double low_window;

		// default maximum (a large integer)
		double default_max_win;

		//
		double max_win;

		//
		double min_win;

		//
		double prev_max;

		//
		double target_win;

		// Boolean indicating whether the protocol is in the slow start. Initially false.
		bool is_BITCP_ss;

		// a variable to keep track of cwnd increase during the BI-TCP slow start.
		double ss_cwnd;

		// the value of cwnd after one _rtt in BI-TCP slow start.
		double ss_target;

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

			beta = 0.8;
			low_window = 64 * 1024;
			Smax = 64 * 1024;
			Smin = 1 * 1024;
			default_max_win = 256 * 1024;
			max_win = default_max_win;
			min_win = 16 * 1024;
			is_BITCP_ss = false;
		}

		#endregion

		#region OnACK_UpdateWindow

		internal override void OnACK_UpdateWindow(RUDPOutgoingPacket packet)
		{
			if (CWND < low_window)
			{
				CWND = CWND + _rudp.MTU / CWND; // normal TCP = (1 packet/cwnd)
				return;
			}

			if (!is_BITCP_ss)
			{
				// bin. increase
				if ((target_win - CWND) < Smax) // bin. search
					CWND += (target_win - CWND) / CWND;
				else
					CWND += Smax / CWND; // additive incre.

				if (max_win > CWND)
				{
					min_win = CWND;
					target_win = (max_win + min_win) / 2;
				}
				else
				{
					is_BITCP_ss = true;
					ss_cwnd = 1;
					ss_target = CWND + 1;
					max_win = default_max_win;
				}
			}
			else
			{
				// slow start
				CWND = CWND + ss_cwnd / CWND;
				if (CWND >= ss_target)
				{
					ss_cwnd = 2.0 * ss_cwnd;
					ss_target = CWND + ss_cwnd;
				}
				if (ss_cwnd >= Smax)
					is_BITCP_ss = false;
			}
		}

		#endregion

		#region OnTimeOut_UpdateWindow

		internal override void OnTimeOut_UpdateWindow()
		{
			// Enter fast recovery
			if (low_window <= CWND)
			{
				prev_max = max_win;
				max_win = CWND;
				CWND = CWND * (1.0 - beta);
				min_win = CWND;
				if (prev_max > max_win) //Fast. Conv.
					max_win = (max_win + min_win) / 2;
				target_win = (max_win + min_win) / 2;
			}
			else
			{
				CWND = CWND * 0.5; // normal TCP
			}
		}

		#endregion

	}

}

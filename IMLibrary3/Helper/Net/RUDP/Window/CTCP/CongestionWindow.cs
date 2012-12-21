using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Helper.Net.RUDP.CTCP
{

	//sealed internal class CongestionWindow : AbstractWindow
	//{

	//    #region Variables

	//    //---- AIMD
	//    private int CA_THRESH = 4;
	//    private int FC_THRESH = 4;

	//    internal int _ssthresh;

	//    internal double _dwnd;

	//    internal long _win;

	//    int _gamma = 30; // 30 packets
	//    int _gammaLow = 5;
	//    int _gammaHigh = 30;

	//    #endregion

	//    #region Constructor

	//    internal CongestionWindow(RUDPSocket rudp)
	//        : base(rudp)
	//    {
	//        _rudp = rudp;
	//    }

	//    #endregion

	//    #region Reset

	//    internal override void Reset()
	//    {
	//        base.Reset();

	//        _ssthresh = 30 * 1024;
	//        _dwnd = 0;
	//    }

	//    #endregion

	//    #region OnSend_UpdateWindow

	//    override internal void OnSend_UpdateWindow(int payloadLength)
	//    {
	//        /*
	//        bool idle = _rudp._outgoingPackets.Count < 1;
	//        if (idle && (HiResTimer.MicroSeconds - _rudp._lastSendTS) > _rudp._rto)
	//            _cwnd = 2 * _rudp._mtu;
	//        */
	//    }

	//    #endregion

	//    #region OnACK_UpdateWindow

	//    internal override void OnACK_UpdateWindow(RUDPOutgoingPacket packet)
	//    {
	//        if (_cwnd <= _ssthresh)
	//        {
	//            //-- Slow start
	//            // Exponential grow : quick start
	//            // cwnd = cwnd + 1;
	//            _cwnd += 1 / (_cwnd + _dwnd);
	//        }
	//        else
	//        {
	//            //-- Performing congestion avoidance
	//            // This is a linear growth of cwnd.
	//            // During congestion avoidance, cwnd is incremented by 1 full-sized
	//            // segment per round-trip time (_rtt).
	//            // cwnd = cwnd + SMSS*SMSS/cwnd
	//            _cwnd += _rudp.MTU * _rudp.MTU / _cwnd;
	//        }

	//        //----
	//        // baseRTT is updated by the minimal RTT that has been observed
	//        expected = _win / _baseRTT;
	//        actuel = _win / rtt;
	//        diff = (expected - actual) * baseRtt;
			
	//        //---- dwnd
	//        if (diff < gamma)
	//        {
	//            _dwnd = _dwnd + Math.Max(alpha * _win ^ k - 1, 0);
	//        }
	//        else
	//        {
	//            _dwnd = Math.Max(_dwnd - epsilon * diff, 0);
	//        }

	//        _win = _cwnd + _dwnd;
	//    }

	//    #endregion

	//    #region OnTimeOut_UpdateWindow

	//    internal override void OnTimeOut_UpdateWindow()
	//    {
	//        // amount of data that has been sent but not yet acknowledged (acked).
	//        int FlightSize = (int)(_cwnd - _ssthresh);

	//        if (_cwnd < _ssthresh ||
	//            _ssthresh < CA_THRESH ||
	//            FlightSize < FC_THRESH)
	//        {
	//            // Slow down
	//            _ssthresh = Math.Max(Math.Min(_rudp._sendSize, (int)_cwnd) / 2, 2);
	//            _cwnd = 2 * _rudp._mtu;
	//        }
	//        else
	//        {
	//            //---- Fast retransmit / Fast recovery
	//            _ssthresh = (int)(_cwnd - _ssthresh / 2);
	//            //Math.Min(_rudp._sendSize / 2, Math.Max(_ssthresh, 2));
	//            _cwnd = _ssthresh;
	//        }

	//        _gamma = 3.0 / 4 * diff_reno;
	//        _gamma = Math.Max(Math.Min(_gamma, _gammaHigh), _gammaLow);

	//        _dwnd = Math.Max(_win * (1.0 - beta) - _cwnd / 2, 0);

	//        _win = _cwnd + _dwnd;
	//    }

	//    #endregion

	//}

}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

using Helper.ThreadingNET35;

namespace Helper.Net.RUDP
{

	#region RUDPSocketStatus

	internal enum RUDPSocketStatus
	{
		Connecting,		// During the connection phase
		Connected,		// We are connected
		Closing,		// During the closing phase
		ClosingACKed,	// When the tear down message has been ACKed
		Closed,			// The connection is closed / It is the default status
		Accepting
	}

	#endregion

	#region RUDPSocketError

	/// <summary>
	/// For more information about socket errors :
	/// http://support.microsoft.com/default.aspx?scid=kb;en-us;819124
	/// </summary>
	public enum RUDPSocketError
	{
		// Summary:
		//     An unspecified System.Net.Sockets.ServerSocket error has occurred.
		SocketError = -1,
		//
		// Summary:
		//     The System.Net.Sockets.ServerSocket operation succeeded.
		Success = 0,
		//
		// Summary:
		//     The overlapped operation was aborted due to the closure of the System.Net.Sockets.ServerSocket.
		//OperationAborted = 995,
		//
		// Summary:
		//     The application has initiated an overlapped operation that cannot be completed
		//     immediately.
		//IOPending = 997,
		//
		// Summary:
		//     A blocking System.Net.Sockets.ServerSocket call was canceled.
		//Interrupted = 10004,
		//
		// Summary:
		//     An attempt was made to access a System.Net.Sockets.ServerSocket in a way that is
		//     forbidden by its access permissions.
		//AccessDenied = 10013,
		//
		// Summary:
		//     An invalid pointer address was detected by the underlying socket provider.
		//Fault = 10014,
		//
		// Summary:
		//     An invalid argument was supplied to a System.Net.Sockets.ServerSocket member.
		//InvalidArgument = 10022,
		//
		// Summary:
		//     There are too many open sockets in the underlying socket provider.
		//TooManyOpenSockets = 10024,
		//
		// Summary:
		//     An operation on a nonblocking socket cannot be completed immediately.
		//WouldBlock = 10035,
		//
		// Summary:
		//     A blocking operation is in progress.
		InProgress = 10036,
		//
		// Summary:
		//     The nonblocking System.Net.Sockets.ServerSocket already has an operation in progress.
		AlreadyInProgress = 10037,
		//
		// Summary:
		//     A System.Net.Sockets.ServerSocket operation was attempted on a non-socket.
		//NotSocket = 10038,
		//
		// Summary:
		//     A required address was omitted from an operation on a System.Net.Sockets.ServerSocket.
		//DestinationAddressRequired = 10039,
		//
		// Summary:
		//     The datagram is too long.
		//MessageSize = 10040,
		//
		// Summary:
		//     The protocol type is incorrect for this System.Net.Sockets.ServerSocket.
		//ProtocolType = 10041,
		//
		// Summary:
		//     An unknown, invalid, or unsupported option or level was used with a System.Net.Sockets.ServerSocket.
		//ProtocolOption = 10042,
		//
		// Summary:
		//     The protocol is not implemented or has not been configured.
		//ProtocolNotSupported = 10043,
		//
		// Summary:
		//     The support for the specified socket type does not exist in this address
		//     family.
		//SocketNotSupported = 10044,
		//
		// Summary:
		//     The address family is not supported by the protocol family.
		//OperationNotSupported = 10045,
		//
		// Summary:
		//     The protocol family is not implemented or has not been configured.
		//ProtocolFamilyNotSupported = 10046,
		//
		// Summary:
		//     The address is incompatible with the requested protocol.
		//AddressFamilyNotSupported = 10047,
		//
		// Summary:
		//     Only one use of an address is normally permitted.
		//AddressAlreadyInUse = 10048,
		//
		// Summary:
		//     The selected address is valid in this context.
		//AddressNotAvailable = 10049,
		//
		// Summary:
		//     The network is not available.
		NetworkDown = 10050,
		//
		// Summary:
		//     No route to the remote host exists.
		NetworkUnreachable = 10051,
		//
		// Summary:
		//     The application tried to set System.Net.Sockets.SocketOptionName.KeepAlive
		//     on a connection that has already timed out.
		NetworkReset = 10052,
		//
		// Summary:
		//     The connection was aborted by the .NET Framework or the underlying socket
		//     provider.
		//ConnectionAborted = 10053,
		//
		// Summary:
		//     The connection was reset by the remote peer.
		ConnectionReset = 10054,
		//
		// Summary:
		//     No free buffer space is available for a System.Net.Sockets.ServerSocket operation.
		NoBufferSpaceAvailable = 10055,
		//
		// Summary:
		//     The System.Net.Sockets.ServerSocket is already connected.
		IsConnected = 10056,
		//
		// Summary:
		//     The application tried to send or receive data, and the System.Net.Sockets.ServerSocket
		//     is not connected.
		NotConnected = 10057,
		//
		// Summary:
		//     A request to send or receive data was disallowed because the System.Net.Sockets.ServerSocket
		//     has already been closed.
		Shutdown = 10058,
		//
		// Summary:
		//     The connection attempt timed out, or the connected host has failed to respond.
		TimedOut = 10060,
		//
		// Summary:
		//     The remote host is actively refusing a connection.
		ConnectionRefused = 10061,
		//
		// Summary:
		//     The operation failed because the remote host is down.
		HostDown = 10064,
		//
		// Summary:
		//     There is no network route to the specified host.
		HostUnreachable = 10065,
		//
		// Summary:
		//     Too many processes are using the underlying socket provider.
		ProcessLimit = 10067,
		//
		// Summary:
		//     The network subsystem is unavailable.
		SystemNotReady = 10091,
		//
		// Summary:
		//     The version of the underlying socket provider is out of range.
		//VersionNotSupported = 10092,
		//
		// Summary:
		//     The underlying socket provider has not been initialized.
		NotInitialized = 10093,
		//
		// Summary:
		//     A graceful shutdown is in progress.
		Disconnecting = 10101,
		//
		// Summary:
		//     The specified class was not found.
		//TypeNotFound = 10109,
		//
		// Summary:
		//     No such host is known. The name is not an official host name or alias.
		HostNotFound = 11001,
		//
		// Summary:
		//     The name of the host could not be resolved. Try again later.
		//TryAgain = 11002,
		//
		// Summary:
		//     The error is unrecoverable or the requested database cannot be located.
		//NoRecovery = 11003,
		//
		// Summary:
		//     The requested name or IP address was not found on the name server.
		NoData = 11004,
	}

	#endregion

	#region RUDPSocketException

	sealed public class RUDPSocketException : Exception
	{
		public RUDPSocketError Error;

		public RUDPSocketException(RUDPSocketError error)
		{
			Error = error;
		}
	}

	#endregion

	#region RUDPSocketNetworkInformation

	sealed public class RUDPSocketNetworkInformation
	{

		#region Variables

		private RUDPSocket _rudp;

		#endregion

		#region Constructor

		internal RUDPSocketNetworkInformation(RUDPSocket rudp)
		{
			_rudp = rudp;
		}

		#endregion

		#region Properties

		public int PathMTU
		{
			get
			{
				return _rudp._mtu;
			}
		}

		public double CongestionWindow
		{
			get
			{
				return _rudp._controlWindow.CWND;
			}
		}

		public double RTT
		{
			get
			{
				return _rudp._rtt;
			}
		}

		public double RTO
		{
			get
			{
				return _rudp._rto;
			}
		}

		/// <summary>
		/// Estimated Bandwidth in Bytes/Milli-seconds
		/// </summary>
		public long BandWidth
		{
			get
			{
				return _rudp._bandwidth;
			}
		}

		#endregion

	}

	#endregion

	#region RUDPCongestionControl

	public enum RUDPCongestionControl
	{
		// The older version of TCP
		TCPTahoe,

		TCPReno,

		// Experimental, but by default on some linux implementation
		TCPBIC,
		TCPCUBIC,
		UDT,

		// For test purpose
		Simple01
	}

	#endregion

	sealed public class RUDPSocket
	{

		#region Variables

		//---- Global
		static internal int LockTimeOut = -1;

		static internal int DefaultConnectionTimeOut = 60000;

		static internal int LastHandle = 0;

		//---- The control thread that manage this socket
		internal int _controlThreadId = -1;

		//---- ServerSocket data
		internal PhysicalSocket _physical;
		internal IPEndPoint _remoteEndPoint;
		internal bool _isRendezVousMode = false;

		//---- State of the socket
		internal RUDPSocketStatus _status = RUDPSocketStatus.Closed;
		internal bool _isShutingDown = false;

		//---- An internal handle
		internal int _handle;

		//----- Asynchronous call datas
		internal RUDPAcceptIAsyncResult _asyncResultAccept;
		internal RUDPConnectIAsyncResult _asyncResultConnect;
		internal RUDPReceiveIAsyncResult _asyncResultReceive;

		//----- Identify a byte in the flow (like TCP)
		internal long _sequence = 0;

		//---- Fragments
		internal ReaderWriterLockSlim _fragmentsLock = new ReaderWriterLockSlim();
		internal LinkedList<FragmentInformation> _fragments = new LinkedList<FragmentInformation>();

		//---- List of outgoing and ingoing packets
		internal ReaderWriterLockSlim _outgoingPacketsLock = new ReaderWriterLockSlim();
		internal List<RUDPOutgoingPacket> _outgoingPackets = new List<RUDPOutgoingPacket>();

		// incoming packets
		internal Object _handleNextUserPacketLock = new Object();
		internal IngoingList _incomingPackets = new IngoingList();

		//---- List of incoming connections sockets
		internal List<RUDPSocket> _acceptedRUDPSockets = new List<RUDPSocket>();

		//---- List of outgoing and ingoing messages
		internal int _ougoingPacketId;

		//---- Receive / Send size
		internal int _receiveSize = 100 * 1024;
		internal int _sendSize = 100 * 1024;

		//---- ACKs management
		internal SACKWindow _sackWindow;

		// Last time an ACK has been sended
		internal long _lastACKSendTS = -1;

		//---- MTU
		internal bool _usePMTUDiscovery = true;
		internal PMTUDiscovery _pmtuDiscovery;
		internal int _mtu = -1;

		//---- Time Stamp : Last time we send a packet
		internal long _lastSendTS;

		//---- Time Stamp : for bandwidth calculation
		internal long _lastBandwidthTS;
		internal long _bandwidthResponse01TS;
		internal long _bandwidth; // byte / ms

		//---- RTT & Time outs

		// RTT (Round Trip Time: time needed to send a message and receive its ACK)
		internal double _rtt;

		// RTO (Retransmission Time Out)
		internal double _rto;

		// Delta RTT (RTT difference)
		internal double _deltaRtt;

		// STO (send Time Out)
		internal long _sto;

		//---- Windows
		internal AbstractWindow _controlWindow;

		//---- Fast retransmit
		internal int _fastRetransmitStartPacketId = -1;
		internal int _fastRetransmitEndPacketId = -1;

		//---- Some statistics
		private RUDPSocketNetworkInformation _networkInformation;
#if STATISTICS
		internal int Stats_ReceivedMessagesCount = 0;
		internal int Stats_SendedMessagesCount = 0;
		internal int Stats_ResendedMessagesCount = 0;
		internal int Stats_SendedACKCount = 0;
		internal int Stats_LastQueuedSendedACKId = 0;
		internal int Stats_LastSendedACKId = 0;
		internal int Stats_ReceivedACKCount = 0;
		internal int Stats_LastReceivedACKId = 0;
#endif
		#endregion

		#region Constructor

		public RUDPSocket()
			: this(RUDPCongestionControl.TCPReno)
		{
		}

		public RUDPSocket(RUDPCongestionControl congestionControl)
		{
			switch (congestionControl)
			{
				case RUDPCongestionControl.TCPTahoe:
					_controlWindow = new Helper.Net.RUDP.Tahoe.CongestionWindow(this);
					break;
				case RUDPCongestionControl.TCPReno:
					_controlWindow = new Helper.Net.RUDP.Reno.CongestionWindow(this);
					break;
				case RUDPCongestionControl.TCPBIC:
					_controlWindow = new Helper.Net.RUDP.BIC.CongestionWindow(this);
					break;
				case RUDPCongestionControl.TCPCUBIC:
					_controlWindow = new Helper.Net.RUDP.CUBIC.CongestionWindow(this);
					break;
			}

			//---- Reset
			Reset(RUDPSocketStatus.Closed);

			//---- Update handle
			_handle = LastHandle;
			LastHandle++;

			//---- Setup network information
			_networkInformation = new RUDPSocketNetworkInformation(this);
		}

		internal void Reset(RUDPSocketStatus status)
		{
			RUDPStack.Trace("RESET PeerInformation");

			//---- Reset all
			_status = status;

			_fastRetransmitStartPacketId = -1;
			_fastRetransmitEndPacketId = -1;

			_ougoingPacketId = -1;

			_sequence = 0;

			_lastSendTS = 0;
			_lastACKSendTS = -1;

			_lastBandwidthTS = 0;
			_bandwidthResponse01TS = 0;
			_bandwidth = 0;

			//---- Clear outgoing
			_outgoingPacketsLock.EnterWriteLock();
			for (int index = 0; index < _outgoingPackets.Count; index++)
				lock (_outgoingPackets[index])
					_outgoingPackets[index].IsACKed = true;
			_outgoingPacketsLock.ExitWriteLock();

			while (_outgoingPackets.Count > 0)
				Thread.Sleep(1);

			_incomingPackets.Clear();

			_sackWindow = new SACKWindow();
			_pmtuDiscovery = new PMTUDiscovery(this);
			_controlWindow.Reset();

			// Reset "control thread id"
			RUDPStack.UnregisterRUDPSocket(this);
		}

		#endregion

		#region Bind

		public void Bind(IPEndPoint endPoint)
		{
			// Get the associated physical socket
			_physical = RUDPStack.GetInstance(endPoint);
		}

		private void AutomaticBind()
		{
			IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);
			Bind(endPoint);
		}

		#endregion

		#region Listen

		public void Listen(int max)
		{
			// Nothing to do !
		}

		#endregion

		#region Connect

		public void Connect(IPEndPoint endPoint)
		{
			IAsyncResult result = BeginConnect(endPoint, null, null);
			EndConnect(result);

			if (_status == RUDPSocketStatus.Closed)
				throw new SocketException();
		}

		public IAsyncResult BeginConnect(IPEndPoint remoteEP,
											AsyncCallback callback,
											Object state)
		{
			if (_physical == null)
				AutomaticBind();

			RUDPConnectIAsyncResult asyncResult = new RUDPConnectIAsyncResult(this, callback, state);
			Interlocked.Exchange<RUDPConnectIAsyncResult>(ref _asyncResultConnect, asyncResult);

			_remoteEndPoint = remoteEP;
			RUDPSocketError result = _physical.BeginConnect(this, DefaultConnectionTimeOut);
			if (result != RUDPSocketError.Success)
			{
				Interlocked.Exchange<RUDPConnectIAsyncResult>(ref _asyncResultConnect, null);
				_remoteEndPoint = null;
				throw new RUDPSocketException(result);
			}

			return asyncResult;
		}

		public void EndConnect(IAsyncResult asyncResult)
		{
			((RUDPConnectIAsyncResult)asyncResult).EndInvoke(true);
		}

		#endregion

		#region Accept

		public RUDPSocket Accept()
		{
			IAsyncResult result = BeginAccept(null, null);
			return EndAccept(result);
		}

		public IAsyncResult BeginAccept(AsyncCallback callback, Object state)
		{
			RUDPAcceptIAsyncResult asyncResult = new RUDPAcceptIAsyncResult(this, callback, state);
			Interlocked.Exchange<RUDPAcceptIAsyncResult>(ref _asyncResultAccept, asyncResult);

			//---- Check if we do not already have a socket
			if (_acceptedRUDPSockets.Count > 0)
			{
				RUDPSocket rudp = _acceptedRUDPSockets[0];
				lock (_acceptedRUDPSockets)
					_acceptedRUDPSockets.RemoveAt(0);

				OnEndAccept(rudp);
			}
			else
			{
				//-- Request an accept
				_physical.BeginAccept(this, callback, state);
			}

			return asyncResult;
		}

		public RUDPSocket EndAccept(IAsyncResult asyncResult)
		{
			RUDPAcceptIAsyncResult result = asyncResult as RUDPAcceptIAsyncResult;
			result.EndInvoke(true);
			return result.AcceptedSocket;
		}

		#endregion

		#region Send

		public int Send(byte[] buffer,
						int offset,
						int size)
		{
			RUDPSocketError errorCode = RUDPSocketError.Success;
			return Send(buffer, offset, size, out errorCode, true);
		}

		public int Send(byte[] buffer,
						int offset,
						int size,
						out RUDPSocketError errorCode)
		{
			return Send(buffer, offset, size, out errorCode, true);
		}

		public int Send(byte[] buffer,
						int offset,
						int size,
						out RUDPSocketError errorCode,
						bool reliable)
		{
			errorCode = RUDPStack.SendPayload(this, buffer, offset, size, reliable, null);

			if (errorCode != RUDPSocketError.Success)
				return -1;

			return size;
		}

		public IAsyncResult BeginSend(byte[] buffer,
										int offset,
										int size,
										out RUDPSocketError errorCode,
										AsyncCallback callback,
										Object state)
		{
			return BeginSend(buffer, offset, size, out errorCode, callback, state, true);
		}

		public IAsyncResult BeginSend(byte[] buffer,
										int offset,
										int size,
										out RUDPSocketError errorCode,
										AsyncCallback callback,
										Object state,
										bool reliable)
		{
			RUDPSendIAsyncResult asyncResult = new RUDPSendIAsyncResult(this, callback, state, size);

			errorCode = RUDPStack.SendPayload(this, buffer, offset, size, reliable, asyncResult);

			if (errorCode != RUDPSocketError.Success)
				return null;

			return asyncResult;
		}

		public int EndSend(IAsyncResult asyncResult)
		{
			RUDPSendIAsyncResult result = asyncResult as RUDPSendIAsyncResult;
			result.EndInvoke(false);

			if (result.SocketError != RUDPSocketError.Success)
				return -1;

			return result._size;
		}

		#endregion

		#region Receive

		public byte[] Receive()
		{
			SocketError error = SocketError.Success;
			IAsyncResult result = BeginReceive(out error, null, null);
			return EndReceive(result);
		}

		public byte[] Receive(out SocketError errorCode)
		{
			IAsyncResult result = BeginReceive(out errorCode, null, null);
			return EndReceive(result);
		}

		public IAsyncResult BeginReceive(AsyncCallback callback,
									Object state)
		{
			SocketError errorCode = SocketError.Success;
			return BeginReceive(out errorCode, callback, state);
		}

		public IAsyncResult BeginReceive(out SocketError errorCode,
										AsyncCallback callback,
										Object state)
		{
			RUDPReceiveIAsyncResult asyncResult = new RUDPReceiveIAsyncResult(this, callback, state);
			Interlocked.Exchange<RUDPReceiveIAsyncResult>(ref _asyncResultReceive, asyncResult);

			errorCode = SocketError.Success;

			//---- Check if we do not already have packets
			HandleNextUserPacket(true);

			return asyncResult;
		}

		/// <summary>
		/// We directly return the payload to avoid memory copy.
		/// </summary>
		public byte[] EndReceive(IAsyncResult asyncResult)
		{
			RUDPReceiveIAsyncResult result = asyncResult as RUDPReceiveIAsyncResult;
			result.EndInvoke(false);

			if (result.SocketError != RUDPSocketError.Success)
				return null;

			return result.Packet.Payload;
		}

		#endregion

		#region Close

		public void Close()
		{
			_physical.Close(this);
		}

		#endregion

		#region Shutdown

		public void Shutdown()
		{
			_physical.Shutdown(this);
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets a value that indicates whether a ServerSocket is connected to a remote host as
		/// of the last Send or Receive operation.
		/// </summary>
		public bool Connected
		{
			get
			{
				return (_status == RUDPSocketStatus.Connected);
			}
		}

		public bool IsRendezVousMode
		{
			get
			{
				return _isRendezVousMode;
			}
			set
			{
				_isRendezVousMode = value;
			}
		}

		public int MTU
		{
			get
			{
				return _mtu;
			}
			set
			{
				_mtu = value;
			}
		}

		public bool UsePMTUDiscovery
		{
			get
			{
				return _usePMTUDiscovery;
			}
			set
			{
				_usePMTUDiscovery = value;
			}
		}

		public int Handle
		{
			get
			{
				return _handle;
			}
		}

		public RUDPSocketNetworkInformation NetworkInformation
		{
			get
			{
				return _networkInformation;
			}
		}

		/// <summary>
		/// Gets or sets a value that specifies the size of the receive buffer of the Socket.
		/// </summary>
		public int ReceiveBufferSize
		{
			get
			{
				return _receiveSize;
			}
			set
			{
				_receiveSize = value;
			}
		}

		/// <summary>
		/// Gets or sets a value that specifies the size of the send buffer of the Socket.
		/// </summary>
		public int SendBufferSize
		{
			get
			{
				return _sendSize;
			}
			set
			{
				_sendSize = value;
			}
		}

		/// <summary>
		/// Round Trip Time
		/// </summary>
		public double RTT
		{
			get
			{
				return Thread.VolatileRead(ref _rtt);
			}
			set
			{
				Thread.VolatileWrite(ref _rto, value);
			}
		}

		/// <summary>
		/// Delta between 2 Round Trip Times
		/// </summary>
		public double DeltaRTT
		{
			get
			{
				return Thread.VolatileRead(ref _deltaRtt);
			}
			set
			{
				Thread.VolatileWrite(ref _rto, value);
			}
		}

		/// <summary>
		/// Retransmission Time Out
		/// </summary>
		public double RTO
		{
			get
			{
				return Thread.VolatileRead(ref _rto);
			}
			set
			{
				Thread.VolatileWrite(ref _rto, value);
			}
		}

		/// <summary>
		/// Send Time Out
		/// </summary>
		public long STO
		{
			get
			{
				return Thread.VolatileRead(ref _sto);
			}
			set
			{
				Thread.VolatileWrite(ref _rto, value);
			}
		}

		#endregion

		#region SetRTT

		internal void SetRTT(double rtt, double deltaRTT, double rto, long sto)
		{
			Thread.VolatileWrite(ref _rtt, rtt);
			Thread.VolatileWrite(ref _deltaRtt, deltaRTT);
			Thread.VolatileWrite(ref _rto, rto);
			Thread.VolatileWrite(ref _sto, sto);
		}

		#endregion

		#region SetFastRetransmit / OnEndFastRetransmit

		internal void StartFastRetransmit(int startPacketId, int endPacketId)
		{
			// Already in fast retransmit
			if (startPacketId > -1)
				return;

			// Pause sending
			PauseTransmission();

			// Allow fast retransmit
			_fastRetransmitEndPacketId = endPacketId;
			_fastRetransmitStartPacketId = startPacketId;
		}

		internal void OnEndFastRetransmit()
		{
			_controlWindow.OnEndFastRetransmit();

			// Reset
			_fastRetransmitStartPacketId = -1;
			_fastRetransmitEndPacketId = -1;

			// Resume sending
			ResumeTransmission();
		}

		#endregion

		#region PauseTransmission / ResumeTransmission

		private void PauseTransmission()
		{
			_controlWindow.PauseTransmission();
		}

		private void ResumeTransmission()
		{
			_controlWindow.ResumeTransmission();
		}

		#endregion

		#region OnEnd...

		internal void OnEndReceive(RUDPSocketError error, RUDPIngoingPacket packet, bool forceAsyncCall, RUDPReceiveIAsyncResult asyncResult)
		{
			asyncResult.Packet = packet;
			asyncResult.ForceAsyncCall = forceAsyncCall;
			asyncResult.SetAsCompleted(error, false);
		}

		internal void OnEndSend(RUDPSocketError error, RUDPSendIAsyncResult asyncResult)
		{
			if (asyncResult == null)
				return;

			asyncResult.SetAsCompleted(error, false);
		}

		internal void OnEndConnect(RUDPSocketError error)
		{
			RUDPConnectIAsyncResult result = null;
			Interlocked.Exchange<RUDPConnectIAsyncResult>(ref result, _asyncResultConnect);
			if (result == null)
				return;

			Interlocked.Exchange<RUDPConnectIAsyncResult>(ref _asyncResultConnect, null);

			result.Connected = (error == RUDPSocketError.Success);
			result.SetAsCompleted(error, false);
		}

		internal void OnEndAccept(RUDPSocket acceptedSocket)
		{
			RUDPAcceptIAsyncResult result = null;
			Interlocked.Exchange<RUDPAcceptIAsyncResult>(ref result, _asyncResultAccept);
			if (result == null)
				return;

			Interlocked.Exchange<RUDPAcceptIAsyncResult>(ref _asyncResultAccept, null);

			result.AcceptedSocket = acceptedSocket;
			result.ForceAsyncCall = true;
			result.SetAsCompleted(RUDPSocketError.Success, false);
		}

		internal void OnDisconnected(RUDPSocketError error)
		{
			OnEndConnect(error);

			RUDPReceiveIAsyncResult asyncResult = null;
			Interlocked.Exchange<RUDPReceiveIAsyncResult>(ref asyncResult, _asyncResultReceive);
			if (asyncResult != null)
			{
				Interlocked.Exchange<RUDPReceiveIAsyncResult>(ref _asyncResultReceive, null);
				OnEndReceive(error, null, false, asyncResult);
			}
		}

		#endregion

		#region HandleNextUserPacket

		internal void HandleNextUserPacket(bool forceAsyncCall)
		{
			if (_asyncResultReceive == null)
				return;

			RUDPReceiveIAsyncResult asyncResult;
			RUDPIngoingPacket packet = null;
			lock (_handleNextUserPacketLock)
			{
				if (_incomingPackets.Count < 1)
					return;

				asyncResult = _asyncResultReceive;
				if (_asyncResultReceive == null)
					return;

				//-- Clean the list
				while (_incomingPackets.Count > 0)
				{
					// Get the current packet
					packet = _incomingPackets.RemoveNextPacket();

					// No more packet
					if (packet == null)
						return;

					// Ordered packet
					if (packet.Channel != RUDPPacketChannel.UserPacket)
						_controlWindow.OnReceiveProcessed(packet);
					else 
						break; // It is the next message to process.
				}

				//---- Check it is not null
				if (packet == null)
					return;

				_asyncResultReceive = null;
			}

			//---- Receive
			if (asyncResult != null)
			{
				_controlWindow.OnReceiveProcessed(packet);
				OnEndReceive(RUDPSocketError.Success, packet, forceAsyncCall, asyncResult);
			}
		}

		#endregion

		#region For debug purpose only
		/*
		public PeerInformationStatus Status
		{
			get
			{
				return _status;
			}
			set
			{
				_status = value;
				BruNetStream.Trace("Update PeerInformationStatus:" + _status);
			}
		}
		*/
		#endregion

	}

	#region RecursionPoint

	public sealed class RecursionPoint
	{

		#region Variables

		private int _isExecuting = 0;

		static private readonly int TRUE = 1;
		static private readonly int FALSE = 0;

		#endregion

		#region Enter

		/// <summary>
		/// Returns true if we are already calling this method (We are in a recursion case)
		/// Returns false if we are not in recursion.
		/// </summary>
		public bool Enter()
		{
			// Avoid recursion for the same thread only (will not work with multiple threads).
			//return (Interlocked.Exchange(ref _isExecuting, Thread.CurrentThread.ManagedThreadId) != FALSE);
			return (Interlocked.Exchange(ref _isExecuting, TRUE) == TRUE);
		}

		#endregion

		#region Exit

		public void Exit()
		{
			Interlocked.Exchange(ref _isExecuting, FALSE);
		}

		#endregion

	}

	#endregion

}

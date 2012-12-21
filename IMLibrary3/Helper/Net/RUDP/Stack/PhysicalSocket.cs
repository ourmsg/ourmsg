using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Threading;

using Helper.ThreadingNET35;

namespace Helper.Net.RUDP
{
	/// <summary>
	/// Represent an instance of an UDP socket and all its associated RUDP sockets.
	/// </summary>
	sealed internal class PhysicalSocket
	{

		#region Variables

		//---- Connected RUDPSockets
		internal Dictionary<IPEndPoint, RUDPSocket> _connectedRDUPs = new Dictionary<IPEndPoint, RUDPSocket>(1);
		internal ReaderWriterLockSlim _connectedRDUPsLock = new ReaderWriterLockSlim();

		//---- Accepting RUDPSockets
		internal RUDPSocket _acceptingRDUP;
		internal ReaderWriterLockSlim _acceptingRDUPLock = new ReaderWriterLockSlim();

		//---- UDP management
		internal System.Net.Sockets.Socket _socket;
		internal byte[] _receiveBuffer = new byte[100 * 1024];
		internal IPEndPoint _canReceiveFromEndPoint = new IPEndPoint(IPAddress.Any, 0);

		#endregion

		#region Constructor

		internal PhysicalSocket()
		{
			_socket = new System.Net.Sockets.Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
			_socket.DontFragment = true;
			//_socket.Blocking = false;

			// To avoid : "An existing connection was forcibly closed by the remote host"
			uint IOC_IN = 0x80000000;
			uint IOC_VENDOR = 0x18000000;
			uint SIO_UDP_CONNRESET = IOC_IN | IOC_VENDOR | 12;
			_socket.IOControl((int)SIO_UDP_CONNRESET, new byte[] { Convert.ToByte(false) }, null);

			// Setup a default Send/Rcv buffer size
			_socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveBuffer, 100 * 1024);
			_socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendBuffer, 1000 * 1024);
		}

		internal void Dispose()
		{
			_receiveBuffer = null;
			_connectedRDUPs.Clear();
			_acceptingRDUP = null;
			_socket = null;
		}

		#endregion

		#region Bind

		internal void Bind(IPEndPoint endPoint)
		{
			_socket.Bind(endPoint);
		}

		#endregion

		#region Close

		internal void Close(RUDPSocket rudp)
		{
			RUDPStack.Close(rudp);
		}

		#endregion

		#region Shutdown

		internal void Shutdown(RUDPSocket rudp)
		{
			RUDPStack.Shutdown(rudp);
		}

		#endregion

		#region BeginConnect

		internal RUDPSocketError BeginConnect(RUDPSocket rudp, int timeOut)
		{
			//---- Add it to our list of "connected" sockets
			RegisterConnectedSocket(rudp);

			return RUDPStack.BeginConnect(rudp, timeOut);
		}

		#endregion

		#region BeginAccept

		internal void BeginAccept(RUDPSocket rudp, AsyncCallback callback, Object state)
		{
			rudp._status = RUDPSocketStatus.Accepting;

			//---- Only one socket accept on this port
			_acceptingRDUPLock.EnterWriteLock();
			_acceptingRDUP = rudp;
			_acceptingRDUPLock.ExitWriteLock();

			//---- Unregister for the stack
			RUDPStack.BeginAccept(rudp);
		}

		#endregion

		#region OnEndAccept

		internal RUDPSocket OnEndAccept(IPEndPoint remoteEndPoint, int packetId)
		{
			// Create the new socket
			RUDPSocket acceptSocket = new RUDPSocket();
			acceptSocket._physical = this;
			acceptSocket._status = RUDPSocketStatus.Connected;
			acceptSocket._remoteEndPoint = remoteEndPoint;
			acceptSocket._incomingPackets.CurrentPacketId = packetId;

			// Add it to our list of "connected" sockets
			RUDPStack.RegisterRUDPSocket(acceptSocket);
			RegisterConnectedSocket(acceptSocket);

			// Release it
			RUDPSocket previousAcceptingRDUP = _acceptingRDUP;
			_acceptingRDUPLock.EnterWriteLock();
			_acceptingRDUP = null;
			_acceptingRDUPLock.ExitWriteLock();

			// End the accept
			previousAcceptingRDUP.OnEndAccept(acceptSocket);

			return acceptSocket;
		}

		#endregion

		#region OnEndSend

		internal void OnEndSend(RUDPSocket rudp, RUDPSendIAsyncResult asyncResult)
		{
			rudp.OnEndSend(RUDPSocketError.Success, asyncResult);
		}

		#endregion

		#region OnDisconnected

		internal void OnDisconnected(RUDPSocket rudp, RUDPSocketError error)
		{
			UnregisterConnectedSocket(rudp);
			rudp.OnDisconnected(error);
		}

		#endregion

		#region RegisterConnectedSocket

		internal void RegisterConnectedSocket(RUDPSocket rudp)
		{
			_connectedRDUPsLock.EnterWriteLock();
			_connectedRDUPs.Add(rudp._remoteEndPoint, rudp);
			_connectedRDUPsLock.ExitWriteLock();
		}

		internal void UnregisterConnectedSocket(RUDPSocket rudp)
		{
			_connectedRDUPsLock.EnterWriteLock();
			_connectedRDUPs.Remove(rudp._remoteEndPoint);
			_connectedRDUPsLock.ExitWriteLock();
		}

		#endregion

	}
}

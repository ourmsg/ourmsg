//#define UDT_TRACE
//#define UDT_FULLTCP
//#define NO_UDT_CONNECTION

using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;
using System.Security;

namespace Helper.Net.UDT
{
	/// <summary>
	/// This class is a wrapper class around the UDT project ( http://udt.sf.net )
	/// </summary>
	public sealed class UDTSocket
	{

		#region API

		[DllImport("transport.dll", EntryPoint = "UDTSocket"), SuppressUnmanagedCodeSecurity()]
		private static extern int API_UDTSocket(int af, int type, int protocol);

		[DllImport("transport.dll", EntryPoint = "UDTConnect"), SuppressUnmanagedCodeSecurity()]
		private static extern int API_UDTConnect(int handle, byte[] name, int namelen);

		[DllImport("transport.dll", EntryPoint = "UDTBind"), SuppressUnmanagedCodeSecurity()]
		private static extern int API_Bind(int handle, byte[] name, int namelen);

		[DllImport("transport.dll", EntryPoint = "UDTListen"), SuppressUnmanagedCodeSecurity()]
		private static extern int API_Listen(int handle, int backlog);

		[DllImport("transport.dll", EntryPoint = "UDTAccept"), SuppressUnmanagedCodeSecurity()]
		private static extern int API_Accept(int handle, byte[] name, out int namelen);

		[DllImport("transport.dll", EntryPoint = "UDTClose"), SuppressUnmanagedCodeSecurity()]
		private static extern int API_Close(int handle);

		[DllImport("transport.dll", EntryPoint = "UDTShutdown"), SuppressUnmanagedCodeSecurity()]
		private static extern int API_Shutdown(int handle, int how);

		[DllImport("transport.dll", EntryPoint = "UDTSend"), SuppressUnmanagedCodeSecurity()]
		private static extern int API_Send(int handle, byte[] buffer, int len, int flags);

		[DllImport("transport.dll", EntryPoint = "UDTRecv"), SuppressUnmanagedCodeSecurity()]
		private static extern int API_Receive(int handle, byte[] buffer, int offset, int len, int flags);

		[DllImport("transport.dll", EntryPoint = "UDTSendmsg"), SuppressUnmanagedCodeSecurity()]
		private static extern int API_Sendmsg(int handle, byte[] buffer, int len, int ttl, bool inorder);

		[DllImport("transport.dll", EntryPoint = "UDTRecvmsg"), SuppressUnmanagedCodeSecurity()]
		private static extern int API_Recvmsg(int handle, byte[] buffer, int offset, int len);

		[DllImport("transport.dll", EntryPoint = "UDTSelect"), SuppressUnmanagedCodeSecurity()]
		private static extern int API_Select([In] int ignoredParameter, [In, Out] IntPtr[] readfds, [In, Out] IntPtr[] writefds, [In, Out] IntPtr[] exceptfds, [In] ref TimeValue timeout);

		[DllImport("transport.dll", EntryPoint = "UDTSetsockopt"), SuppressUnmanagedCodeSecurity()]
		private static extern int API_Setsockopt(int handle, int level, UDTSocketOptionName option, ref object optionValue, int optlen);

		[DllImport("transport.dll", EntryPoint = "UDTGetsockopt"), SuppressUnmanagedCodeSecurity()]
		private static extern int API_Getsockopt(int handle, int level, UDTSocketOptionName option, ref object optionValue, int optlen);

		[DllImport("transport.dll", EntryPoint = "UDTGetpeername"), SuppressUnmanagedCodeSecurity()]
		private static extern int API_Getpeername(int handle, byte[] name, out int namelen);

		[DllImport("transport.dll", EntryPoint = "UDTGetlasterrorCode"), SuppressUnmanagedCodeSecurity()]
		private static extern int API_GetLastErrorCode();

		[DllImport("transport.dll", EntryPoint = "UDTGetlasterrorMessage"), SuppressUnmanagedCodeSecurity()]
		private static extern string API_GetLastErrorMessage();

		[DllImport("transport.dll", EntryPoint = "UDTIsOnAccept"), SuppressUnmanagedCodeSecurity()]
		private static extern bool API_IsOnAccept(int handle);

		[DllImport("transport.dll", EntryPoint = "UDTIsOnRead"), SuppressUnmanagedCodeSecurity()]
		private static extern bool API_IsOnRead(int handle);

		[DllImport("transport.dll", EntryPoint = "UDTIsOnWrite"), SuppressUnmanagedCodeSecurity()]
		private static extern bool API_IsOnWrite(int handle);

		[DllImport("transport.dll", EntryPoint = "UDTIsOnError"), SuppressUnmanagedCodeSecurity()]
		private static extern bool API_IsOnError(int handle);

		[DllImport("transport.dll", EntryPoint = "UDTIsConnected"), SuppressUnmanagedCodeSecurity()]
		private static extern bool API_IsConnected(int handle);

		[DllImport("transport.dll", EntryPoint = "UDTSelect"), SuppressUnmanagedCodeSecurity()]
		private static extern int API_UDTSelect(int[] readfds, int readCount, int[] writefds, int writeCount, int[] exceptfds, int timeoutMicroseconds);

		[DllImport("transport.dll", EntryPoint = "UDTWaitForEvent"), SuppressUnmanagedCodeSecurity()]
		private static extern void API_WaitForEvent();

		//[DllImport("transport.dll", EntryPoint = "UDTGetoverlappedresult")]
		//private static extern bool API_Getoverlappedresult(int handle, int ioHandle, ref int progress, bool wait);

		private static int UDT_ERROR = -1;
		private static int UDT_INVALID_SOCK = -1;

		[StructLayout(LayoutKind.Sequential)]
		internal struct TimeValue
		{

			public int Seconds;
			public int Microseconds;
		}

		[StructLayout(LayoutKind.Sequential)]
		internal struct UDTLinger
		{

			internal short OnOff;
			internal short Time;
		}

		#endregion

		#region API - Asynchronous

		[DllImport("transport.dll", EntryPoint = "UDTSetAsynchronous"), SuppressUnmanagedCodeSecurity()]
		private static extern int API_SetAsynchronous(int handle, bool isAsynchronous);

		[DllImport("transport.dll", EntryPoint = "UDTSetEndSendCallback"), SuppressUnmanagedCodeSecurity()]
		private static extern int API_SetUDTEndSendCallback([MarshalAs(UnmanagedType.FunctionPtr)]EndOperationHandler callback);

		[DllImport("transport.dll", EntryPoint = "UDTBeginSend"), SuppressUnmanagedCodeSecurity()]
		private static extern int API_BeginSend(int handle, byte[] buffer, int len, int flags, IntPtr context);

		#endregion

		#region Variables

		private static Thread _udtThreadProcessing;

		internal static CommandProcessor.CommandProcessor _processor = new Helper.CommandProcessor.CommandProcessor("UDT");

		//---- Asynch send & receive
		[UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl)]
		private delegate void EndOperationHandler(IntPtr pointer);

		// This method is called by UDT once a send is done
		static private EndOperationHandler udtEndSendCallback;

		// This method is called by UDT once a receive is done
		//static private EndOperationHandler udtEndReceiveCallback;

		//---- Generic information
		private SocketType _socketType;

		//---- Tags
		public object Tag;

#if UDT_FULLTCP

        private System.Net.Sockets.Socket _tcpSocket;
#else

		//---- UDT
		// Socket Handler
		private int _handle = -1;

		// EndPoints
		private IPEndPoint _remoteEndPoint;
		private IPEndPoint _localEndPoint;
#endif

		#endregion

		#region Static

		/// <summary>
		/// Initialize the background processing for all the UDT threads.
		/// </summary>
		static UDTSocket()
		{
#if !NO_UDT_CONNECTION
			AppDomain.CurrentDomain.ProcessExit += new EventHandler(CurrentDomain_ProcessExit);

			// Set the callbacks
			udtEndSendCallback = new EndOperationHandler(UDTEndSendCallback);
			GC.KeepAlive(udtEndSendCallback);
			API_SetUDTEndSendCallback(udtEndSendCallback);

			// Start the processor
			_processor.Start();

			// Main select thread
			_udtThreadProcessing = new Thread(new ThreadStart(UDTThreadProcessing));
			_udtThreadProcessing.IsBackground = true;
			_udtThreadProcessing.Name = "UDTThread Select Processing";
			_udtThreadProcessing.Start();
#endif
		}

		/// <summary>
		/// Stop all the processing for all the UDT sockets.
		/// </summary>
		static public void ShutDownUDT()
		{
#if !NO_UDT_CONNECTION

			if (_processor != null)
				lock (_processor)
				{

					if (_udtThreadProcessing != null)
					{
						_udtThreadProcessing.Abort();
						_udtThreadProcessing = null;
					}

					if (_processor != null)
					{
						_processor.Stop();
						_processor = null;
					}
				}
#endif
		}

		static void CurrentDomain_ProcessExit(object sender, EventArgs e)
		{
			ShutDownUDT();
		}

		#endregion

		#region Constructors

#if !UDT_FULLTCP

		/// <summary>
		/// This method is used only by the UDT "accept" method in order
		/// to create a new socket.
		/// </summary>
		private UDTSocket(int handle, SocketType socketType, IPEndPoint remoteEndPoint, IPEndPoint localEndPoint)
		{
			_socketType = socketType;
			_handle = handle;
			_remoteEndPoint = remoteEndPoint;
			_localEndPoint = localEndPoint;

			IsAsynchronous = true;

			/*

			if ( socketType == SocketType.Stream )
			{
				//UDT::setsockopt(client, 0, UDT_MSS, new int(1052), sizeof(int));
				int MTU = 1052;
				API_Setsockopt(_handle, 0, UDTOption.UDT_MSS, ref MTU, Marshal.SizeOf(MTU));
			}
			*/
			//int Disconnected = 60000;
			//API_Setsockopt(_handle, 0, UDTOption.UDT_LINGER, ref Disconnected, Marshal.SizeOf(Disconnected));
		}
#else

        public UDTSocket(System.Net.Sockets.Socket tcpSocket)
        {
            _tcpSocket = tcpSocket;
        }
#endif

		public UDTSocket(AddressFamily addressFamily)
		{
#if UDT_FULLTCP
            _tcpSocket = new System.Net.Sockets.Socket(addressFamily, SocketType.Stream, ProtocolType.Tcp);
#else
			_socketType = SocketType.Stream;
			_handle = API_UDTSocket((int)addressFamily, (int)_socketType, (int)0);

			IsAsynchronous = true;

			/*

			if ( socketType == SocketType.Stream )
			{
				//UDT::setsockopt(client, 0, UDT_MSS, new int(1052), sizeof(int));
				int MTU = 1052;
				API_Setsockopt(_handle, 0, UDTOption.UDT_MSS, ref MTU, Marshal.SizeOf(MTU));
			}
			*/
#endif
		}

		#endregion

		#region Connect

		public void Connect(EndPoint remoteEP)
		{
#if !NO_UDT_CONNECTION
			//---- Tracing
			UDTTrace("UDTSocket[" + Handle + "]-Connect:" + remoteEP.ToString());

#if UDT_FULLTCP
            _tcpSocket.Connect(remoteEP);
#else
			//---- UDT
			SocketAddress address = ((IPEndPoint)remoteEP).Serialize();

			byte[] addressBuffer = new byte[address.Size];

			for (int index = 0; index < address.Size; index++)
				addressBuffer[index] = address[index];

			if (UDT_ERROR == API_UDTConnect(_handle, addressBuffer, address.Size))
				throw new UDTSocketException(); // Cannot connect
#endif
#else
			throw new UDTSocketException(); // Cannot connect
#endif
		}

		#endregion

		#region Select

		static public void Select(int[] readHandles, int[] writeHandles, int[] exceptionHandles, int timeOutMicroSeconds)
		{

			if (UDT_ERROR == API_UDTSelect(readHandles, readHandles.Length, writeHandles, writeHandles.Length, exceptionHandles, timeOutMicroSeconds))
				throw new UDTSocketException(); // Cannot connect
		}

		#endregion

		#region Bind

		public void Bind(EndPoint localEP)
		{
			_localEndPoint = (IPEndPoint)localEP;

			// Tracing
			UDTTrace("Bind[" + Handle + "]" + localEP.ToString());

#if UDT_FULLTCP
            _tcpSocket.Bind(localEP);
#else
			//---- UDT
			SocketAddress address = ((IPEndPoint)localEP).Serialize();

			byte[] addressBuffer = new byte[address.Size];

			for (int index = 0; index < address.Size; index++)
				addressBuffer[index] = address[index];

			if (UDT_ERROR == API_Bind(_handle, addressBuffer, address.Size))
				throw new UDTSocketException();
#endif
		}

		#endregion

		#region Listen

		public void Listen(int backlog)
		{
#if UDT_FULLTCP
            _tcpSocket.Listen(backlog);
#else

			//---- UDT
			if (UDT_ERROR == API_Listen(_handle, backlog))
				throw new UDTSocketException();
#endif
		}

		#endregion

		#region Accept

		public UDTSocket Accept()
		{
#if UDT_FULLTCP
            System.Net.Sockets.Socket newSocket = _tcpSocket.Accept();
            UDTSocket newUDTSocket = new UDTSocket(newSocket);

            // Tracing
            UDTTrace("UDTSocket[" + _tcpSocket.Handle + ", new:" + newSocket.Handle + "]-Accept:" + newSocket.RemoteEndPoint.ToString());

            return newUDTSocket;
#else
			//---- UDT
			EndPoint localEP = new IPEndPoint(IPAddress.Parse("0.0.0.0"), 0);
			SocketAddress address = ((IPEndPoint)localEP).Serialize();

			byte[] addressBuffer = new byte[address.Size];

			for (int index = 0; index < address.Size; index++)
				addressBuffer[index] = address[index];

			//---- Accept
			int size = 0;
			int newHandle = API_Accept(_handle, addressBuffer, out size);

			if (UDT_INVALID_SOCK == newHandle) // UDT::INVALID_SOCK
			{
				throw new UDTSocketException(API_GetLastErrorCode(),
											API_GetLastErrorMessage());
			}

			//---- Get the IP+port of the peer socket (RemoteEndPoint)
			for (int index = 0; index < address.Size; index++)
				address[index] = addressBuffer[index];

			IPEndPoint remoteEndPoint;

			if (address.Family == AddressFamily.InterNetwork)
				remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);

			else
				remoteEndPoint = new IPEndPoint(IPAddress.IPv6Any, 0);
			remoteEndPoint = (IPEndPoint)(remoteEndPoint.Create(address));

			//---- Get the IP+port of the socket (LocalEndPoint)
			size = address.Size;

			if (UDT_ERROR == API_Getpeername(newHandle, addressBuffer, out size))
			{
				throw new UDTSocketException(API_GetLastErrorCode(),
											API_GetLastErrorMessage());
			}
			IPEndPoint localEndPoint;

			if (address.Family == AddressFamily.InterNetwork)
				localEndPoint = new IPEndPoint(IPAddress.Any, 0);

			else
				localEndPoint = new IPEndPoint(IPAddress.IPv6Any, 0);
			localEndPoint = (IPEndPoint)(localEndPoint.Create(address));

			//---- Tracing
			UDTTrace("UDTSocket[" + _handle + ", new:" + newHandle + "]-Accept:" + remoteEndPoint.ToString());

			//---- Return the new socket
			return new UDTSocket(newHandle, _socketType, remoteEndPoint, localEndPoint);
#endif
		}

		#endregion

		#region Close

		public void Close()
		{
#if UDT_FULLTCP
            _tcpSocket.Close();
#else
			//---- UDT
			API_Close(_handle);
			_handle = -1;
#endif
		}

		#endregion

		#region Shutdown

		public void Shutdown(SocketShutdown how)
		{
#if UDT_FULLTCP
            _tcpSocket.Shutdown(how);
#else
			API_Shutdown(_handle, (int)how);
#endif
		}

		#endregion

		#region Send

		public int Send(byte[] buffer)
		{
			return Send(buffer, SocketFlags.None);
		}

		public int Send(byte[] buffer, SocketFlags socketFlags)
		{
			return Send(buffer, buffer.Length, socketFlags);
		}

		public int Send(byte[] buffer, int length, SocketFlags socketFlags)
		{
			UDTTrace("UDTSocket[" + Handle + "]-Send:size=" + length);

#if UDT_FULLTCP
            return _tcpSocket.Send(buffer, length, socketFlags);
#else

			//---- UDT
			if (_socketType == SocketType.Dgram)
				return API_Sendmsg(_handle, buffer, length,
					-1, // Infinite time out
					true); // Messages are sent and received in the same order

			else
			{
				return API_Send(_handle, buffer, length, (int)socketFlags);
			}
#endif
		}

		public int Send(byte[] buffer, int offset, int length, SocketFlags socketFlags)
		{
#if UDT_FULLTCP
            return _tcpSocket.Send(buffer, offset, length, socketFlags);
#else

			//---- UDT
			if (offset < 1)
				return Send(buffer, length, socketFlags);

			byte[] newBuffer = new byte[buffer.Length - offset];
			Array.Copy(buffer, offset, newBuffer, 0, buffer.Length - offset);
			return Send(newBuffer, length, socketFlags);
#endif
		}

		#endregion

		#region Receive

		public int Receive(byte[] buffer, int length, SocketFlags socketFlags)
		{
			return Receive(buffer, 0, length, socketFlags);
		}

		public int Receive(byte[] buffer, int offset, int length, SocketFlags socketFlags)
		{
#if UDT_FULLTCP
            int size = _tcpSocket.Receive(buffer, offset, length, socketFlags);
            UDTTrace("UDTSocket[" + Handle + "]-Receive:size=" + size);
            return size;
#else

			//---- UDT			
			try
			{

				if (_socketType == SocketType.Dgram)
					return API_Recvmsg(_handle, buffer, offset, length);

				else
				{
					int size = API_Receive(_handle, buffer, offset, length, (int)socketFlags);

					UDTTrace("UDTSocket[" + _handle + "]-Receive:size=" + size);

					if (size < 0) //  Like TCP
						return 0;

					return size;
				}
			}

			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				return 0;
			}
#endif
		}

		#endregion

		#region REMOVED: GetOverlappedResult
		/*
		public void GetOverlappedResult(int overlappedIOHandle, ref int progress, bool wait)
		{
			API_Getoverlappedresult(_handle, overlappedIOHandle, ref progress, wait);
		}
		*/
		#endregion

		#region UDT Thread - ASync Management

		// List of sockets waiting for accept
		static List<AsyncAcceptRegistration> _acceptRegistrations = new List<AsyncAcceptRegistration>(100);

		// List of sockets waiting for receive
		static List<AsyncReceiveRegistration> _receiveRegistrations = new List<AsyncReceiveRegistration>(100);

		static private void UDTThreadProcessing()
		{

			try
			{
				while (Thread.CurrentThread.IsAlive)
				{
					//---- Accept

					for (int index = _acceptRegistrations.Count - 1; index > -1; index--)
					{
						Monitor.Enter(_acceptRegistrations);
						AsyncAcceptRegistration registration = _acceptRegistrations[index];
						Monitor.Exit(_acceptRegistrations);

						if (registration.Socket.IsOnAccept)
						{
							// Stop "BeginAccept"
							lock (_acceptRegistrations)
								_acceptRegistrations.RemoveAt(index);

							// Queue it for the call back
							_processor.AddCommand(registration);
						}
					}

					//---- Receive

					for (int index = _receiveRegistrations.Count - 1; index > -1; index--)
					{
						Monitor.Enter(_receiveRegistrations);
						AsyncReceiveRegistration registration = _receiveRegistrations[index];
						Monitor.Exit(_receiveRegistrations);

						if (!registration.Socket.Connected || registration.Socket.IsOnRead)
						{
							// Stop "BeginReceive"
							lock (_receiveRegistrations)
								_receiveRegistrations.RemoveAt(index);

							// Queue it for the call back
							_processor.AddCommand(registration);
						}
					}

					//---- Like "Select" Wait for an event or 1 millisecond
					API_WaitForEvent();
				}
			}
			catch (ThreadInterruptedException)
			{ }
		}

		#endregion

		#region BeginAccept

		public IAsyncResult BeginAccept(AsyncCallback callback, object state)
		{
			AsyncAcceptRegistration asyncRegistration = new AsyncAcceptRegistration(this, callback, state);
			lock (_acceptRegistrations)
				_acceptRegistrations.Add(asyncRegistration);

			return null;
		}

		public UDTSocket EndAccept(IAsyncResult ar)
		{
			return (UDTSocket)((UDTAsyncResult)ar)._socket;
		}

		#endregion

		#region BeginConnect

		public IAsyncResult BeginConnect(IPEndPoint endPoint, AsyncCallback callback, object state)
		{
			AsyncConnectRegistration asyncRegistration = new AsyncConnectRegistration(this, endPoint, callback, state);

			_processor.AddCommand(asyncRegistration);

			return null;
		}

		public void EndConnect(IAsyncResult ar)
		{

			if (((UDTAsyncResult)ar).Exception != null)
			{
				throw ((UDTAsyncResult)ar).Exception;
			}
		}

		#endregion

		#region BeginReceive

		public IAsyncResult BeginReceive(byte[] buffer, int offset, int size, SocketFlags socketFlags, AsyncCallback callback, object state)
		{
			AsyncReceiveRegistration asyncRegistration = new AsyncReceiveRegistration(this, buffer, offset, size, socketFlags, callback, state);
			lock (_receiveRegistrations)
				_receiveRegistrations.Add(asyncRegistration);

			return null;
		}

		public int EndReceive(IAsyncResult ar)
		{
			return ((UDTAsyncResult)ar)._size;
		}

		#endregion

		#region BeginSend

		public IAsyncResult BeginSend(byte[] buffer, int offset, int size, SocketFlags socketFlags, out SocketError error, AsyncCallback callback, object state)
		{
			error = SocketError.Success;

			try
			{
				return BeginSend(buffer, offset, size, socketFlags, callback, state);
			}
			catch (Exception)
			{
				error = SocketError.SocketError;
				return null;
			}
		}

		public IAsyncResult BeginSend(byte[] buffer, int offset, int size, SocketFlags socketFlags, AsyncCallback callback, object state)
		{
			AsyncSendRegistration asyncRegistration = new AsyncSendRegistration(this, buffer, offset, size, socketFlags, callback, state);

			IntPtr pointer = GCHandle.ToIntPtr(GCHandle.Alloc(asyncRegistration));

			if (UDT_ERROR == API_BeginSend(_handle, buffer, size, (int)socketFlags, pointer))
				throw new UDTSocketException();

			// HACK !!!!!
			_processor.AddCommand(asyncRegistration);

			return null;
		}

		private static void UDTEndSendCallback(IntPtr pointer)
		{
			AsyncSendRegistration asyncRegistration = (AsyncSendRegistration)GCHandle.FromIntPtr(pointer).Target;
			_processor.AddCommand(asyncRegistration);
		}

		public int EndSend(IAsyncResult ar)
		{
			return ((UDTAsyncResult)ar)._size;
		}

		#endregion

		#region IsOn...

		public bool IsOnAccept
		{
			get
			{
#if UDT_FULLTCP
                List<System.Net.Sockets.Socket> list = new List<System.Net.Sockets.Socket>(1);
                list.Add(_tcpSocket);

                System.Net.Sockets.Socket.Select(list, null, null, 0);

                if (list.Count < 1)
                    return false;

                return true;
#else
				return API_IsOnAccept(_handle);
#endif
			}
		}

		public bool IsOnRead
		{
			get
			{
#if UDT_FULLTCP
                List<System.Net.Sockets.Socket> list = new List<System.Net.Sockets.Socket>(1);
                list.Add(_tcpSocket);

                System.Net.Sockets.Socket.Select(list, null, null, 0);

                if (list.Count < 1)
                    return false;

                return true;
#else
				return API_IsOnRead(_handle);
#endif
			}
		}

		public bool IsOnWrite
		{
			get
			{
#if UDT_FULLTCP
                return true;
#else
				return API_IsOnWrite(_handle);
#endif
			}
		}

		public bool IsOnError
		{
			get
			{
#if UDT_FULLTCP
                return false;
#else
				return API_IsOnError(_handle);
#endif
			}
		}

		#endregion

		#region SetSocketOption

		public void SetSocketOption(SocketOptionLevel optionLevel, UDTSocketOptionName optionName, int optionValue)
		{
			SetSocketOption(optionLevel, optionName, (object)optionValue);
		}

		public void SetSocketOption(SocketOptionLevel optionLevel, UDTSocketOptionName optionName, object optionValue)
		{
#if UDT_FULLTCP
            //_tcpSocket.SetSocketOption(optionLevel, optionName, optionValue);
#else
			//---- UDT
			int size = -1;

			if (optionValue is int)
				size = sizeof(int);

			else if (optionValue is long)
				size = sizeof(long);

			else if (optionValue is LingerOption)
			{
				// If false it is the default behaviors.
				// If true the socket "close" method will wait that all the sending data have been send and
				// that he has receive the ack. The close method does not return until all the data is delivered
				// or until "time" (second linger option) has expired).
				/*
				UDTLinger linger = new UDTLinger();
				linger.OnOff = ((LingerOption)optionValue).Enabled ? ((short)1) : ((short)0);
				linger.Time = (short)((LingerOption)optionValue).LingerTime;

				size = Marshal.SizeOf(typeof(UDTLinger));

				optionValue = linger;*/
				return;
			}

			else throw new NotImplementedException();

			try
			{
				API_Setsockopt(_handle, (int)optionLevel, optionName, ref optionValue, size);
			}

			catch (Exception e)
			{
				Console.Write(e.Message);
			}

			//API_Setsockopt(int handle, int level, UDTOption option, ref int optionValue /*const void* optval*/, int optlen);
#endif
		}

		#endregion

		#region Properties

		public bool Connected
		{
			get
			{
#if UDT_FULLTCP
            return _tcpSocket.Connected;
#else

				if (_handle < 0)
					return false;

				return API_IsConnected(_handle);
#endif
			}
		}

		public EndPoint LocalEndPoint
		{
			get
			{
#if UDT_FULLTCP
                return _tcpSocket.LocalEndPoint;
#else
				return _localEndPoint;
#endif
			}
		}

		public EndPoint RemoteEndPoint
		{
			get
			{
#if UDT_FULLTCP
                return _tcpSocket.RemoteEndPoint;
#else
				return _remoteEndPoint;
#endif
			}
		}

		public int Handle
		{
			get
			{
#if UDT_FULLTCP
            return _tcpSocket.Handle.ToInt32();
#else
				return _handle;
#endif
			}
		}

		private bool IsAsynchronous
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				//API_SetAsynchronous(_handle, value);
			}
		}

		#endregion

		#region Options

		public int SendTimeOut
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				SetSocketOption(SocketOptionLevel.IP, UDTSocketOptionName.UDT_SNDTIMEO, value);
				//API_Setsockopt(this._handle, 0, (int)UDTOption.UDT_SNDTIMEO, ref (object)value, 4);
			}
		}

		public int ReceiveTimeOut
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				SetSocketOption(SocketOptionLevel.IP, UDTSocketOptionName.UDT_RCVTIMEO, value);
				//API_Setsockopt(this._handle, 0, (int)UDTOption.UDT_RCVTIMEO, ref (object)value, 4);
			}
		}

		#endregion

		#region DEBUG...

		[System.Diagnostics.Conditional("UDT_TRACE")]
		static private void UDTTrace(string message)
		{
			Console.WriteLine(message);
		}

		#endregion

	}

	#region UDTSocketOptionName

	public enum UDTSocketOptionName
	{
		UDT_MSS = 0, // the Maximum Transfer Unit
		UDT_SNDSYN = 1, // if sending is blocking
		UDT_RCVSYN = 2, // if receiving is blocking
		UDT_CC = 3, // custom congestion control algorithm
		UDT_FC = 4, // deprecated, for compatibility only
		UDT_SNDBUF = 5, // maximum buffer in sending queue
		UDT_RCVBUF = 6, // UDT receiving buffer size
		UDT_LINGER = 7, // waiting for unsent data when closing
		UDP_SNDBUF = 8, // UDP sending buffer size
		UDP_RCVBUF = 9, // UDP receiving buffer size
		UDT_MAXMSG = 10, // maximum datagram message size
		UDT_MSGTTL = 11, // time-to-live of a datagram message
		UDT_RENDEZVOUS = 12, // rendezvous connection mode
		UDT_SNDTIMEO = 13, // send() timeout
		UDT_RCVTIMEO = 14 // recv() timeout
	}

	#endregion

	#region UDTAsyncResult

	public sealed class UDTAsyncResult : IAsyncResult
	{

		internal object _stateObject;
		internal int _size;
		internal UDTSocket _socket;
		internal int _overlappedIoHanlde;
		internal UDTSocketException Exception;

		public UDTAsyncResult(int size, object stateObject, UDTSocket socket, int overlappedIoHanlde)
		{
			_size = size;
			_stateObject = stateObject;
			_socket = socket;
			_overlappedIoHanlde = overlappedIoHanlde;
		}

		public UDTAsyncResult(int size, object stateObject)
		{
			_size = size;
			_stateObject = stateObject;
		}

		// Summary:
		//     Gets a System.Threading.WaitHandle that is used to wait for an asynchronous
		//     operation to complete.
		//
		// Returns:
		//     A System.Threading.WaitHandle that is used to wait for an asynchronous operation
		//     to complete.
		public WaitHandle AsyncWaitHandle
		{
			get { return null; }
		}

		// Summary:
		//     Gets an indication of whether the asynchronous operation completed synchronously.
		//
		// Returns:
		//     true if the asynchronous operation completed synchronously; otherwise, false.
		public bool CompletedSynchronously
		{
			get { return true; }
		}

		// Summary:
		//     Gets an indication whether the asynchronous operation has completed.
		//
		// Returns:
		//     true if the operation is complete; otherwise, false.
		public bool IsCompleted
		{
			get { return true; }
		}

		// Summary:
		//     Gets a user-defined object that qualifies or contains information about an
		//     asynchronous operation.
		//
		// Returns:
		//     A user-defined object that qualifies or contains information about an asynchronous
		//     operation.
		public object AsyncState
		{
			get { return _stateObject; }
		}
	}

	#endregion

	#region UDTSocketException

	public sealed class UDTSocketException : Exception
	{

		private int _errorCode;

		public UDTSocketException(int errorCode, string errorMessage)
			: base(errorMessage)
		{
			_errorCode = errorCode;
		}

		public UDTSocketException()
			: base(API_GetLastErrorMessage())
		{
			_errorCode = API_GetLastErrorCode();
		}

		public int ErrorCode
		{
			get
			{
				return _errorCode;
			}
			set { _errorCode = value; }
		}

		[DllImport("transport.dll", EntryPoint = "UDTGetlasterrorCode")]
		private static extern int API_GetLastErrorCode();

		[DllImport("transport.dll", EntryPoint = "UDTGetlasterrorMessage")]
		private static extern string API_GetLastErrorMessage();
	}

	#endregion

}
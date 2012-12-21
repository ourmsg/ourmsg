using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;
using System.Net;

using Helper.CommandProcessor;

namespace Helper.Net.UDT
{

	#region AsyncConnectRegistration

	internal sealed class AsyncConnectRegistration : ICommand
	{

		internal AsyncCallback ParamCallBack;
		internal object ParamState;
		internal UDTSocket Socket;
		internal IPEndPoint EndPoint;

		internal AsyncConnectRegistration(UDTSocket socket, IPEndPoint endPoint, AsyncCallback paramCallBack, object paramState)
		{
			Socket = socket;
			EndPoint = endPoint;
			ParamCallBack = paramCallBack;
			ParamState = paramState;
		}

		public void Execute()
		{
			UDTAsyncResult result = new UDTAsyncResult(0, ParamState, null, 0);

			try
			{
				Socket.Connect(EndPoint);
			}

			catch (UDTSocketException e)
			{
				result.Exception = e;
			}

			ParamCallBack(result);
		}
	}

	#endregion

	#region AsyncAcceptRegistration

	internal sealed class AsyncAcceptRegistration : ICommand
	{

		internal AsyncCallback ParamCallBack;
		internal object ParamState;
		internal UDTSocket Socket;

		internal AsyncAcceptRegistration(UDTSocket socket, AsyncCallback paramCallBack, object paramState)
		{
			Socket = socket;
			ParamCallBack = paramCallBack;
			ParamState = paramState;
		}

		public void Execute()
		{

			try
			{
				UDTSocket acceptedSocket = Socket.Accept();

				ParamCallBack(new UDTAsyncResult(0, ParamState, acceptedSocket, 0));
			}

			catch (UDTSocketException e)
			{

				// A blocking operation was interrupted
				// by a call to WSACancelBlockingCall.
				// (By example when "close" is called on the socket)
				if (e.ErrorCode == 10004)
					return;
			}
		}
	}

	#endregion

	#region AsyncReceiveRegistration

	internal sealed class AsyncReceiveRegistration : ICommand
	{

		internal UDTSocket Socket;
		internal AsyncCallback ParamCallBack;
		internal object ParamState;
		internal byte[] ParamBuffer;
		internal int ParamOffset;
		internal int ParamSize;
		internal SocketFlags ParamSocketFlags;
		internal int OverlappedIoHanlde;

		internal AsyncReceiveRegistration(UDTSocket socket, byte[] paramBuffer, int paramOffset, int paramSize, SocketFlags paramSocketFlags, AsyncCallback paramCallBack, object paramState)
		{
			Socket = socket;
			ParamCallBack = paramCallBack;
			ParamState = paramState;
			ParamBuffer = paramBuffer;
			ParamOffset = paramOffset;
			ParamSize = paramSize;
			ParamSocketFlags = paramSocketFlags;
		}

		public void Execute()
		{
			if (!Socket.Connected)
			{
				// Socket disconnected
				ParamCallBack(new UDTAsyncResult(0, ParamState));
				return;
			}

			int receivedSize = Socket.Receive(ParamBuffer, ParamOffset, ParamSize, ParamSocketFlags);

			ParamCallBack(new UDTAsyncResult(receivedSize, ParamState, Socket, OverlappedIoHanlde));
		}
	}

	#endregion

	#region AsyncSendRegistration

	internal sealed class AsyncSendRegistration : ICommand
	{

		internal UDTSocket Socket;
		internal AsyncCallback ParamCallBack;
		internal object ParamState;
		internal byte[] ParamBuffer;
		internal int ParamOffset;
		internal int ParamSize;
		internal SocketFlags ParamSocketFlags;

		internal AsyncSendRegistration(UDTSocket socket, byte[] paramBuffer, int paramOffset, int paramSize, SocketFlags paramSocketFlags, AsyncCallback paramCallBack, object paramState)
		{
			Socket = socket;
			ParamCallBack = paramCallBack;
			ParamState = paramState;
			ParamBuffer = paramBuffer;
			ParamOffset = paramOffset;
			ParamSize = paramSize;
			ParamSocketFlags = paramSocketFlags;
		}

		public void Execute()
		{

			if (!Socket.Connected)
			{
				// Socket disconnected
				ParamCallBack(new UDTAsyncResult(0, ParamState));
				return;
			}

			ParamCallBack(new UDTAsyncResult(ParamSize, ParamState, Socket, 0));
		}
	}

	#endregion

	#region IocpException class

	/// <summary>An exception raised by one of the methods in the IocpThreadPool class.</summary>
	public sealed class IocpException : ApplicationException
	{

		#region Parameterized constructor (message)

		/// <summary>IocpException Constructor.</summary>
		/// <param name="message">Exception message.</param>
		public IocpException(string message)
			: base(message)
		{
		}

		#endregion

	}

	#endregion

}
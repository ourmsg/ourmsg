using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Threading;

namespace Helper.Net.RUDP
{

	#region RUDPAsyncResult

	public class RUDPAsyncResult : IAsyncResult
	{

		#region Variables

		// Fields set at construction which never change while 
		// operation is pending
		private readonly AsyncCallback _asyncCallback;
		private readonly Object _asyncState;

		// Fields set at construction which do change after 
		// operation completes
		private const Int32 c_StatePending = 0;
		private const Int32 c_StateCompletedSynchronously = 1;
		private const Int32 c_StateCompletedAsynchronously = 2;
		private Int32 m_CompletedState = c_StatePending;

		//
		internal RUDPSocket _rudp;
		public RUDPSocketError SocketError;

		// Field that may or may not get set depending on usage
		private ManualResetEvent _asyncWaitHandle;

		public bool ForceAsyncCall = false;

		#endregion

		#region Constructor

		internal RUDPAsyncResult(RUDPSocket rudp, AsyncCallback callback, Object state)
		{
			_rudp = rudp;
			_asyncCallback = callback;
			_asyncState = state;
		}

		#endregion

		#region SetAsCompleted

		internal void SetAsCompleted(RUDPSocketError socketError, Boolean completedSynchronously)
		{
			// Passing null for exception means no error occurred. 
			// This is the common case
			SocketError = socketError;

			// The m_CompletedState field MUST be set prior calling the callback
			Int32 prevState = Interlocked.Exchange(ref m_CompletedState,
			   completedSynchronously ? c_StateCompletedSynchronously : c_StateCompletedAsynchronously);

			if (prevState != c_StatePending)
				throw new InvalidOperationException("You can set a result only once");

			// If the event exists, set it
			//Thread.MemoryBarrier(); // Joe Duffy
			if (_asyncWaitHandle != null)
				_asyncWaitHandle.Set();

			// If a callback method was set, call it
			if (_asyncCallback != null)
			{
				if (!ForceAsyncCall)
				{
					try
					{
						_asyncCallback(this);
					}
					catch (Exception exception)
					{
						RUDPStack.HandleException(exception);
					}
				}
				else
					ThreadPool.UnsafeQueueUserWorkItem(new WaitCallback(CompleteAsyncCall), null);
			}
		}

		#endregion

		#region CompleteAsyncCall

		private void CompleteAsyncCall(object state)
		{
			try
			{
				_asyncCallback(this);
			}
			catch (Exception exception)
			{
				RUDPStack.HandleException(exception);
			}
		}

		#endregion

		#region EndInvoke

		internal void EndInvoke(bool canThrowException)
		{
			// This method assumes that only 1 thread calls EndInvoke 
			// for this object
			if (!IsCompleted)
			{
				// If the operation isn't done, wait for it
				AsyncWaitHandle.WaitOne();
				AsyncWaitHandle.Close();
				_asyncWaitHandle = null;  // Allow early GC
			}

			// Operation is done: if an error occured, throw it
			if (canThrowException)
				if (SocketError != RUDPSocketError.Success)
					throw new RUDPSocketException(SocketError);
		}

		#endregion

		#region IAsyncResult Members

		public object AsyncState
		{
			get
			{
				return _asyncState;
			}
		}

		public WaitHandle AsyncWaitHandle
		{
			get
			{
				if (_asyncWaitHandle == null)
				{
					Boolean done = IsCompleted;
					ManualResetEvent mre = new ManualResetEvent(done);
					if (Interlocked.CompareExchange(ref _asyncWaitHandle, mre, null) != null)
					{
						// Another thread created this object's event; dispose 
						// the event we just created
						mre.Close();
					}
					else
					{
						if (!done && IsCompleted)
						{
							// If the operation wasn't done when we created 
							// the event but now it is done, set the event
							_asyncWaitHandle.Set();
						}
					}
				}
				return _asyncWaitHandle;
			}
		}

		public Boolean CompletedSynchronously
		{
			get
			{
				return Thread.VolatileRead(ref m_CompletedState) == c_StateCompletedSynchronously;
			}
		}

		public Boolean IsCompleted
		{
			get
			{
				return Thread.VolatileRead(ref m_CompletedState) != c_StatePending;
			}
		}

		#endregion

	}

	#endregion

	#region IAsyncResult: Connect

	sealed public class RUDPConnectIAsyncResult : RUDPAsyncResult
	{
		internal bool Connected = false;

		internal RUDPConnectIAsyncResult(RUDPSocket rudp, AsyncCallback callback, Object state)
			: base(rudp, callback, state)
		{
		}
	}

	#endregion

	#region IAsyncResult: Accept

	sealed public class RUDPAcceptIAsyncResult : RUDPAsyncResult
	{
		internal RUDPSocket AcceptedSocket;

		internal RUDPAcceptIAsyncResult(RUDPSocket rudp, AsyncCallback callback, Object state)
			: base(rudp, callback, state)
		{
		}
	}

	#endregion

	#region IAsyncResult: Send

	sealed public class RUDPSendIAsyncResult : RUDPAsyncResult
	{
		internal int _size;

		internal RUDPSendIAsyncResult(RUDPSocket rudp, AsyncCallback callback, Object state, int size)
			: base(rudp, callback, state)
		{
			_size = size;
		}
	}

	#endregion

	#region IAsyncResult: Receive

	sealed public class RUDPReceiveIAsyncResult : RUDPAsyncResult
	{
		internal RUDPIngoingPacket Packet;

		internal RUDPReceiveIAsyncResult(RUDPSocket rudp, AsyncCallback callback, Object state)
			: base(rudp, callback, state)
		{
		}
	}

	#endregion

}

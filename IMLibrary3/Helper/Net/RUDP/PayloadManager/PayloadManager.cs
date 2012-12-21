using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Threading;

using Helper.ThreadingNET35;

namespace Helper.Net.RUDP
{
	/// <summary>
	/// Manage the allocation of the payloads. It is done like this because
	/// we always use the same payload size.
	/// 
	/// So it is better to reuse memory blocks.
	/// </summary>
	static internal class PayloadManager
	{

		#region Variables

		static Queue<byte[]>[] _queues = new Queue<byte[]>[156 + 1];
		static ReaderWriterLockSlim[] _locks = new ReaderWriterLockSlim[156 + 1];

		static readonly int MaxAllocation = 1024 * 1000 * 4; // 4 Mb

		// Total bytes allocated and managed by the manager
		static int _totalAllocation = 0;

		// Current bytes in the queues
		static int _currentAllocation = 0;

		#endregion

		#region Constructor

		static PayloadManager()
		{
			foreach (int index in Enum.GetValues(typeof(RUDPPacketChannel)))
			{
				_queues[index] = new Queue<byte[]>();
				_locks[index] = new ReaderWriterLockSlim();
			}
		}

		#endregion

		#region Allocate

		static public byte[] Allocate(RUDPPacketChannel channel, int size)
		{
			//return new byte[size];

			Queue<byte[]> queue = _queues[(int)channel];
			ReaderWriterLockSlim rwlock = _locks[(int)channel];

			byte[] buffer;

			//---- Get from the cache
			rwlock.EnterWriteLock();
			if (queue.Count > 0)
			{
				buffer = queue.Dequeue();
				_currentAllocation -= buffer.Length;

				// Only usefull when MTU change
				if (buffer.Length != size)
					Array.Resize<byte>(ref buffer, size);

				rwlock.ExitWriteLock();
				return buffer;
			}

			rwlock.ExitWriteLock();

			//---- Create new one
			if (_totalAllocation < MaxAllocation)
			{
				_totalAllocation += size;
				buffer = new byte[size];

				// ask the garbage collector to avoid from relocating a certain object in memory
				System.Runtime.InteropServices.GCHandle.Alloc(buffer, System.Runtime.InteropServices.GCHandleType.Pinned);
				GC.KeepAlive(buffer);

				return buffer;
			}
			else
				// Use the GC when we exceed the limit
				return new byte[size];

		}

		#endregion

		#region Deallocate

		static public void Deallocate(RUDPPacketChannel channel, byte[] buffer)
		{
			if (_currentAllocation < MaxAllocation)
			{
				Queue<byte[]> queue = _queues[(int)channel];
				ReaderWriterLockSlim rwlock = _locks[(int)channel];

				rwlock.EnterWriteLock();
				queue.Enqueue(buffer);
				rwlock.ExitWriteLock();

				_currentAllocation += buffer.Length;
			}
		}

		#endregion

	}

}

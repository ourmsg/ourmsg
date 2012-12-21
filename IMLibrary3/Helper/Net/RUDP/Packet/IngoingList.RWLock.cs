using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

using Helper.ThreadingNET35;

namespace Helper.Net.RUDP
{
	internal sealed class IngoingListRWLock
	{

		#region Variables

		private ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();
		private LinkedList<RUDPIngoingPacket> _list = new LinkedList<RUDPIngoingPacket>();
		private int _currentPacketId = -1;

		#endregion

		#region AddPacket

		internal void AddPacket(RUDPIngoingPacket packet)
		{
			if (packet.PacketId < 0)
			{
				_lock.EnterWriteLock();
				_list.AddLast(packet);
				_lock.ExitWriteLock();
				return;
			}

			_lock.EnterUpgradeableReadLock();

			try
			{
				LinkedListNode<RUDPIngoingPacket> node = _list.Last;
				while (node != null && (node.Value.PacketId < 0 || packet.PacketId < node.Value.PacketId))
					node = node.Previous;

				if (node == null)
				{
					_lock.EnterWriteLock();
					_list.AddFirst(packet);
					_lock.ExitWriteLock(); // Needed ?
					return;
				}

				// If duplicated packet -> drop
				if (packet.PacketId == node.Value.PacketId)
					return;

				// Add
				_lock.EnterWriteLock();
				_list.AddAfter(node, packet);
				_lock.ExitWriteLock(); // Needed ?
			}
			finally
			{
				_lock.ExitUpgradeableReadLock();
			}
		}

		#endregion

		#region RemoveNextPacket

		/// <summary>
		/// Returns the next packet to process and remove it.
		/// </summary>
		/// <returns></returns>
		internal RUDPIngoingPacket RemoveNextPacket()
		{
			_lock.EnterUpgradeableReadLock();

			try
			{
				LinkedListNode<RUDPIngoingPacket> node = _list.First;
				if (node == null)
					return null;

				if (node.Value.PacketId < 0 || node.Value.PacketId == _currentPacketId + 1)
				{
					_lock.EnterWriteLock();
					_currentPacketId++;
					_list.Remove(node);
					_lock.ExitWriteLock();
					return node.Value;
				}
			}
			finally
			{
				_lock.ExitUpgradeableReadLock();
			}

			return null;
		}

		#endregion

		#region Properties

		internal int Count
		{
			get
			{
				return _list.Count;
			}
		}

		internal int CurrentPacketId
		{
			get
			{
				return _currentPacketId;
			}
			set
			{
				_currentPacketId = value;
			}
		}

		internal int LastPacketId
		{
			get
			{
				_lock.EnterReadLock();

				try
				{
					if (_list.Count < 1)
						return -1;
					return _list.Last.Value.PacketId;
				}
				finally
				{
					_lock.ExitReadLock();
				}
			}
		}

		#endregion

		#region Clear

		internal void Clear()
		{
			_lock.EnterWriteLock();

			_list.Clear();
			_currentPacketId = -1;

			_lock.ExitWriteLock();
		}

		#endregion

		#region ContainsPacket

		internal bool ContainsPacket(int packetId)
		{
			_lock.EnterReadLock();

			LinkedListNode<RUDPIngoingPacket> node = _list.First;
			while (node != null)
				if (node.Value.PacketId != packetId)
					node = node.Next;
				else
				{
					_lock.ExitReadLock();
					return true;
				}
				
			_lock.ExitReadLock();

			return false;
		}

		#endregion

	}
}

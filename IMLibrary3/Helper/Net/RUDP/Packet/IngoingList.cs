using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

using Helper.ThreadingNET35;

namespace Helper.Net.RUDP
{
	internal sealed class IngoingList
	{

		#region Variables

		private object _lock = new object();
		private LinkedList<RUDPIngoingPacket> _list = new LinkedList<RUDPIngoingPacket>();
		private int _currentPacketId = -1;

		#endregion

		#region AddPacket

		internal void AddPacket(RUDPIngoingPacket packet)
		{
			lock (_lock)
			{
				if (packet.PacketId < 0)
				{
					_list.AddLast(packet);
					return;
				}

				LinkedListNode<RUDPIngoingPacket> node = _list.Last;
				while (node != null && (node.Value.PacketId < 0 || packet.PacketId < node.Value.PacketId))
					node = node.Previous;

				if (node == null)
				{
					_list.AddFirst(packet);
					return;
				}

				// If duplicated packet -> drop
				if (packet.PacketId == node.Value.PacketId)
					return;

				// Add
				_list.AddAfter(node, packet);
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
			lock (_lock)
			{
				LinkedListNode<RUDPIngoingPacket> node = _list.First;
				if (node == null)
					return null;

				if (node.Value.PacketId < 0 || node.Value.PacketId == _currentPacketId + 1)
				{
					_currentPacketId++;
					_list.Remove(node);
					return node.Value;
				}
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
				lock (_lock)
				{
					if (_list.Count < 1)
						return -1;
					return _list.Last.Value.PacketId;
				}
			}
		}

		#endregion

		#region Clear

		internal void Clear()
		{
			lock (_lock)
			{
				_list.Clear();
				_currentPacketId = -1;
			}
		}

		#endregion

		#region ContainsPacket

		internal bool ContainsPacket(int packetId)
		{
			lock (_lock)
			{

				LinkedListNode<RUDPIngoingPacket> node = _list.First;
				while (node != null)
					if (node.Value.PacketId != packetId)
						node = node.Next;
					else
						return true;

			}

			return false;
		}

		#endregion

	}
}

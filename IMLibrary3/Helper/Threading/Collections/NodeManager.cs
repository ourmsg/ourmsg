using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Helper.Threading.Collections
{

	public class NodeManager<T>
	{

		public static NodeManager<T> Default = new NodeManager<T>();
		public LockFreeNode<T> Allocate() { return Allocate(default(T)); }
		public virtual LockFreeNode<T> Allocate(T item)
		{
			return new LockFreeNode<T>(item);
		}

		public virtual void Free(LockFreeNode<T> node)
		{
			node.Item = default(T); // Allow early GC
		}
	}

	public class NodePool<T> : NodeManager<T>
	{

		// The head of the stack (always refers to a node; never null)
		private LockFreeNode<T> m_head = new LockFreeNode<T>();

		public NodePool()
		{
		}

		public override LockFreeNode<T> Allocate(T item)
		{
			LockFreeNode<T> node;

			do
			{
				// Get head
				node = m_head.Next;

				// If no head, stack is empty, return new node
				if (node == null) return new LockFreeNode<T>(item);

				// If previous head == what we think is head, change head to next node
				// else try again
			} while (!InterlockedEx.IfThen(ref m_head.Next, node, node.Next));
			node.Item = item;
			return node;
		}

		public override void Free(LockFreeNode<T> node)
		{
			node.Item = default(T); // Allow early GC

			// Try to make the new node be the head of the stack
			do
			{
				// Make the new node refer to the old head
				node.Next = m_head.Next;

				// If previous head's next == what we thought was next, change head's Next to the new node
				// else, try again if another thread changed the head
			} while (!InterlockedEx.IfThen(ref m_head.Next, node.Next, node));
		}
	}

	public sealed class LockFreeNode<T>
	{

		public LockFreeNode<T> Next; // Refers to next node or null
		public T Item;       // Item in this Node
		public LockFreeNode() { }
		public LockFreeNode(T item) { Item = item; }
	}
}
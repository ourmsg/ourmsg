
// Copyright (c) 2006 by Jeffrey Richter and Wintellect
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Helper.Threading.Collections
{
	sealed public class LockFreeQueue<T>
	{

		#region Variables

		private NodeManager<T> m_nodeManager;

		// The head & tail of the queue (always refer to nodes; never null)
		private LockFreeNode<T> m_head;
		private LockFreeNode<T> m_tail;

		//private int m_count = 0;

		#endregion

		#region Constructor

		public LockFreeQueue(NodeManager<T> nodeManager)
		{
			m_nodeManager = (nodeManager == null) ? NodeManager<T>.Default : nodeManager;
			// Get a single node and make the head and tail refer to it
			// The node's Next field is null marking the end of the queue
			m_head = m_nodeManager.Allocate();
			m_tail = m_head;
		}

		public LockFreeQueue()
			: this(null)
		{
		}

		#endregion

		#region Enqueue

		// If two threads call Enqueue simultaneously:
		// Both threads create and initialize a new node
		// The tail's Next will refer to one of these 2 new nodes
		// The new new tail's Next will refer to the other of the 2 new nodes
		// m_tail will refer to the 1st new node appended (not the real tail)
		// To fix this: Enqueue initializes by advancing m_tail to the node whose Next is null
		public void Enqueue(T item)
		{
			// Get (or allocate) a node and initialize it
			LockFreeNode<T> newNode = m_nodeManager.Allocate(item);

			LockFreeNode<T> tempTail = null;

			for (Boolean appendedNewNode = false; !appendedNewNode; )
			{
				// Get the current tail and what IT refers to
				tempTail = m_tail;
				LockFreeNode<T> tempTailNext = tempTail.Next;

				// If another thread changed the tail, start over
				Thread.MemoryBarrier(); // Make sure the value read from m_tail is fresh

				if (tempTail != m_tail) continue;

				// If the tail isn't truely the tail, fix the tail, start over
				if (tempTailNext != null)
				{
					// This can happen if multiple threads append nodes at the same time
					// A new node thinks it's the tail (Next is null) as another thread's new node
					// updates the previous node's Nextthinks it's the tail's Next field may not 
					InterlockedEx.IfThen(ref m_tail, tempTail, tempTailNext);
					continue;
				}

				// The tail is truely the tail, try to append the new node
				appendedNewNode = InterlockedEx.IfThen(ref tempTail.Next, null, newNode);
			}

			// When new node is sucessfully appended, make the tail refer to it
			// This can fail if another thread scoots in. If this happens, our node is 
			// appended to the linked-list but m_tail refers to another node that is not 
			// the tail. The next Enqueue/Dequeue call will fix m_tail
			InterlockedEx.IfThen(ref m_tail, tempTail, newNode);

			//Interlocked.Increment(ref m_count);
		}

		#endregion

		#region TryDequeue

		public Boolean TryDequeue(out T item)
		{
			item = default(T);

			// Loop until we manage to advance the head, removing 
			// a node (if there are no nodes to dequeue, we'll exit the method instead)
			for (Boolean dequeuedNode = false; !dequeuedNode; )
			{
				// make local copies of the head, the tail, and the head's Next reference
				LockFreeNode<T> tempHead = m_head;
				LockFreeNode<T> tempTail = m_tail;
				LockFreeNode<T> tempHeadNext = tempHead.Next;

				// If another thread changed the head, start over
				Thread.MemoryBarrier(); // Make sure the value read from m_head is fresh

				if (tempHead != m_head) continue;

				// If the head equals the tail
				if (tempHead == tempTail)
				{

					// If the head node refers to null, then the queue is empty
					if (tempHeadNext == null) return false;

					// The head refers to the tail whose Next is not null. This means
					// we have a lagging tail; update it
					InterlockedEx.IfThen(ref m_tail, tempTail, tempHeadNext);
					continue;
				}

				// The head and tail nodes are different; dequeue the head node and advance the head
				item = tempHeadNext.Item;
				dequeuedNode = InterlockedEx.IfThen(ref m_head, tempHead, tempHeadNext);

				if (dequeuedNode) m_nodeManager.Free(tempHead);
			}
			//Interlocked.Decrement(ref m_count);
			return true;
		}

		#endregion

		#region Dequeue

		public T Dequeue()
		{
			T item;

			// If item can be dequeued, return it; else throw
			if (TryDequeue(out item)) return item;
			throw new InvalidOperationException("Queue is empty");
		}

		#endregion

		#region Clear

		public void Clear()
		{
			// If another thread changed the head, start over
			Thread.MemoryBarrier(); // Make sure the value read from m_head is fresh
			Interlocked.Exchange(ref m_head, m_nodeManager.Allocate());
			Interlocked.Exchange(ref m_tail, m_head);
			//m_count = 0;
		}

		#endregion

	}

}
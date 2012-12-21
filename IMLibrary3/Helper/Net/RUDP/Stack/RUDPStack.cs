//#define CONSOLE_TRACE
//#define CONSOLE_TRACE_MEMORY
//#define STATISTICS
//#define TEST_PACKETLOOSE

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;

using Helper.Threading.Collections;
using Helper.ThreadingNET35;

namespace Helper.Net.RUDP
{

	#region Documentations

	/// <summary>
	/// Features :
	/// -------------
	/// 1 - Unreliable packets
	/// 2 - Reliable messaging (You are insured that the message arrives)
	/// 3 - Ordering messages delivery (Allow sending/receiving the messages in the same order)
	/// 4 - KeepAlive feature (to insure that there is no broken connection)
	/// 5 - Has a Tear down message (to close the connection gracefully)
	/// 6 - RendezVous mode and NAT traversal
	/// 7 - Packet fragmentation
	/// 8 - Sliding window (using slot technic)
	/// 9 - Piggy backing (ACK stored in packets)
	/// 10 - Multiples ACKs in a packet (Like SACK for TCP).
	/// 
	/// In progress :
	/// -------------
	/// 1 - Congestion Avoidance Problem :  http://www.cs.utk.edu/~dunigan/tcptour/javis/tcp_congavd.html
	///										http://www.ssfnet.org/Exchange/tcp/tcpTutorialNotes.html
	///										http://www.cs.cmu.edu/afs/cs/academic/class/15441-s06/lectures/L20-TCP-Congestion.pdf
	/// To do:
	/// ------
	/// 2 - Better errors management
	/// 3 - Bandwidth x Delay Product : http://www.cs.utk.edu/~dunigan/tcptour/javis/tcp_slidwin.html
	/// adapt the size of the congestion window. (And the Time out too)
	/// 4 - Slow start : http://www.cs.utk.edu/~dunigan/tcptour/javis/tcp_slowstart.html
	/// 5 - Fast Retransmit Algorithm : http://www.cs.utk.edu/~dunigan/tcptour/javis/tcp_fastrtx.html
	/// 6 - Fast-Recovery Algorithm : http://www.cs.utk.edu/~dunigan/tcptour/javis/tcp_fastrec.html
	/// 
	/// Improvement :
	/// -------------
	/// Optimization : http://gyroweb.inria.fr/~viennot/enseignement/projets/ltcodestcp/ltcodestcphevea.html
	/// Study UDT protocol to improve the used algorithms.
	/// Avoid SIN Attack : http://sewww.epfl.ch/SIC/SA/SPIP/Publications/IMG/pdf/9-3-page13.pdf
	/// http://www.microsoft.com/technet/technetmag/issues/2007/01/CableGuy/default.aspx?loc=fr
	/// Better NAT traversal : http://www.codeproject.com/useritems/STUN_client.asp
	/// 
	/// http://www.tcpipguide.com/free/index.htm
	///
	/// Documentation :
	/// ---------------
	// Test : http://www.pcausa.com/Utilities/pcattcp.htm
	///
	/// UDP server : http://clutch-inc.com/blog/?p=4
	///
	/// TCP: http://www.commentcamarche.net/internet/tcp.php3
	///		http://www.lri.fr/~colette/AnimationFlash/anim_TCP.swf
	///		http://iptables-tutorial.frozentux.net/fr/x1391.html
	///		http://en.wikipedia.org/wiki/Transmission_Control_Protocol
	///		http://www-lsr.imag.fr/users/Andrzej.Duda/PS/2-eme-annee/TCP.pdf
	///
	/// HSTCP: http://www.hep.ucl.ac.uk/~ytl/tcpip/highspeedtcp/hstcp/index.html
	///
	/// Based on TCP:
	/// ------------
	/// http://www.cs.utk.edu/~dunigan/tcptour/
	/// http://abcdrfc.free.fr/rfc-vf/rfc793.html (French)
	/// http://abcdrfc.free.fr/rfc-vf/pdf/rfc793.pdf
	///
	/// Some documentation at :
	/// -----------------------
	/// TCP :
	/// http://www.soi.wide.ad.jp/class/20020032/slides/11/index_32.html
	/// http://www.univ-orleans.fr/sciences/info/ressources/Modules/master2/cci_reseau/ccicours/ch3_couche_transport_1page.pdf
	/// 
	/// UDT :
	/// http://www.cs.uic.edu/~ygu/paper/udt-comnet-v3.pdf
	/// http://udt.sourceforge.net/doc/draft-gg-udt-01.txt
	/// http://www.cs.uic.edu/~ygu/paper/pfldnet04-udt-uic-anl.ppt
	/// </summary>

	#endregion

	#region Articles to improve this implementation

	// On Making TCP More Robust to Packet Reordering
	// http://216.239.59.104/search?q=cache:yL3n-wdnB4AJ:www.icir.org/mallman/papers/tcp-reorder-ccr.ps+tcp+several+ack+packet&hl=fr&ct=clnk&cd=7&gl=be
	// http://www.icir.org/mallman/papers/

	// UDT
	// http://udt.sf.net

	// Hyper Threading
	// http://www.intel.com/cd/ids/developer/asmo-na/eng/194566.htm?prn=Y

	// http://msdn.microsoft.com/msdnmag/issues/05/08/Concurrency/

	#endregion

	#region ControlThreadInformation

	sealed internal class ControlThreadInformation
	{
		//---- Load balancing
		internal int ControlThreadId;
		internal ProcessThread ProcessThread;
		internal long PreviousProcessorTime;

		// On which processor this control thread executes
		internal int ThreadAffinity;

		internal long LastCheckThreadCPUUsageTS = 0;

		//---- The list of logical sockets
		internal List<RUDPSocket> _rudpSockets = new List<RUDPSocket>();
		internal ReaderWriterLockSlim _rudpSocketsLock = new ReaderWriterLockSlim();

		//---- Control Thread
		internal Thread _protocolControlThread;
		internal ManualResetEvent _protocolControlEvent = new ManualResetEvent(false);

		internal Stopwatch _chargeCheckStopWatch = new Stopwatch();
	}

	#endregion

	static public class RUDPStack
	{

		#region Variables

#if TEST_PACKETLOOSE
		static private Random _looseRandom = new Random();
#endif

		static private readonly bool IsSingleCpuMachine = (Environment.ProcessorCount == 1);

		internal const int UDPHeaderLength = 20 + 8; // 20 bytes for IP , 8 bytes for UDP 
		internal const int RUDPHeaderLength = 47; // 1 + 1 + 1 + 4 + 4 + 4 + 4 * 8

		// The time we wait before sending a keep alive message
		private const long KeepAliveInterval = 9000000; // 9 seconds

		// The time we wait before a bandwith test
		private const long BandwidthInterval = 1000000; // 1 second

		// The frequency of load balancing
		private const long CheckThreadCPUUsageInterval = 1000000; // 1 second

		// Send ACK every 200 ms
		private const int DelayACKTime = 200000;

		static private readonly byte[] Clean8Bytes = new byte[8];

		//---- Stack Management
		static private volatile bool _isStackRunning;

		//---- The list of physical sockets, to manage mapping between RUDP and Physical
		static private Dictionary<IPEndPoint, PhysicalSocket> _physicals = new Dictionary<IPEndPoint, PhysicalSocket>(1000);

		//---- ControlThreadInformation
		static private int ProcessorsCount = Environment.ProcessorCount;
		static private ControlThreadInformation[] _controlThreadInformations;
		static private long[] _controlThreadCPUUsages;
		static private object _loadBalancingSync = new object();

		#endregion

		#region Constructor

		static RUDPStack()
		{
			AppDomain.CurrentDomain.ProcessExit += new EventHandler(CurrentDomain_ProcessExit);

			_isStackRunning = true;

			//---- Set to the maximum affinity
			UpdateAffinity();

			//---- Set up the control threads
			_controlThreadInformations = new ControlThreadInformation[ProcessorsCount];
			_controlThreadCPUUsages = new long[ProcessorsCount];
			int affinity = 1;
			for (int index = 0; index < ProcessorsCount; index++)
			{
				ControlThreadInformation info = new ControlThreadInformation();

				//---- Timers
				info.ThreadAffinity = affinity;
				info.ControlThreadId = index;
				info._protocolControlThread = new Thread(new ParameterizedThreadStart(ControlTimer));
				info._protocolControlThread.Name = "RUDP Control Timer - CPU(" + index + ")";
				info._protocolControlThread.IsBackground = true;
				info._protocolControlThread.Priority = ThreadPriority.Normal;

				_controlThreadInformations[index] = info;

				info._protocolControlThread.Start(info);

				affinity = affinity * 2;
			}
		}

		#endregion

		#region StopStack

		static internal void StopStack()
		{
			//---- Close all the connections
			for (int index = 0; index < _controlThreadInformations.Length; index++)
			{
				_controlThreadInformations[index]._rudpSocketsLock.EnterWriteLock();

				for (int rudpIndex = _controlThreadInformations[index]._rudpSockets.Count - 1; rudpIndex > -1; rudpIndex--)
					Close(_controlThreadInformations[index]._rudpSockets[rudpIndex]);

				_controlThreadInformations[index]._rudpSocketsLock.ExitWriteLock();
			}

			//---- Stop everything
			_isStackRunning = false;
		}

		#endregion

		#region Instance Factory

		/// <summary>
		/// Static method to get/create a PhysicalSocket
		/// </summary>
		/// <param name="endPoint">The binded end point</param>
		internal static PhysicalSocket GetInstance(IPEndPoint endPoint)
		{
			lock (_physicals)
			{
				PhysicalSocket physical;

				bool isAnyEndPoint = endPoint.Equals(new IPEndPoint(IPAddress.Any, 0));

				// Check if there is already an existing instance
				if (!isAnyEndPoint)
				{
					if (_physicals.TryGetValue(endPoint, out physical))
						return physical;

					// In case no instance exists, create one
					physical = new PhysicalSocket();
					physical.Bind(endPoint);
					RegisterPhysicalSocket(physical);
					_physicals.Add(endPoint, physical);
				}
				else
				{
					physical = new PhysicalSocket();

					while (true)
					{
						int port = new Random().Next(Int16.MaxValue);
						IPAddress localAddress = LocalIPAddress(ProtocolType.IPv4);
						endPoint = new IPEndPoint(localAddress, port);

						// In case no instance exists, create one
						try
						{
							physical.Bind(endPoint);
							break;
						}
						catch { }
					}

					RegisterPhysicalSocket(physical);
					_physicals.Add(endPoint, physical);
				}

				return physical;
			}
		}

		/// <summary>
		/// Release a PhysicalSocket instance and all its resources.
		/// </summary>
		/// <param name="physical">The socket to release</param>
		internal static void ReleaseInstance(PhysicalSocket physical)
		{
			lock (_physicals)
			{
				physical.Dispose();
				_physicals.Remove((IPEndPoint)physical._socket.LocalEndPoint);
			}
		}

		#endregion

		#region RegisterPhysicalSocket

		static private void RegisterPhysicalSocket(PhysicalSocket physical)
		{
			EndPoint tempEndPoint = (EndPoint)physical._canReceiveFromEndPoint;
			physical._socket.BeginReceiveFrom(physical._receiveBuffer, 0, physical._receiveBuffer.Length, SocketFlags.None, ref tempEndPoint, new AsyncCallback(OnEndReceive), physical);
		}

		#endregion

		#region RegisterConnectedSocket

		internal static void RegisterRUDPSocket(RUDPSocket rudp)
		{
			int controlThreadId = -1;
			int count = Int32.MaxValue;
			for (int index = 0; index < _controlThreadInformations.Length; index++)
				if (_controlThreadInformations[index]._rudpSockets.Count < count)
				{
					controlThreadId = index;
					count = _controlThreadInformations[index]._rudpSockets.Count;
				}

			// Register the socket
			rudp._controlThreadId = controlThreadId;
			_controlThreadInformations[controlThreadId]._rudpSocketsLock.EnterWriteLock();
			if (!_controlThreadInformations[controlThreadId]._rudpSockets.Contains(rudp))
				_controlThreadInformations[controlThreadId]._rudpSockets.Add(rudp);
			_controlThreadInformations[controlThreadId]._rudpSocketsLock.ExitWriteLock();
		}

		internal static void UnregisterRUDPSocket(RUDPSocket rudp)
		{
			if (rudp._controlThreadId > -1)
			{
				int controlThreadId = rudp._controlThreadId;
				_controlThreadInformations[controlThreadId]._rudpSocketsLock.EnterWriteLock();
				_controlThreadInformations[controlThreadId]._rudpSockets.Remove(rudp);
				rudp._controlThreadId = -1;
				_controlThreadInformations[controlThreadId]._rudpSocketsLock.ExitWriteLock();
			}
		}

		#endregion

		#region BeginConnect

		internal static RUDPSocketError BeginConnect(RUDPSocket rudp, int timeOut)
		{
			Trace("Connecting to :" + rudp._remoteEndPoint);

			if (rudp._status == RUDPSocketStatus.Connected)
				return RUDPSocketError.IsConnected;

			if (rudp._status == RUDPSocketStatus.Connecting)
				return RUDPSocketError.AlreadyInProgress;

			//---- Set the status
			rudp.Reset(RUDPSocketStatus.Connecting);

			//---- Register for the stack
			RUDPStack.RegisterRUDPSocket(rudp);

			//---- Send a ping
			if (rudp.IsRendezVousMode)
				PushPacketToSend(rudp, true, RUDPPacketChannel.PingRendezVous, null, 0, 0);
			else
				PushPacketToSend(rudp, true, RUDPPacketChannel.Ping, null, 0, 0);

			return RUDPSocketError.Success;
		}

		#endregion

		#region BeginAccept

		internal static bool BeginAccept(RUDPSocket rudp)
		{
			Trace("Accepting at :" + rudp._remoteEndPoint);

			if (rudp._status != RUDPSocketStatus.Accepting)
				return false;

			//---- Register for the stack
			RUDPStack.RegisterRUDPSocket(rudp);

			return true;
		}

		#endregion

		#region CurrentDomain_ProcessExit

		private static void CurrentDomain_ProcessExit(object sender, EventArgs e)
		{
			try
			{
				for (int index = 0; index < _controlThreadInformations.Length; index++)
				{
					// Try to shutdow gracefully, but it is not sure because the process is killed
					_controlThreadInformations[index]._rudpSocketsLock.EnterUpgradeableReadLock();

					for (int rudpIndex = _controlThreadInformations[index]._rudpSockets.Count - 1; rudpIndex > -1; rudpIndex--)
						Shutdown(_controlThreadInformations[index]._rudpSockets[rudpIndex]);

					_controlThreadInformations[index]._rudpSocketsLock.ExitUpgradeableReadLock();
				}
			}
			catch { }
		}

		#endregion

		#region Close

		/// <summary>
		/// Close the socket. Send the tear down message.
		/// </summary>
		internal static void Close(RUDPSocket rudp)
		{
			if (rudp._status == RUDPSocketStatus.Closed)
				return;

			if (rudp._status == RUDPSocketStatus.Accepting)
			{
				rudp.Reset(RUDPSocketStatus.Closed);
				return;
			}

			AsyncShutdown(rudp);
		}

		private static void AsyncShutdown(RUDPSocket rudp)
		{
			ThreadPool.QueueUserWorkItem(new WaitCallback(AsyncShutdownCB), rudp);
		}

		private static void AsyncShutdownCB(object state)
		{
			Shutdown((RUDPSocket)state);
		}

		#endregion

		#region Shutdown

		internal static void Shutdown(RUDPSocket rudp)
		{
			if (rudp._status == RUDPSocketStatus.Accepting)
			{
				rudp.Reset(RUDPSocketStatus.Closed);
				return;
			}

			if (rudp._status == RUDPSocketStatus.Closed ||
				rudp._isShutingDown)
				return;

			if (rudp._status == RUDPSocketStatus.Closing ||
				rudp._status == RUDPSocketStatus.ClosingACKed)
				return;

			if (rudp._status == RUDPSocketStatus.Connecting)
			{
				rudp.Reset(RUDPSocketStatus.Closed);
				return;
			}

			//---- Send the tear down message

			//-- Update the status
			rudp._isShutingDown = true;
			rudp._status = RUDPSocketStatus.Closing;

			//-- Wait for sending
			while (!rudp._controlWindow.CanSend(0))
			{
				if (rudp._status != RUDPSocketStatus.Closing)
					return;
				Thread.Sleep(100);
			}

			//-- Send the tear down message
			PushPacketToSend(rudp, true, RUDPPacketChannel.TearDown, null, 0, 0);

			//---- Currently closing the connection, wait for the end of the operation
			long startTime = HiResTimer.MicroSeconds;

			//-- Wait until closed
			// Wait until "ClosingACKed"
			// Wait until we have receive the "TearDown" message too and send the ACK
			// Wait until "Time out"
			while (rudp._status == RUDPSocketStatus.Closing &&
					rudp._outgoingPackets.Count > 0 &&
					(HiResTimer.MicroSeconds - startTime) < rudp._sto)
			{
				Thread.Sleep(100);
			}

			//---- Set the status as closed
			rudp.Reset(RUDPSocketStatus.Closed);

			//---- Notify
			rudp._physical.OnDisconnected(rudp, RUDPSocketError.Shutdown);
		}

		#endregion

		#region SendPayload

		internal static RUDPSocketError SendPayload(RUDPSocket rudp, byte[] payload, int offset, int payloadLength, bool reliable, RUDPSendIAsyncResult asyncResult)
		{
			// We are no longer active
			if (!_isStackRunning)
				return RUDPSocketError.SystemNotReady;

			//---- Only when connected
			if (rudp._status != RUDPSocketStatus.Connected)
				return RUDPSocketError.NotConnected;

			//---- Fragmentation
			asyncResult.ForceAsyncCall = true;
			FragmentInformation fragments = NewFragmentInformation(rudp, reliable, payload, offset, payloadLength, asyncResult);

			//----- Async send
			rudp._fragmentsLock.EnterWriteLock();
			rudp._fragments.AddFirst(fragments);
			rudp._fragmentsLock.ExitWriteLock();

			ForceFragmentsSending(rudp._controlThreadId);

			//---- If possible send Sync... otherwise relate to control thread
			/*
			if (!SendFragments(fragments))
			{
				rudp._fragmentsLock.EnterWriteLock();
				rudp._fragments.AddFirst(fragments);
				rudp._fragmentsLock.ExitWriteLock();

				_protocolControlEvent.Set();
			}
			*/

			//---- Synchrone send
			/*
			while (!SendFragments(fragments))
				fragments.rudp._controlWindow.WaitObject.WaitOne();
			*/
			return fragments.Error;
		}

		#endregion

		#region PushPacketToSend

		internal static bool PushPacketToSend(RUDPSocket rudp,
											bool reliablePacket,
											RUDPPacketChannel channel,
											byte[] payload,
											int offset,
											int payloadLength)
		{
			int packetId = -1;
			if (reliablePacket)
				packetId = Interlocked.Increment(ref rudp._ougoingPacketId);

			//---- Get the SACKs
			SACKSlot slot1 = null;
			SACKSlot slot2 = null;
			SACKSlot slot3 = null;
			SACKSlot slot4 = null;
			rudp._sackWindow.GetSLACKSlots(out slot1, out slot2, out slot3, out slot4);

			//---- Copy the payload to send
			byte[] rudpPayload = MakePacketPayload(rudp, packetId, channel, slot1, slot2, slot3, slot4, payload, offset, payloadLength);

			//---- Create a packet
			RUDPOutgoingPacket packet = NewOutgoingPacket(packetId, rudp._sequence, rudpPayload, channel);
			packet.CurrentCwnd = rudp._controlWindow.CWND;

			if (reliablePacket)
			{
				//---- Notify the control window
				rudp._controlWindow.OnSend(packetId, rudp._sequence, payloadLength);

				//---- Increment sequence number
				Interlocked.Exchange(ref rudp._sequence, rudp._sequence + payloadLength);
			}

			//---- In the "resend list"
			if (reliablePacket)
			{
				rudp._outgoingPacketsLock.EnterWriteLock();
				rudp._outgoingPackets.Add(packet);
				rudp._outgoingPacketsLock.ExitWriteLock();
			}

			//---- Send the packet
			packet.TSFirstSend = HiResTimer.MicroSeconds;
			if (!SocketSendPacket(rudp, packet, packet.Payload, packet.TSFirstSend))
			{
				// Nothing to do... socket is closed and reseted !
				return false;
			}

			return true;
		}

		#endregion

		#region MakePacketPayload

		/// <summary>
		/// Create the RUDP packet : HEADER + payload
		/// </summary>
		internal static byte[] MakePacketPayload(RUDPSocket rudp,
												int packetId,
												RUDPPacketChannel channel,
												SACKSlot slot1, SACKSlot slot2, SACKSlot slot3, SACKSlot slot4,
												byte[] payload,
												int offset,
												int payloadLength)
		{
			//---- Here we make the payload while we will send:
			/**
			 * We create a header
			 * The format is:
			 * --------------
			 * - 1 byte : protocol version
			 * - 1 byte : header information
			 * - 1 byte : channel
			 * - 4 bytes : message ID
			 * - 4 bytes : advertized congestion window size
			 * - 4 bytes : payloadLength
			 * - 4 X (4 + 4) : 32 bytes for 4 SACK slots
			 * - the payload bytes
			 */
			int headerOffset = 0;
			byte[] packetPayload = PayloadManager.Allocate(channel, RUDPHeaderLength + payloadLength);

			//---- Protocol version
			packetPayload[headerOffset] = (byte)1;
			headerOffset++;

			//---- Header information
			// 3 bits : number of ACK slots

			// sack slot (3 bits)
			byte sacksSlotCount = 0;
			if (slot1 != null)
				sacksSlotCount++;
			if (slot2 != null)
				sacksSlotCount++;
			if (slot3 != null)
				sacksSlotCount++;
			if (slot4 != null)
				sacksSlotCount++;

			packetPayload[headerOffset] = 0; // Reset (Because buffer reused)
			packetPayload[headerOffset] |= sacksSlotCount;
			headerOffset++;

			//---- Channel
			packetPayload[headerOffset] = (byte)channel;
			headerOffset++;

			//---- Message Id
			BinaryHelper.WriteInt(packetId, packetPayload, headerOffset);
			headerOffset += 4;

			//---- Control information : Advertised window
			if (rudp == null)
				BinaryHelper.WriteInt(-1, packetPayload, headerOffset);
			else
				BinaryHelper.WriteInt((int)rudp._controlWindow.AdvertisedWindow, packetPayload, headerOffset);
			headerOffset += 4;

			//---- Payload length
			BinaryHelper.WriteInt(payloadLength, packetPayload, headerOffset);
			headerOffset += 4;

			//---- SACK Slots
			if (slot1 != null)
			{
				BinaryHelper.WriteInt(slot1.StartPacketId, packetPayload, headerOffset);
				headerOffset += 4;
				BinaryHelper.WriteInt(slot1.EndPacketId, packetPayload, headerOffset);
				headerOffset += 4;
			}
			else
			{
				// Reset (Because buffer reused)
				Buffer.BlockCopy(Clean8Bytes, 0, packetPayload, headerOffset, 8);
				headerOffset += 8;
			}
			if (slot2 != null)
			{
				BinaryHelper.WriteInt(slot2.StartPacketId, packetPayload, headerOffset);
				headerOffset += 4;
				BinaryHelper.WriteInt(slot2.EndPacketId, packetPayload, headerOffset);
				headerOffset += 4;
			}
			else
			{
				// Reset (Because buffer reused)
				Buffer.BlockCopy(Clean8Bytes, 0, packetPayload, headerOffset, 8);
				headerOffset += 8;
			}
			if (slot3 != null)
			{
				BinaryHelper.WriteInt(slot3.StartPacketId, packetPayload, headerOffset);
				headerOffset += 4;
				BinaryHelper.WriteInt(slot3.EndPacketId, packetPayload, headerOffset);
				headerOffset += 4;
			}
			else
			{
				// Reset (Because buffer reused)
				Buffer.BlockCopy(Clean8Bytes, 0, packetPayload, headerOffset, 8);
				headerOffset += 8;
			}
			if (slot4 != null)
			{
				BinaryHelper.WriteInt(slot4.StartPacketId, packetPayload, headerOffset);
				headerOffset += 4;
				BinaryHelper.WriteInt(slot4.EndPacketId, packetPayload, headerOffset);
				headerOffset += 4;
			}
			else
			{
				// Reset (Because buffer reused)
				Buffer.BlockCopy(Clean8Bytes, 0, packetPayload, headerOffset, 8);
				headerOffset += 8;
			}

			if (payload == null)
				return packetPayload;

			//---- Payload
			Buffer.BlockCopy(payload, offset, packetPayload, headerOffset, payloadLength);

			return packetPayload;
		}

		#endregion

		#region UpdatePacketPayload

		static private void UpdatePacketPayload(byte[] packetPayload, SACKSlot slot1, SACKSlot slot2, SACKSlot slot3, SACKSlot slot4)
		{
			//---- Update header
			byte sacksSlotCount = 0;
			if (slot1 != null)
				sacksSlotCount++;
			if (slot2 != null)
				sacksSlotCount++;
			if (slot3 != null)
				sacksSlotCount++;
			if (slot4 != null)
				sacksSlotCount++;

			packetPayload[1] = sacksSlotCount;

			//---- Update slots
			int headerOffset = 15;
			if (slot1 != null)
			{
				BinaryHelper.WriteInt(slot1.StartPacketId, packetPayload, headerOffset);
				headerOffset += 4;
				BinaryHelper.WriteInt(slot1.EndPacketId, packetPayload, headerOffset);
				headerOffset += 4;
			}
			else headerOffset += 8;
			if (slot2 != null)
			{
				BinaryHelper.WriteInt(slot2.StartPacketId, packetPayload, headerOffset);
				headerOffset += 4;
				BinaryHelper.WriteInt(slot2.EndPacketId, packetPayload, headerOffset);
				headerOffset += 4;
			}
			else headerOffset += 8;
			if (slot3 != null)
			{
				BinaryHelper.WriteInt(slot3.StartPacketId, packetPayload, headerOffset);
				headerOffset += 4;
				BinaryHelper.WriteInt(slot3.EndPacketId, packetPayload, headerOffset);
				headerOffset += 4;
			}
			else headerOffset += 8;
			if (slot4 != null)
			{
				BinaryHelper.WriteInt(slot4.StartPacketId, packetPayload, headerOffset);
				headerOffset += 4;
				BinaryHelper.WriteInt(slot4.EndPacketId, packetPayload, headerOffset);
				headerOffset += 4;
			}
			//else headerOffset += 8;
		}

		#endregion

		#region SocketSendACK

		private static bool SocketSendACK(RUDPSocket rudp,
											PhysicalSocket physical,
											IPEndPoint remoteEndPoint,
											byte[] rudpPayload)
		{
			try
			{
				physical._socket.SendTo(rudpPayload, remoteEndPoint);
				//physical._socket.BeginSendTo(rudpPayload, 0, rudpPayload.Length, SocketFlags.None, rudp._remoteEndPoint, null, null);
			}
			catch (SocketException exception)
			{
				if (rudp != null)
					OnSocketUnhandledError(rudp, SocketErrorToRUDPSocketError(exception.SocketErrorCode), null);

				return false;
			}

			if (rudp != null)
				rudp._lastACKSendTS = HiResTimer.MicroSeconds;

			return true;
		}

		#endregion

		#region SocketSendPacket

		private static bool SocketSendPacket(RUDPSocket rudp, RUDPOutgoingPacket packet, byte[] rudpPayload, long now)
		{
			//---- Send the request
			try
			{
				rudp._physical._socket.SendTo(rudpPayload, rudp._remoteEndPoint);
				//rudp._physical._socket.BeginSendTo(rudpPayload, 0, rudpPayload.Length, SocketFlags.None, rudp._remoteEndPoint, null, null);
			}
			catch (SocketException exception)
			{
				if (exception.ErrorCode == (int)SocketError.MessageSize && packet.Channel == RUDPPacketChannel.MTUTuning)
				{
					// ICMP type 3 subtype 4
					// ICMP message, tell that this packet is too big
					rudp._pmtuDiscovery.OnICMPError(packet);
					return true;
				}

				OnSocketUnhandledError(rudp, SocketErrorToRUDPSocketError(exception.SocketErrorCode), null);
				return false;
			}

			rudp._lastSendTS = now;
			packet.TSLastSend = now;

			return true;
		}

		#endregion

		#region OnEndReceive

		/// <summary>
		/// Receive bytes from a socket
		/// </summary>
		private static void OnEndReceive(IAsyncResult result)
		{
			PhysicalSocket physical = (PhysicalSocket)result.AsyncState;

			EndPoint tempEndPoint = (EndPoint)physical._canReceiveFromEndPoint;

			//---- End receive
			EndPoint sender = new IPEndPoint(IPAddress.Any, 0);
			int size = -1;

			try
			{
				size = physical._socket.EndReceiveFrom(result, ref sender);
			}
			catch (SocketException socketException)
			{
				// The I/O operation has been aborted because of either 'a thread exit or an application request:
				// What you should really be doing is starting another BeginRead when you see a SocketException
				// with error code 995 (aborted) during EndRead.
				// What this means is that there wasn't any data on the socket to read, but that's fine if
				// you're just trying to read the next thing that comes off the socket.
				// NOTE that you should look at both a SocketException wrapped in an IOException and a straight SocketException. 

				// 995 :WSA_OPERATION_ABORTED
				// Overlapped operation aborted. This Win32 error indicates that an overlapped I/O operation
				// was canceled because of the closure of a socket. In addition, this error can occur when
				// executing the SIO_FLUSH ioctl command.
				if (socketException.ErrorCode == 995)
				{
					// Restart receiving
					physical._socket.BeginReceiveFrom(physical._receiveBuffer, 0, physical._receiveBuffer.Length, SocketFlags.None, ref tempEndPoint, new AsyncCallback(OnEndReceive), physical);
					return;
				}

				if (socketException.ErrorCode == 10054)
					physical._socket.BeginReceiveFrom(physical._receiveBuffer, 0, physical._receiveBuffer.Length, SocketFlags.None, ref tempEndPoint, new AsyncCallback(OnEndReceive), physical);
				else
				{
					OnSocketUnhandledError(physical, sender as IPEndPoint, socketException.SocketErrorCode);
					return;
				}
			}

			//----Simulate packet loss
#if TEST_PACKETLOOSE
			if (_looseRandom.NextDouble() < 0.1)
			{
				physical._socket.BeginReceiveFrom(physical._receiveBuffer, 0, physical._receiveBuffer.Length, SocketFlags.None, ref tempEndPoint, new AsyncCallback(OnEndReceive), physical);
				return;
			}
#endif

			//---- Handle the packet
			RUDPPacketChannel channel = RUDPPacketChannel.Undefined;
			int packetId = -2;
			int advertisedWindowSize = 0;
			SACKSlot slot1 = null;
			SACKSlot slot2 = null;
			SACKSlot slot3 = null;
			SACKSlot slot4 = null;
			byte[] payload = null;

			if (!_isStackRunning)
				return;

			//-- Decode payload
			HandlePayload(physical, physical._receiveBuffer, size, sender as IPEndPoint,
							out channel,
							out packetId,
							out advertisedWindowSize,
							out slot1,
							out slot2,
							out slot3,
							out slot4,
							out payload);

			//-- Restart receiving
			physical._socket.BeginReceiveFrom(physical._receiveBuffer, 0, physical._receiveBuffer.Length, SocketFlags.None, ref tempEndPoint, new AsyncCallback(OnEndReceive), physical);

			//-- Handle the packet

			HandlePacket(physical, sender as IPEndPoint, channel, packetId, advertisedWindowSize, slot1, slot2, slot3, slot4, payload);
		}

		#endregion

		#region HandlePayload

		/// <summary>
		/// Handle the bytes received from a socket.
		/// </summary>
		private static void HandlePayload(PhysicalSocket physical, byte[] payload, int length, IPEndPoint sender,
				out RUDPPacketChannel channel,
				out int packetId,
				out int advertisedWindowSize,
				out SACKSlot slot1,
				out SACKSlot slot2,
				out SACKSlot slot3,
				out SACKSlot slot4,
				out byte[] packetPayload)
		{
			int offset = 0;

			//-- Protocol version
			byte version = payload[offset];
			offset++;

			//-- Header information
			byte sacksSlotCount = payload[offset];
			offset++;

			//-- Channel
			byte channelByte = payload[offset];
			offset++;
			switch (channelByte)
			{
				case 0:
					channel = RUDPPacketChannel.Undefined;
					break;
				case 10:
					channel = RUDPPacketChannel.Ping;
					break;
				case 20:
					channel = RUDPPacketChannel.PingRendezVous;
					break;
				case 30:
					channel = RUDPPacketChannel.TearDown;
					break;
				case 40:
					channel = RUDPPacketChannel.UserPacket;
					break;
				case 50:
					channel = RUDPPacketChannel.ACK;
					break;
				case 70:
					channel = RUDPPacketChannel.OutOfOrder;
					break;
				case 100:
					channel = RUDPPacketChannel.KeepAlive;
					break;
				case 120:
					channel = RUDPPacketChannel.MTUTuning;
					break;
				case 121:
					channel = RUDPPacketChannel.MTUTuningACK;
					break;
				case 150:
					channel = RUDPPacketChannel.Bandwidth01;
					break;
				case 151:
					channel = RUDPPacketChannel.BandwidthResponse01;
					break;
				case 155:
					channel = RUDPPacketChannel.Bandwidth02;
					break;
				case 156:
					channel = RUDPPacketChannel.BandwidthResponse02;
					break;
				default:
					throw new Exception("Unsupported channel type");
			}

			//-- Packet Id
			packetId = BinaryHelper.ReadInt(payload, offset);
			offset += 4;

			//-- Control information : Advertised window
			advertisedWindowSize = BinaryHelper.ReadInt(payload, offset);
			offset += 4;

			//-- Payload length
			int payloadLength = BinaryHelper.ReadInt(payload, offset);
			offset += 4;

			//---- SACK Slots
			slot1 = null;
			slot2 = null;
			slot3 = null;
			slot4 = null;
			int startPacketId;
			int endPacketId;
			if (sacksSlotCount > 0)
			{
				startPacketId = BinaryHelper.ReadInt(payload, offset);
				offset += 4;
				endPacketId = BinaryHelper.ReadInt(payload, offset);
				offset += 4;
				slot1 = new SACKSlot(startPacketId, endPacketId);
			}
			else offset += 8;
			if (sacksSlotCount > 1)
			{
				startPacketId = BinaryHelper.ReadInt(payload, offset);
				offset += 4;
				endPacketId = BinaryHelper.ReadInt(payload, offset);
				offset += 4;
				slot2 = new SACKSlot(startPacketId, endPacketId);
			}
			else offset += 8;
			if (sacksSlotCount > 2)
			{
				startPacketId = BinaryHelper.ReadInt(payload, offset);
				offset += 4;
				endPacketId = BinaryHelper.ReadInt(payload, offset);
				offset += 4;
				slot3 = new SACKSlot(startPacketId, endPacketId);
			}
			else offset += 8;
			if (sacksSlotCount > 3)
			{
				startPacketId = BinaryHelper.ReadInt(payload, offset);
				offset += 4;
				endPacketId = BinaryHelper.ReadInt(payload, offset);
				offset += 4;
				slot4 = new SACKSlot(startPacketId, endPacketId);
			}
			else offset += 8;

			//-- Payload
			packetPayload = new byte[payloadLength];
			if (payloadLength > 0)
				Buffer.BlockCopy(payload, offset, packetPayload, 0, payloadLength);
		}

		#endregion

		#region HandlePacket

		private static void HandlePacket(PhysicalSocket physical,
										IPEndPoint sender,
										RUDPPacketChannel channel,
										int packetId,
										int advertisedWindowSize,
										SACKSlot slot1, SACKSlot slot2, SACKSlot slot3, SACKSlot slot4,
										byte[] payload)
		{
			RUDPSocket rudp = null;

			//---- PING
			if (channel == RUDPPacketChannel.Ping || channel == RUDPPacketChannel.PingRendezVous)
			{
				rudp = HandlePing(physical, sender, packetId, channel);

				// Do not handle this message
				if (rudp == null)
					return;
			}

			//---- Search the socket
			if (rudp == null)
			{
				physical._connectedRDUPsLock.EnterReadLock();
				physical._connectedRDUPs.TryGetValue(sender, out rudp);
				physical._connectedRDUPsLock.ExitReadLock();
			}

			//---- Direct send of ACK, because socket can be shutdowned and removed
			if (channel == RUDPPacketChannel.TearDown)
			{
				byte[] packetPayload = MakePacketPayload(rudp, -1, RUDPPacketChannel.ACK, new SACKSlot(packetId, packetId), null, null, null, null, 0, 0);
				SocketSendACK(rudp, physical, sender, packetPayload);
				PayloadManager.Deallocate(RUDPPacketChannel.ACK, packetPayload);
			}

			//---- Released socket
			if (rudp == null)
				return;

#if CONSOLE_TRACE
			if (packetId > -1)
				Trace("Handle packet (" + rudp.Handle + ")(" + channel + "):" + packetId);
#endif

			//---- Advertised window
			rudp._controlWindow.OnReceiveAdvertisedWindow(advertisedWindowSize);

			//---- Handle ACKs
			HandleACKs(rudp, slot1, slot2, slot3, slot4);

			if (channel == RUDPPacketChannel.ACK)
				return;

			//---- Non reliable messages
			if (packetId < 0)
			{
				//-- Bandwidth
				if (channel == RUDPPacketChannel.Bandwidth01)
				{
					PushPacketToSend(rudp, false, RUDPPacketChannel.BandwidthResponse01, null, 0, 0);
					return;
				}
				else if (channel == RUDPPacketChannel.Bandwidth02)
				{
					PushPacketToSend(rudp, false, RUDPPacketChannel.BandwidthResponse02, payload, 0, 8);
					return;
				}
				else if (channel == RUDPPacketChannel.BandwidthResponse01)
				{
					rudp._bandwidthResponse01TS = HiResTimer.MicroSeconds;
				}
				else if (channel == RUDPPacketChannel.BandwidthResponse02)
				{
					//---- Calculate bandwidth
					// Bdw (Bytes / milli-sec)
					long now = HiResTimer.MicroSeconds;
					double delay = (now - rudp._bandwidthResponse01TS) / 1000;
					if (delay < 0.001)
						delay = 0.001;

					// Arrival Speed
					double arrivalSpeed = (RUDPHeaderLength + UDPHeaderLength) / delay;

					// RTT
					double currentRtt = (now - BinaryHelper.ReadInt(payload, 0)) / 1000;
					if (currentRtt < 0.001)
						currentRtt = 0.001;

					// BDP = Bandwidth(Byte / Ms) * RTT;
					double bandwidth = (long)(arrivalSpeed * currentRtt);
					rudp._bandwidth = (long)(rudp._bandwidth * 0.875f + bandwidth * 0.125f);
				}

				//-- MTU Tuning
				else if (channel == RUDPPacketChannel.MTUTuning)
				{
					rudp._pmtuDiscovery.OnReceiveProbe(payload.Length);
					return;
				}
				else if (channel == RUDPPacketChannel.MTUTuningACK)
				{
					rudp._pmtuDiscovery.OnReceiveProbeACK(payload);
					return;
				}

				//if ((rudp._incomingNonReliablePackets.Count * rudp._mtu) >= rudp._receiveSize)
				//return;

				RUDPIngoingPacket nonReliablePacket = new RUDPIngoingPacket(rudp, packetId, payload, channel, HiResTimer.MicroSeconds);
				rudp._incomingPackets.AddPacket(nonReliablePacket);

				rudp.HandleNextUserPacket(false);
				return;
			}

			//---- Do not process a duplicated packets
			bool isDuplicatedPacket;
			isDuplicatedPacket = (packetId <= rudp._incomingPackets.CurrentPacketId);
			if (!isDuplicatedPacket)
				isDuplicatedPacket = rudp._incomingPackets.ContainsPacket(packetId);

			//---- Can I receive the packet now ?
			bool canReceive = rudp._controlWindow.CanReceive(packetId, payload.Length);

			//---- Send the ACK
			if (channel != RUDPPacketChannel.Ping && channel != RUDPPacketChannel.PingRendezVous)
				if (canReceive ||	// Can receive, then we send ACK
					(!canReceive && isDuplicatedPacket))	// Is duplicated, then already in the list -> send another ACK
					rudp._sackWindow.OnReceivePacket(packetId);

			//---- Check if we can handle this message
			if (!canReceive || isDuplicatedPacket)
				return;

			//---- If we are not connected, we cannot hanlde messages ! We need a connection before.
			if (rudp._status != RUDPSocketStatus.Connected && channel == RUDPPacketChannel.UserPacket)
				return;

			//---- Check for Out of order packets
			if (rudp._incomingPackets.Count > 0)
			{
				int greaterPacketId = rudp._incomingPackets.LastPacketId;
				if (packetId != greaterPacketId + 1)
				{
					byte[] oooPayload = new byte[8];
					// start packet
					BinaryHelper.WriteInt(greaterPacketId + 1, oooPayload, 0);
					// end packet
					BinaryHelper.WriteInt(packetId - 1, oooPayload, 4);
					PushPacketToSend(rudp, false, RUDPPacketChannel.OutOfOrder, oooPayload, 0, 8);
				}
			}

			//---- Receive Out of order notification
			if (channel == RUDPPacketChannel.OutOfOrder)
			{
				rudp._controlWindow.OnOutOfOrder(BinaryHelper.ReadInt(payload, 0), BinaryHelper.ReadInt(payload, 4));
			}

			//---- TEAR DOWN
			if (channel == RUDPPacketChannel.TearDown)
			{
				// Initiate the close process
				if (rudp._status == RUDPSocketStatus.Connected)
				{
					// Notify control window
					rudp._controlWindow.OnReceive(null);

					// Start shutdown
					AsyncShutdown(rudp);
				}

				return;
			}

			//---- Add the packet to incoming list
			RUDPIngoingPacket packet = new RUDPIngoingPacket(rudp, packetId, payload, channel, HiResTimer.MicroSeconds);

			// Notify control window
			rudp._controlWindow.OnReceive(packet);
			rudp._incomingPackets.AddPacket(packet);

			//------ Handle the ordered ingoing packets
			rudp.HandleNextUserPacket(false);
		}

		#endregion

		#region HandleACKs

		private static void HandleACKs(RUDPSocket rudp,
										SACKSlot slot1,
										SACKSlot slot2,
										SACKSlot slot3,
										SACKSlot slot4)
		{
			// No ack
			if (slot1 == null)
				return;

			int maxId = slot1.EndPacketId;
			if (slot4 != null)
				maxId = slot4.EndPacketId;
			else if (slot3 != null)
				maxId = slot3.EndPacketId;
			else if (slot2 != null)
				maxId = slot2.EndPacketId;

#if CONSOLE_TRACE
			if (slot1 != null)
				Trace("Handle ACK[1](" + rudp.Handle + "): " + slot1.StartPacketId + " <-> " + slot1.EndPacketId);
			if (slot2 != null)
				Trace("Handle ACK[2](" + rudp.Handle + "): " + slot2.StartPacketId + " <-> " + slot2.EndPacketId);
			if (slot3 != null)
				Trace("Handle ACK[3](" + rudp.Handle + "): " + slot3.StartPacketId + " <-> " + slot3.EndPacketId);
			if (slot4 != null)
				Trace("Handle ACK[4](" + rudp.Handle + "): " + slot4.StartPacketId + " <-> " + slot4.EndPacketId);
#endif

			//---- Prepare the list of packets
			List<RUDPOutgoingPacket> toACKPackets = new List<RUDPOutgoingPacket>();

			RUDPOutgoingPacket lastPacket = null;
			double currentRTT = Double.MaxValue;
			rudp._outgoingPacketsLock.EnterReadLock();

			try
			{
				for (int index = 0; index < rudp._outgoingPackets.Count; index++)
				{
					RUDPOutgoingPacket packet = rudp._outgoingPackets[index];

					if (packet.PacketId > maxId)
						break;

					if (packet.IsACKed)
						continue;

					if (slot4 != null)
						if (packet.PacketId >= slot4.StartPacketId && packet.PacketId <= slot4.EndPacketId)
						{
							if (packet.Retransmission < 1)
							{
								lastPacket = packet;
								currentRTT = Math.Min(currentRTT, HiResTimer.MicroSeconds - lastPacket.TSFirstSend);
							}
							toACKPackets.Add(packet);
							continue;
						}

					if (slot3 != null)
						if (packet.PacketId >= slot3.StartPacketId && packet.PacketId <= slot3.EndPacketId)
						{
							if (packet.Retransmission < 1)
							{
								lastPacket = packet;
								currentRTT = Math.Min(currentRTT, HiResTimer.MicroSeconds - lastPacket.TSFirstSend);
							}
							toACKPackets.Add(packet);
							continue;
						}

					if (slot2 != null)
						if (packet.PacketId >= slot2.StartPacketId && packet.PacketId <= slot2.EndPacketId)
						{
							if (packet.Retransmission < 1)
							{
								lastPacket = packet;
								currentRTT = Math.Min(currentRTT, HiResTimer.MicroSeconds - lastPacket.TSFirstSend);
							}
							toACKPackets.Add(packet);
							continue;
						}

					if (packet.PacketId >= slot1.StartPacketId && packet.PacketId <= slot1.EndPacketId)
					{
						if (packet.Retransmission < 1)
						{
							lastPacket = packet;
							currentRTT = Math.Min(currentRTT, HiResTimer.MicroSeconds - lastPacket.TSFirstSend);
						}
						toACKPackets.Add(packet);
					}
				}
			}
			finally
			{
				rudp._outgoingPacketsLock.ExitReadLock();
			}

			//---- If no good packet, use current RTT
			if (lastPacket == null)
				currentRTT = rudp.RTT;

			if (currentRTT < 1)
				currentRTT = 1;

			//---- Set the ACK for all the packets
			for (int index = 0; index < toACKPackets.Count; index++)
			{
				RUDPOutgoingPacket packet = toACKPackets[index];
				SetPacketACKed(rudp, packet, currentRTT);
			}
		}

		#endregion

		#region SetPacketACKed

		private static void SetPacketACKed(RUDPSocket rudp, RUDPOutgoingPacket packet, double currentRTT)
		{
			lock (packet)
			{
				if (packet.IsACKed)
					return;

				rudp._controlWindow.OnACK(packet, currentRTT);

				// Mark as ACKed
				packet.IsACKed = true;
			}

			Trace("Packet ACKed(" + rudp.Handle + "): " + packet.PacketId + " " + packet.Channel);

			//---- Ping ACK
			if ((packet.Channel == RUDPPacketChannel.Ping || packet.Channel == RUDPPacketChannel.PingRendezVous) &&
				rudp._status == RUDPSocketStatus.Connecting)
			{
				rudp._status = RUDPSocketStatus.Connected;

				// MTU tuning
				if (rudp._usePMTUDiscovery)
					rudp._pmtuDiscovery.StartTuning();

				// connection done
				rudp.OnEndConnect(RUDPSocketError.Success);

				return;
			}

			//---- Tear Down ACK : It was a tear down message, it has been received, we can close
			if (packet.Channel == RUDPPacketChannel.TearDown &&
				rudp._status == RUDPSocketStatus.Closing)
			{
				rudp._status = RUDPSocketStatus.ClosingACKed;

				// Remove it to our list of "connected" sockets
				if (rudp._remoteEndPoint != null)
				{
					// Unregister for the stack
					UnregisterRUDPSocket(rudp);
					rudp._physical.UnregisterConnectedSocket(rudp);
				}
			}
		}

		#endregion

		#region HandlePing

		private static RUDPSocket HandlePing(PhysicalSocket physical, IPEndPoint sender, int packetId, RUDPPacketChannel channel)
		{
			RUDPSocket rudp = null;

			physical._connectedRDUPsLock.EnterReadLock();
			physical._connectedRDUPs.TryGetValue(sender, out rudp);
			physical._connectedRDUPsLock.ExitReadLock();

			//---- Ping
			if (channel == RUDPPacketChannel.Ping)
			{
				//-- This connection already exist, duplicated Ping
				if (rudp != null)
				{
					// Resend the ACK
					rudp._sackWindow.OnReceivePacket(packetId);
					return null;
				}

				//-- No accepting socket
				if (physical._acceptingRDUP == null)
				{
					// Maybe the socket is not yet ready for accepting, do nothing
					return null;
				}

				//-- Accept
				rudp = physical.OnEndAccept(sender, packetId);

				//-- ACK connection
				rudp._sackWindow.OnReceivePacket(packetId);

				//return physical._acceptingRDUP;
				return rudp;
			}

			//---- Ping , with Rendez vous
			if (rudp != null && rudp._status == RUDPSocketStatus.Connecting && rudp._isRendezVousMode)
			{
				//---- End of connection
				rudp._status = RUDPSocketStatus.Connected;
				rudp.OnEndConnect(RUDPSocketError.Success);

				//---- Accept the rendez vous connection
				rudp._sackWindow.OnReceivePacket(packetId);

				return rudp;
			}

			return null;
		}

		#endregion

		#region OnDisconnected

		internal static void OnDisconnected(RUDPSocket rudp, DisconnectionReason reason)
		{
			if (rudp._status == RUDPSocketStatus.Closed)
				return;

			//---- Reset
			rudp._outgoingPacketsLock.EnterWriteLock();
			rudp._outgoingPackets.Clear();
			rudp._outgoingPacketsLock.ExitWriteLock();
			rudp.Reset(RUDPSocketStatus.Closed);

			//---- Notify
			if (reason != DisconnectionReason.ConnectionClosed)
			{
				RUDPSocketError error = RUDPSocketError.ConnectionReset;
				if (reason == DisconnectionReason.SocketError)
					error = RUDPSocketError.SocketError;
				if (reason == DisconnectionReason.TimeOut)
					error = RUDPSocketError.ConnectionReset;

				rudp._physical.OnDisconnected(rudp, error);
			}
		}

		#endregion

		#region ControlTimer

		private static void ControlTimer(object controlInformation)
		{
			try
			{
				UpdateThreadAffinity((ControlThreadInformation)controlInformation);
				Thread.BeginThreadAffinity();
				ControlTimerProcessing((ControlThreadInformation)controlInformation);
				Thread.EndThreadAffinity();
			}
			catch (Exception exception)
			{
				Thread.EndThreadAffinity();
				StackFatalException(exception);
			}

			return;
		}

		private static void ControlTimerProcessing(ControlThreadInformation controlInformation)
		{
			while (Thread.CurrentThread.IsAlive && _isStackRunning)
			{
				long now = HiResTimer.MicroSeconds;

				//---- Check load balancing
				LoadBalancingTimer(controlInformation, now);

				//---- Processing
				bool canResetEvent = true;
				for (int index = controlInformation._rudpSockets.Count - 1; index > -1; index--)
				{
					RUDPSocket rudp = controlInformation._rudpSockets[index];
					if (rudp._status == RUDPSocketStatus.Closed)
					{
						UnregisterRUDPSocket(rudp);
						continue;
					}

					//-- 0 - Transmission
					canResetEvent &= TransmissionTimer(rudp, controlInformation);

					//-- 1 - Send
					canResetEvent &= RetransmissionTimer(rudp, now, controlInformation);

					//-- 2 - ACKs
					ACKTimer(rudp, now);

					//-- 3 - Check for keep alive
					KeepAliveTimer(rudp, now);

					//-- 4 - MTU Discovery
					rudp._pmtuDiscovery.OnHeartBeat(now);

					//-- 5 - Bandwidth
					BandwidthTimer(rudp, now);
				}

				//---- Do not use 100%
				//Thread.Sleep(1);
				controlInformation._protocolControlEvent.WaitOne(1, true);
				if (canResetEvent)
					controlInformation._protocolControlEvent.Reset();
			}

		}

		#endregion

		#region TransmissionTimer

		private static bool TransmissionTimer(RUDPSocket rudp, ControlThreadInformation controlInformation)
		{
			if (rudp._fragments.Count < 1)
				return true;

			rudp._fragmentsLock.EnterReadLock();
			FragmentInformation fragments = rudp._fragments.Last.Value;
			rudp._fragmentsLock.ExitReadLock();

			if (SendFragments(fragments, controlInformation))
			{
				rudp._fragmentsLock.EnterWriteLock();
				rudp._fragments.RemoveLast();
				rudp._fragmentsLock.ExitWriteLock();
				ReleaseFragmentInformation(fragments);
				return true;
			}

			// Else continue for other socket
			// will try to send packet during next loop
			ForceFragmentsSending(controlInformation.ControlThreadId);

			return false;
		}

		#endregion

		#region SendFragments

		/// <summary>
		/// Send all the fragments.
		/// </summary>
		/// <returns>True if all the fragments have been sent</returns>
		static private bool SendFragments(FragmentInformation fragments, ControlThreadInformation controlInformation)
		{
			controlInformation._chargeCheckStopWatch.Reset();
			controlInformation._chargeCheckStopWatch.Start();
			while (fragments.Size > 0)
			{
				//---- MSS : Full header (IP + UDP + RUDP)
				int MSS = fragments.rudp._mtu - (RUDPStack.UDPHeaderLength + RUDPStack.RUDPHeaderLength);

				int currentLength = Math.Min(MSS, fragments.Size);

				//---- If reliable, wait for the congestion windows
				if (fragments.IsReliable && !fragments.rudp._controlWindow.CanSend(currentLength))
					return false;

				//---- Send
				if (!PushPacketToSend(fragments.rudp,
									fragments.IsReliable,
									RUDPPacketChannel.UserPacket,
									fragments.Payload,
									fragments.Offset,
									currentLength))
				{
					fragments.Error = RUDPSocketError.SocketError;
					if (fragments.AsyncResult != null)
						OnSocketUnhandledError(fragments.rudp, fragments.Error, fragments.AsyncResult);
					return true;
				}

				fragments.Size -= currentLength;
				fragments.Offset += currentLength;

				//--- Avoid to block the Control thread, because sending a lot of fragments (allow to send ACKs too...)
				if (fragments.Size > 0 && controlInformation._chargeCheckStopWatch.ElapsedMilliseconds > 0)
					return false;
			}

			//---- End of send
			if (fragments.AsyncResult != null)
				fragments.rudp._physical.OnEndSend(fragments.rudp, fragments.AsyncResult);

			return true;
		}

		#endregion

		#region ForceFragmentsSending

		static internal void ForceFragmentsSending(int controlThreadId)
		{
			if (controlThreadId > -1)
				_controlThreadInformations[controlThreadId]._protocolControlEvent.Set();
		}

		#endregion

		#region RetransmissionTimer

		private static bool RetransmissionTimer(RUDPSocket rudp, long now, ControlThreadInformation controlInformation)
		{
			int count = rudp._outgoingPackets.Count;
			bool hasDoFastRetransmit = false;

			controlInformation._chargeCheckStopWatch.Reset();
			controlInformation._chargeCheckStopWatch.Start();
			for (int index = 0; index < count; index++)
			{
				rudp._outgoingPacketsLock.EnterReadLock();
				RUDPOutgoingPacket packet = rudp._outgoingPackets[index];
				rudp._outgoingPacketsLock.ExitReadLock();

				//---- Not yet sended
				if (packet.TSLastSend < 0)
					continue;

				//---- It is ACKed
				if (packet.IsACKed)
				{
					rudp._outgoingPacketsLock.EnterWriteLock();
					rudp._outgoingPackets.RemoveAt(index);
					rudp._outgoingPacketsLock.ExitWriteLock();

					ReleaseOutgoingPacket(packet);

					index--;
					count--;
					continue;
				}

				//---- Check for time out
				if (packet.TSFirstSend > -1 && (now - packet.TSFirstSend) > rudp._sto)
				{
					//-- Normal time out
					// Send connection Reset with ACK
					OnDisconnected(rudp, DisconnectionReason.TimeOut);
					return true;
				}

				//---- Retransmission or not ?
				bool fastRetransmit = (packet.PacketId >= rudp._fastRetransmitStartPacketId && packet.PacketId <= rudp._fastRetransmitEndPacketId);
				if (!fastRetransmit && (now - packet.TSLastSend) < rudp._rto)
					continue;

				hasDoFastRetransmit = hasDoFastRetransmit | fastRetransmit;

				//---- Get the SACK slots to send with
				SACKSlot slot1 = null, slot2 = null, slot3 = null, slot4 = null;
				rudp._sackWindow.GetSLACKSlots(out slot1, out slot2, out slot3, out slot4);

				// Update the payload for the SACK slots
				UpdatePacketPayload(packet.Payload, slot1, slot2, slot3, slot4);

#if CONSOLE_TRACE
				string acksList = "";
				if (slot1 != null)
					acksList += " [" + slot1.StartPacketId + " <-> " + slot1.EndPacketId + "]";
				if (slot2 != null)
					acksList += " [" + slot2.StartPacketId + " <-> " + slot2.EndPacketId + "]";
				if (slot3 != null)
					acksList += " [" + slot3.StartPacketId + " <-> " + slot3.EndPacketId + "]";
				if (slot4 != null)
					acksList += " [" + slot4.StartPacketId + " <-> " + slot4.EndPacketId + "]";
#endif

				// Send
#if CONSOLE_TRACE
				Trace("Resend packet(" + rudp.Handle + "): " + packet.PacketId + " RTO=" + rudp._rto + "RTT=" + rudp._rtt + " ACKs:" + acksList);
#endif
				if (SocketSendPacket(rudp, packet, packet.Payload, now))
				{
					rudp._controlWindow.OnTimeOut(packet);

					// Update
					packet.Retransmission++;
				}

				if (controlInformation._chargeCheckStopWatch.ElapsedMilliseconds > 0)
					return false;
			}

			//---- Reset fast retransmit
			if (hasDoFastRetransmit)
				rudp.OnEndFastRetransmit();

			return true;
		}

		#endregion

		#region ACKTimer

		private static void ACKTimer(RUDPSocket rudp, long now)
		{
			int acksCount = rudp._sackWindow.ACKCount;
			if (acksCount < 1)
				return;

			//now = HiResTimer.MicroSeconds;
			//---- Delayed ACKs
			if (acksCount < 2)
				if (rudp._lastACKSendTS > -1 && (now - rudp._lastACKSendTS) < DelayACKTime)
					return;

			/*
			bool sendACKDueToTimeOut = (acksCount < 2 && rudp._lastACKSendTS > -1 && (now - rudp._lastACKSendTS) >= DelayACKTime);
			long delayedACKTS = 0;
			if (sendACKDueToTimeOut)
			{
				delayedACKTS = now - rudp._lastACKSendTS;
			}
			*/

			rudp._lastACKSendTS = HiResTimer.MicroSeconds;

			//---- Prepare the SACKs list
			List<SACKSlot> sackSlots = rudp._sackWindow.PrepareACKList();

			for (int index = 0; index < sackSlots.Count; index++)
			{
				//---- Get the SACK slots to send with
				SACKSlot slot1 = null, slot2 = null, slot3 = null, slot4 = null;
				if (sackSlots.Count > 0)
				{
					slot1 = sackSlots[0];
					sackSlots.RemoveAt(0);
				}
				if (sackSlots.Count > 0)
				{
					slot2 = sackSlots[0];
					sackSlots.RemoveAt(0);
				}
				if (sackSlots.Count > 0)
				{
					slot3 = sackSlots[0];
					sackSlots.RemoveAt(0);
				}
				if (sackSlots.Count > 0)
				{
					slot4 = sackSlots[0];
					sackSlots.RemoveAt(0);
				}

#if CONSOLE_TRACE
				if (slot1 != null)
					Trace("Send ACK(" + rudp.Handle + "): " + slot1.StartPacketId + " <-> " + slot1.EndPacketId);
				if (slot2 != null)
					Trace("Send ACK(" + rudp.Handle + "): " + slot2.StartPacketId + " <-> " + slot2.EndPacketId);
				if (slot3 != null)
					Trace("Send ACK(" + rudp.Handle + "): " + slot3.StartPacketId + " <-> " + slot3.EndPacketId);
				if (slot4 != null)
					Trace("Send ACK(" + rudp.Handle + "): " + slot4.StartPacketId + " <-> " + slot4.EndPacketId);
#endif

				byte[] packetPayload = MakePacketPayload(rudp, -1, RUDPPacketChannel.ACK, slot1, slot2, slot3, slot4, null, 0, 0);
				SocketSendACK(rudp, rudp._physical, rudp._remoteEndPoint, packetPayload);
				PayloadManager.Deallocate(RUDPPacketChannel.ACK, packetPayload);
			}
		}

		#endregion

		#region KeepAliveTimer

		private static void KeepAliveTimer(RUDPSocket rudp, long now)
		{
			long lastSendTS = Math.Max(rudp._lastSendTS, rudp._lastACKSendTS);

			//---- Send a keep alive (if possible)
			if (rudp._status == RUDPSocketStatus.Connected &&
				(now - lastSendTS) > RUDPStack.KeepAliveInterval &&
				rudp._controlWindow.CanSend(0))
			{
				PushPacketToSend(rudp, true, RUDPPacketChannel.KeepAlive, new byte[0], 0, 0);
			}
		}

		#endregion

		#region BandwidthTimer

		private static void BandwidthTimer(RUDPSocket rudp, long now)
		{
			//---- Send 2 packets
			if (rudp._status == RUDPSocketStatus.Connected &&
				(now - rudp._lastBandwidthTS) > RUDPStack.BandwidthInterval)
			{
				// Too small packet size can lead to overestimation
				byte[] fullMSSPacket = new byte[rudp._mtu - UDPHeaderLength - RUDPHeaderLength];
				BinaryHelper.WriteLong(now, fullMSSPacket, 0);

				PushPacketToSend(rudp, false, RUDPPacketChannel.Bandwidth01, fullMSSPacket, 0, fullMSSPacket.Length);
				PushPacketToSend(rudp, false, RUDPPacketChannel.Bandwidth02, fullMSSPacket, 0, fullMSSPacket.Length);
				rudp._lastBandwidthTS = now;
			}
		}

		#endregion

		#region LoadBalancingTimer

		/// <summary>
		/// Check the usage of the CPU by this thread. If too much, do a balancing of the sockets.
		/// </summary>
		/// <param name="controlInformation"></param>
		private static void LoadBalancingTimer(ControlThreadInformation controlInformation, long now)
		{
			if (ProcessorsCount < 2 || controlInformation._rudpSockets.Count < 1)
				return;

			if ((now - controlInformation.LastCheckThreadCPUUsageTS) < CheckThreadCPUUsageInterval)
				return;

			lock (_loadBalancingSync)
			{
				//---- Calculate time spent
				long currentUsage = controlInformation.ProcessThread.TotalProcessorTime.Ticks;
				long spentTime = currentUsage - controlInformation.PreviousProcessorTime;
				controlInformation.PreviousProcessorTime = currentUsage;

				// Save it
				Thread.VolatileWrite(ref _controlThreadCPUUsages[controlInformation.ControlThreadId], spentTime);

				controlInformation.LastCheckThreadCPUUsageTS = now;

				long minUsage = Int32.MaxValue;
				long maxUsage = Int32.MinValue;
				int minIndex = -1;
				int maxIndex = -1;

				//---- Check for the min and max usage
				for (int index = 0; index < ProcessorsCount; index++)
				{
					if (_controlThreadCPUUsages[index] > maxUsage)
					{
						maxUsage = _controlThreadCPUUsages[index];
						maxIndex = index;
					}
					if (_controlThreadCPUUsages[index] < minUsage)
					{
						minUsage = _controlThreadCPUUsages[index];
						minIndex = index;
					}
				}

				//---- I use too much CPU compared to others
				float percent = 1.0f / 0.15f; // 15%
				if ((minUsage * percent) > maxUsage && maxIndex == controlInformation.ControlThreadId)
				{
					// Remove the socket from our list
					RUDPSocket balancingSocket = controlInformation._rudpSockets[0];
					controlInformation._rudpSockets.RemoveAt(0);

					// Put the socket in the new list
					_controlThreadInformations[minIndex]._rudpSocketsLock.EnterWriteLock();
					_controlThreadInformations[minIndex]._rudpSockets.Add(balancingSocket);
					_controlThreadInformations[minIndex]._rudpSocketsLock.ExitWriteLock();
					//System.Console.WriteLine("BALANCING");
				}
				//else System.Console.WriteLine("NO BALANCING");
			}
		}

		#endregion

		#region Trace

#if CONSOLE_TRACE_MEMORY
		static private List<string> _traces = new List<string>();
#endif

		[Conditional("CONSOLE_TRACE")]
		internal static void Trace(string text)
		{
#if CONSOLE_TRACE_MEMORY
			lock (_traces)
				_traces.Add(text);
#else
			Console.WriteLine(text);
#endif
		}

		#endregion

		#region HandleException

		internal static void HandleException(Exception exception, params object[] args)
		{
			string paramsText = "";

			foreach (object val in args)
				paramsText += " - " + val.ToString();

			if (paramsText.Length > 0)
				Console.WriteLine("CriticalError :" + exception.Message + '(' + paramsText + ")\n" + exception.StackTrace);

			else
				Console.WriteLine("CriticalError :" + exception.Message + '\n' + exception.StackTrace);
		}

		#endregion

		#region StackFatalException

		private static void StackFatalException(Exception exception)
		{
			HandleException(exception);
		}

		#endregion

		#region SocketErrorToRUDPSocketError

		static private RUDPSocketError SocketErrorToRUDPSocketError(SocketError socketError)
		{
			int error = (int)socketError;
			return (RUDPSocketError)Enum.ToObject(typeof(RUDPSocketError), error);
		}

		#endregion

		#region OnSocketUnhandledError

		/// <summary>
		/// Called when we have an error on a socket.
		/// </summary>
		static internal void OnSocketUnhandledError(PhysicalSocket physical, IPEndPoint remoteEndPoint, SocketError error)
		{
			//---- Get the socket
			RUDPSocket rudp;
			physical._connectedRDUPsLock.EnterReadLock();
			try
			{
				if (!physical._connectedRDUPs.TryGetValue(remoteEndPoint, out rudp))
					return; // Released socket
			}
			finally
			{
				physical._connectedRDUPsLock.ExitReadLock();
			}

			//---- Handle the error
			OnSocketUnhandledError(rudp, SocketErrorToRUDPSocketError(error), null);
		}

		/// <summary>
		/// Called when we have an error on a socket.
		/// </summary>
		static internal void OnSocketUnhandledError(RUDPSocket rudp, RUDPSocketError error, RUDPSendIAsyncResult sendAsyncResult)
		{
			//---- Disconnect the socket
			OnDisconnected(rudp, DisconnectionReason.SocketError);

			//---- Handle the error and forward it to the socket
			if (rudp._status == RUDPSocketStatus.Connecting)
				rudp.OnEndConnect(error);
			else
			{
				// On Send Error
				if (sendAsyncResult != null)
					rudp.OnEndSend(error, sendAsyncResult);

				// ELSE ... HOW TO GET sendAsyncResult when NULL ?????

				// On Receive Error
				RUDPReceiveIAsyncResult receiveAsyncResult = null;
				Interlocked.Exchange<RUDPReceiveIAsyncResult>(ref receiveAsyncResult, rudp._asyncResultReceive);
				if (receiveAsyncResult != null)
				{
					Interlocked.Exchange<RUDPReceiveIAsyncResult>(ref rudp._asyncResultReceive, null);

					rudp.OnEndReceive(error, null, true, receiveAsyncResult);
				}
			}
		}

		#endregion

		#region Affinity / Load balancing helpers

		/// <summary>
		/// Put the affinity to the maximum (Use all the CPUs).
		/// </summary>
		private static void UpdateAffinity()
		{
			Process process = Process.GetCurrentProcess();
			int affinity = (int)Math.Pow(2, ProcessorsCount) - 1;
			process.ProcessorAffinity = new IntPtr(affinity);
		}

		/// <summary>
		/// Set up a control thread to use the right CPU
		/// </summary>
		/// <param name="controlInformation"></param>
		private static void UpdateThreadAffinity(ControlThreadInformation controlInformation)
		{
			int id = AppDomain.GetCurrentThreadId();
			Process process = Process.GetCurrentProcess();

			foreach (ProcessThread processThread in process.Threads)
				if (processThread.Id == id)
				{
					controlInformation.ProcessThread = processThread;
					//processThread.IdealProcessor = processorAffinity;
					processThread.ProcessorAffinity = new IntPtr(controlInformation.ThreadAffinity);
				}
		}

		#endregion

		#region Helpers

		/// <summary>
		/// The purpose of StallThread is to increase efficiency when contention on the SpinWaitLocks
		/// object is detected.
		/// On a multi-processor machine, the thread will call Thread.SpinWait, which causes the thread
		/// to remain in user mode; it will not transition to kernel mode and it will never enter a
		/// wait state. Thread's SpinWait method was added to support hyper-threaded CPUs.
		/// If your code is running on a machine with hyper-threaded CPUs, this method kicks
		/// the other thread so that it starts running (for more information about hyper-threaded CPUs,
		/// see the sidebar "Hyper-Threaded CPUs").
		/// When there is contention on a single-processor machine, I do force the thread to transition
		/// to kernel mode (by calling SwitchToThread) for if I didn't, CPU time would be wasted as the
		/// thread spun without any hope of finding the lock released.
		/// </summary>
		public static void StallThread()
		{
			if (IsSingleCpuMachine)
			{
				// On single-CPU system, spinning does no good
				SwitchToThread();
			}
			else
			{
				// The multi-CPU system might be hyper-threaded, let the other thread run
				Thread.SpinWait(1);
			}
		}

		/// <summary>
		/// Returns the local machine IP address.
		/// Can be an IPv6 or IPv4
		/// </summary>
		static internal IPAddress LocalIPAddress(ProtocolType protocol)
		{
			IPHostEntry localMachineInfo = Dns.Resolve(Dns.GetHostName());

			// Search IPv6 Address (if supported)
			if (System.Net.Sockets.Socket.OSSupportsIPv6 && protocol == ProtocolType.IPv6)

				foreach (IPAddress ipAddress in localMachineInfo.AddressList)

					if (ipAddress.AddressFamily == AddressFamily.InterNetworkV6)

						if (ipAddress.ToString() != "::1")
							return ipAddress;

			// Search for IPv4
			foreach (IPAddress ipAddress in localMachineInfo.AddressList)

				if (ipAddress.AddressFamily == AddressFamily.InterNetwork)

					if (ipAddress.ToString() != "127.0.0.1")
						return ipAddress;

			// IP = "127.0.0.1" ... No IP !!
			return IPAddress.Parse("127.0.0.1");
		}

		#endregion

		#region Packets Pool

		private static LockFreeQueue<RUDPOutgoingPacket> _outgoingPacketsPools = new LockFreeQueue<RUDPOutgoingPacket>();

		internal static RUDPOutgoingPacket NewOutgoingPacket(int packetId, long sequence, byte[] payload, RUDPPacketChannel channel)
		{
			//return new RUDPOutgoingPacket(packetId, sequence, payload, channel);
			RUDPOutgoingPacket packet;

			if (!_outgoingPacketsPools.TryDequeue(out packet))
			{
				for (int index = 0; index < 100; index++)
					_outgoingPacketsPools.Enqueue(new RUDPOutgoingPacket(-1, -1, null, RUDPPacketChannel.Undefined));
				return new RUDPOutgoingPacket(packetId, sequence, payload, channel);
			}

			packet.Reset();
			packet.PacketId = packetId;
			packet.Payload = payload;
			packet.Channel = channel;
			packet.Sequence = sequence;

			return packet;
		}

		internal static void ReleaseOutgoingPacket(RUDPOutgoingPacket packet)
		{
			//return;
			_outgoingPacketsPools.Enqueue(packet);
			PayloadManager.Deallocate(packet.Channel, packet.Payload);
			packet.Payload = null;
		}

		#endregion

		#region Fragments Pool

		private static LockFreeQueue<FragmentInformation> _fragmentsPools = new LockFreeQueue<FragmentInformation>();

		internal static FragmentInformation NewFragmentInformation(RUDPSocket rudpSocket, bool isReliable, byte[] payload, int offset, int size, RUDPSendIAsyncResult asyncResult)
		{
			//return new FragmentInformation(rudpSocket, isReliable, payload, offset, size, asyncResult);

			FragmentInformation fragment;

			if (!_fragmentsPools.TryDequeue(out fragment))
			{
				for (int index = 0; index < 100; index++)
					_fragmentsPools.Enqueue(new FragmentInformation(null, false, null, -1, -1, null));
				return new FragmentInformation(rudpSocket, isReliable, payload, offset, size, asyncResult);
			}

			fragment.rudp = rudpSocket;
			fragment.IsReliable = isReliable;
			fragment.Offset = offset;
			fragment.Size = size;
			fragment.Payload = payload;
			fragment.AsyncResult = asyncResult;

			return fragment;
		}

		internal static void ReleaseFragmentInformation(FragmentInformation fragment)
		{
			//return;
			_fragmentsPools.Enqueue(fragment);
		}

		#endregion

		#region API

		[DllImport("Kernel32", ExactSpelling = true), System.Security.SuppressUnmanagedCodeSecurity()]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern Boolean SwitchToThread();

		#endregion

	}

	#region DisconnectionReason

	public enum DisconnectionReason
	{
		TimeOut,
		ConnectionClosed,
		SocketError
	}

	#endregion

}

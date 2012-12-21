using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;

using Helper.Debug;
using Helper.Threading.Collections;

namespace Helper.CommandProcessor
{
	sealed public class CommandProcessor
	{

		#region Variables

		private string _name;

		private List<Thread> _threads = new List<Thread>();

		private LockFreeQueue<ICommand> _commands = new LockFreeQueue<ICommand>();
		private int _commandsCount = 0;

		volatile private bool _isRunning = false;

		// Internals
		private int _busyThreads = 0;
		private int _maxThreadsCount = Environment.ProcessorCount * 2;
		private int _minThreadsCount = Environment.ProcessorCount;
		private int _uselessThreadTimeout = 15000; // after 15 second inactivity, thread is removed

		// Stats
		private long _totalExecutedCommands = 0;

		#endregion

		#region Constructor

		public CommandProcessor(string name)
		{
			_name = name;
			AppDomain.CurrentDomain.ProcessExit += new EventHandler(CurrentDomain_ProcessExit);
		}

		private void CurrentDomain_ProcessExit(object sender, EventArgs e)
		{
			Stop();
		}

		#endregion

		#region Properties

		/// <summary>
		/// Returns the current number of threads used by this processor.
		/// </summary>
		public int ConcurrentThreads
		{
			get
			{
				lock (_threads)
					return _threads.Count;
			}
		}

		/// <summary>
		/// Returns the total number of executed commands.
		/// </summary>
		public long TotalExecutedCommands
		{
			get
			{
				return _totalExecutedCommands;
			}
		}

		#endregion

		#region Start

		/// <summary>
		/// Start the processor
		/// </summary>
		public void Start()
		{
			_isRunning = true;

			for (int index = 0; index < _minThreadsCount; index++)
				AddThreadToPool();
		}

		#endregion

		#region Stop

		/// <summary>
		/// Stop the processor
		/// </summary>
		public void Stop()
		{

			if (!_isRunning)
				return;

			_isRunning = false;

			// Wake up all the threads
			lock (this)
				Monitor.PulseAll(this);

			// Force all the threads to stop
			lock (_threads)

				foreach (Thread thread in _threads)
					thread.Abort(); // Violently stop the thread.

			_threads.Clear();
		}

		#endregion

		#region AddCommand

		/// <summary>
		/// Add a new command
		/// </summary>
		/// <param name="command">The command to execute</param>
		public void AddCommand(ICommand command)
		{
			_commands.Enqueue(command);

			Interlocked.Increment(ref _commandsCount);

			// Wakeup a processing thread
			lock (this)
				Monitor.Pulse(this);
		}

		/// <summary>
		/// Add a new command
		/// </summary>
		/// <param name="command">The command to execute</param>
		public void AddCommand(ICommand command, ThreadPriority threadPriority)
		{
			// NO MORE USED !!!!!! NO USE OF PRIORITY
			_commands.Enqueue(command);

			Interlocked.Increment(ref _commandsCount);

			// Wakeup a processing thread
			lock (this)
				Monitor.Pulse(this);
		}

		#endregion

		#region Processing

		private void ThreadProc()
		{

			try
			{

				while (_isRunning)
				{

					//---- Wait until we wake up this thread
					if (_commandsCount < 1)
					{
						//-- Wait...
						bool hasTimeOut = false;
						lock (this)
							hasTimeOut = !Monitor.Wait(this, _uselessThreadTimeout);

						//-- The thread pool is NOT heavily used, remove this thread from the pool
						if (hasTimeOut)
						{
							lock (_threads)

								if (_threads.Count > _minThreadsCount)
								{
									_threads.Remove(Thread.CurrentThread);
									return;
								}
							continue;
						}
					}

					//---- Process at least one command, if fast enough can process several commands
					Interlocked.Increment(ref _busyThreads);

					//---- Thread pool is heavily used, add a thread to the pool
					if (_busyThreads == _threads.Count)
						AddThreadToPool();

					ProcessAllCommands();
					Interlocked.Decrement(ref _busyThreads);
				}
			}

			catch (ThreadAbortException)
			{
			}

			catch (Exception exception)
			{
				ExceptionsHandler.Handle(exception);
			}
		}

		/// <summary>
		/// To avoid too much context switching, we try to processing several commands.
		/// But this thread cannot execute during too much time.
		/// </summary>
		private void ProcessAllCommands()
		{
			while (_isRunning)
			{
				//---- Get the next command
				ICommand command;

				if (!_commands.TryDequeue(out command))
					return; // No more commands

				Interlocked.Decrement(ref _commandsCount);

				//---- Execute the command
				try
				{
					command.Execute();
				}

				catch (Exception exception)
				{
					ExceptionsHandler.Handle(exception);
					//Console.WriteLine(exception.Message);
				}

				// Statistics
				Interlocked.Increment(ref _totalExecutedCommands);

				/*

								// Process commands since more than 20 ms, allows context switching
								if (HighResClock.TicksToMs(HighResClock.NowTicks - ticks) > ThreadHelper.ThreadMaxExecutionTime)
								{
									ThreadHelper.StallThread();
									return;
								}*/
			}
		}

		#endregion

		#region AddThreadToPool

		private void AddThreadToPool()
		{
			lock (_threads)
			{

				if (_maxThreadsCount > -1 && _threads.Count >= _maxThreadsCount)
					return;

				Thread newThread = new Thread(new ThreadStart(ThreadProc));
				newThread.Name = _name;
				newThread.IsBackground = true;
				newThread.Priority = ThreadPriority.Normal;
				newThread.Start();

				_threads.Add(newThread);
			}
		}

		#endregion

	}
}

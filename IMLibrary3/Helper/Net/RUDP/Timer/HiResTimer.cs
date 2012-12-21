using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Helper.Net.RUDP
{
	static public class HiResTimer
	{
		static private double _frequency = ((double)Stopwatch.Frequency) / 1000000;

		/// <summary>
		/// Returns the current time in micro seconds
		/// </summary>
		static public long MicroSeconds
		{
			get
			{
				return (long)(Stopwatch.GetTimestamp() / _frequency);
			}
		}
	}
}

using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinTimer.Helper
{
	public class UtcTimestamper
	{
		DateTime startTime;
		bool wasReset = false;

		public UtcTimestamper()
		{
			startTime = DateTime.Now;
		}

		public string GetFormattedTimestamp()
		{
			TimeSpan duration = DateTime.Now.Subtract(startTime);

			return wasReset ? $"Service restarted at {startTime}  \n({duration:c} ago)." : $"Service started at {startTime} \n({duration:c} ago).";
		}

		public void Restart()
		{
			startTime = DateTime.Now;
			wasReset = true;
		}
	}
}

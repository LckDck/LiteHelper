using System;
using System.Timers;
using LiteHelper.Interfaces;

namespace LiteHelper.Droid.InterfaceImplementation
{
	public class TimerInstance : ITimerInstance
	{
		private readonly Timer _tmr;
		public event EventHandler TimerElapsed;

		public TimerInstance ()
		{
			_tmr = new Timer ();
			_tmr.BeginInit ();
			_tmr.Elapsed += TmrOnElapsed;
			_tmr.AutoReset = true;
			_tmr.Interval = TimeSpan.FromSeconds (1).TotalMilliseconds;
			_tmr.EndInit ();
		}

		public void SetInterval (double milliseconds)
		{
			_tmr.BeginInit ();
			_tmr.Interval = milliseconds;
			_tmr.EndInit ();
		}

		public void StopTimer ()
		{
			_tmr.Stop ();
		}

		public void StartTimer ()
		{
			_tmr.Start ();
		}

		public void Dispose ()
		{
			_tmr.Elapsed -= TmrOnElapsed;
			_tmr.Close ();
			_tmr.Dispose ();
		}

		private void TmrOnElapsed (object sender, ElapsedEventArgs e)
		{
			if (TimerElapsed != null) {
				TimerElapsed (null, EventArgs.Empty);
			}
		}
	}
}
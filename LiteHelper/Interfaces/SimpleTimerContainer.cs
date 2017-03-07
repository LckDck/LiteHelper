using System;
using Microsoft.Practices.ServiceLocation;

namespace LiteHelper.Interfaces
{
	public class SimpleTimerContainer
	{
		protected void StartTimer ()
		{
			DestroyTimer ();
			CreateTimer ();

			_timer.StartTimer ();
		}

		protected virtual void OnTimerTick (object sender, EventArgs e)
		{
			DestroyTimer ();
		}


		private void DestroyTimer ()
		{
			if (_timer == null) {
				return;
			}
			_timer.StopTimer ();
			_timer.TimerElapsed -= OnTimerTick;
			_timer.Dispose ();
			_timer = null;
		}



		ITimerInstance _timer;
		private void CreateTimer ()
		{
			_timer = ServiceLocator.Current.GetInstance<ITimerInstance> ();
			_timer.TimerElapsed += OnTimerTick;
		}

	}
}

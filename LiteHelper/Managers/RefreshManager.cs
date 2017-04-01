using System;
namespace LiteHelper.Managers
{
	public class RefreshManager
	{
		public RefreshManager ()
		{
		}

		public void DispatchScrollChanged (int position)
		{
			if (ScrollChanged != null) {
				ScrollChanged.Invoke (null, new PositionEventArgs { Position = position});
			}
		}

		public event EventHandler<PositionEventArgs> ScrollChanged;
	}
}

public class PositionEventArgs : EventArgs { 
	public int Position { get; set;}
}

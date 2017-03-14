using System;
namespace LiteHelper.Managers
{
	public class CodeManager
	{
		internal void UpdateCode (string code)
		{
			if (ChangeCode != null) {
				ChangeCode.Invoke (null, new CodeEventArgs { Code = code });
			}
		}

		internal void DispatchMenuClosing ()
		{
			if (MenuIsClosing != null) {
				MenuIsClosing.Invoke (null, new EventArgs());
			}
		}

		public event EventHandler<CodeEventArgs> ChangeCode;
		public event EventHandler<EventArgs> MenuIsClosing;

}

	public class CodeEventArgs : EventArgs { 
		public string Code { get; set; }
	}
}

using System;
namespace LiteHelper.Managers
{
	public class CodeManager
	{
		internal void UpdateCode (string code)
		{
			ChangeCode.Invoke (null, new CodeEventArgs { Code = code});
		}

		public event EventHandler<CodeEventArgs> ChangeCode;

	}

	public class CodeEventArgs : EventArgs { 
		public string Code { get; set; }
	}
}

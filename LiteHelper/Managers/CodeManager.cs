using System;
namespace LiteHelper.Managers
{
	public class CodeManager
	{
		internal void UpdateCode (string code)
		{
			ResendCode.Invoke (null, new CodeEventArgs { Code = code});
		}

		public event EventHandler<CodeEventArgs> ResendCode;

	}

	public class CodeEventArgs : EventArgs { 
		public string Code { get; set; }
	}
}

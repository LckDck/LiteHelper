using System;
namespace LiteHelper.History
{
	public class CodeInfo
	{
		public string Code {get; set;}
		public string Status {get; set;}
		public double LastEditTime { get; set; }
		public bool Deleted { get; internal set; }
	}
}

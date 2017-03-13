using System;
using System.Linq;

namespace LiteHelper
{
	public static class Utils
	{
		public static string Capitalize (string input)
		{
			if (String.IsNullOrEmpty (input))
				return string.Empty;
			return input.Substring(0, 1).ToUpper () + input.Substring (1);
		}

		public static bool VibroOff { get; set;}

		public static int LastSelection { get; set; }

	}
}
using System;
using System.Collections.Generic;
using System.Linq;

namespace Just.Core
{
	public class Configuration
	{
		public static string JustFileName = "just.";
		public static bool ShowFileNameInComments;
		public static Dictionary<ContentType, bool> Minify = new Dictionary<ContentType, bool>
		{
			{ ContentType.JavaScripts, false },
			{ ContentType.Stylesheets, true },
		};
	}
}

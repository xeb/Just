using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Just.Core.Minifiers
{
	public class CssMinifier
	{
		public static string Compress(string script)
		{
			// some basic minifying
			script = Regex.Replace(script, @"\/\*(.*?)\*\/", String.Empty, RegexOptions.Singleline);

			var replacements = new[]
			{
				new { search = "\t", replace = " " },
				new { search = "\r", replace = String.Empty },
				new { search = "\n", replace = String.Empty },
						
			};

			foreach (var replacement in replacements)
			{
				script = script.Replace(replacement.search, replacement.replace);
			}

			for (int i = 0; i < 10; i++) // run through this 10 times.  Replacements tend to cause other spaces to appear.
			{
				foreach (var trim in new[] { ":", ";", "}", "{", ",", ">", })
				{
					script = script.Replace(String.Format(" {0}", trim), trim);
					script = script.Replace(String.Format("{0} ", trim), trim);
				}
			}

			while (script.Contains("  "))
			{
				script = script.Replace("  ", " ");
			}

			return script;
		}
	}
}
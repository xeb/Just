using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Just.Web
{
	
	/// <summary>
	/// Just == JavaScript Unified Source Transposer
	/// </summary>
	public class JustHandler : IHttpHandler 
	{
		private const string JUST_FILE = "just.js";
		private const string DIRECTORY = "~/content/javascripts/";

		public void ProcessRequest(HttpContext context)
		{
			var dir = new DirectoryInfo(context.Server.MapPath(DIRECTORY));

			// If we are requesting the "just.js" file, wrap all JavaScript files
			if(context.Request.Url.PathAndQuery.ToLower().Contains(JUST_FILE))
			{
				// Get Files
				var javaScript = GetJavaScripts(dir, ParseOrderList(context.Request));
				context.Response.Write(javaScript);
			}
			else
			{
				// Return requested file
				context.Response.Write(File.ReadAllText(context.Server.MapPath(context.Request.Url.LocalPath)));
			}
		}

		public bool IsReusable
		{
			get { return true; }
		}

		#region -- Private Methods --

		private static List<string> ParseOrderList(HttpRequest request)
		{
			var list = new List<string>();

			if (String.IsNullOrEmpty(request.QueryString["d"])) return list;

			list.AddRange(request.QueryString["d"].Split(new[]{","}, StringSplitOptions.RemoveEmptyEntries));
			list.ForEach(s => s.Replace(",",""));

			return list;
		}

		private static string GetJavaScripts(DirectoryInfo directory, IEnumerable<string> scriptOrderList)
		{
			var sb = new StringBuilder();
			var allFiles = directory.GetFiles("*.js").ToList();

			foreach(var fileName in scriptOrderList)
			{
				string value = fileName;

				if(allFiles.Count(f => f.Name.StartsWith(value)) > 1)
				{
					throw new Exception(String.Format("Just cannot process.  More than 1 file found that starts with '{0}'.  Please be more specific in your dependency list.", fileName));
				}

				var file = allFiles.SingleOrDefault(f => f.Name.StartsWith(value));
				if (file == null) continue;

				sb.AppendLine(File.ReadAllText(file.FullName));
				allFiles.Remove(file);
			}

			foreach(var file in allFiles.OrderBy(f => f.Name))
			{
				sb.AppendLine(File.ReadAllText(file.FullName));
			}

			return sb.ToString();
		}

		#endregion
	}
	}
}

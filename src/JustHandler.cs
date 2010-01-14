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
				var javaScript = GetJavaScripts(dir);
				context.Response.Write(javaScript);
			}
		}

		public bool IsReusable
		{
			get { return true; }
		}

		#region -- Private Methods --

		private static string GetJavaScripts(DirectoryInfo directory)
		{
			var sb = new StringBuilder();

			foreach(var file in directory.GetFiles("*.js"))
			{
				sb.AppendLine(File.ReadAllText(file.FullName));
			}

			return sb.ToString();
		}

		#endregion
	}
}

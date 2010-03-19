using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using Just.Core.Minifiers;

namespace Just.Core
{
	/// <summary>
	/// Just == JavaScript Unified Source Transposer
	/// </summary>
	public class JustHandler : IHttpHandler 
	{
		public void ProcessRequest(HttpContext context)
		{
			// If we are requesting a "just.*" file, wrap all  files
			if(context.Request.Url.PathAndQuery.ToLower().Contains(Configuration.JustFileName) && 
				!String.IsNullOrEmpty(context.Request.Url.Query))
			{
				new JustRequest(context, ContentType.JavaScripts, GoogleClosureAPI.Compress);
				new JustRequest(context, ContentType.Stylesheets, CssMinifier.Compress);
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
	}
}
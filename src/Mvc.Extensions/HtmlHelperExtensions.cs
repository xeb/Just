﻿using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace Just.Core.Mvc.Extensions
{
	public static class HtmlHelperExtensions
	{
		/// <summary>
		/// Maps a Content Path for the given HtmlHelper's context
		/// </summary>
		/// <param name="helper"></param>
		/// <param name="url"></param>
		/// <returns></returns>
		public static string MapContentPath(this HtmlHelper helper, string url)
		{
			return new UrlHelper(helper.ViewContext.RequestContext).Content(url);
		}

		/// <summary>
		/// Takes an array of script names and outputs them all into 1 call to just.js 
		/// </summary>
		/// <param name="htmlHelper"></param>
		/// <param name="scriptNames"></param>
		/// <returns></returns>
		public static string IncludeJustJs(this HtmlHelper htmlHelper, params string[] scriptNames)
		{
			var root = ContentManager.GetContentRoot(ContentType.JavaScripts);
			var format = "<script type=\"text/javascript\" src=\"" + MapContentPath(htmlHelper, root) + "just.js?d=";

			scriptNames.ToList().ForEach(s => format = String.Format("{0}{1},", format, s));

			format += "&t=" + GetTimestamp(new DirectoryInfo(htmlHelper.ViewContext.HttpContext.Server.MapPath(root)));

			return String.Concat(format, "\"></script>");
		}

		/// <summary>
		/// Takes an array of stylesheet names and outputs them all into 1 call to just.css 
		/// </summary>
		/// <param name="htmlHelper"></param>
		/// <param name="styleSheets"></param>
		/// <returns></returns>
		public static string IncludeJustCss(this HtmlHelper htmlHelper, params string[] styleSheets)
		{
			var root = ContentManager.GetContentRoot(ContentType.Stylesheets);
			var format = "<link media=\"screen\" rel=\"stylesheet\" type=\"text/css\" href=\"" + MapContentPath(htmlHelper, root) + "just.css?d=";

			styleSheets.ToList().ForEach(s => format = String.Format("{0}{1},", format, s));

			format += "&t=" + GetTimestamp(new DirectoryInfo(htmlHelper.ViewContext.HttpContext.Server.MapPath(root)));

			return String.Concat(format, "\" />");
		}

		private static string GetTimestamp(DirectoryInfo directory)
		{
			return directory.GetFiles().OrderBy(f => f.LastWriteTime).FirstOrDefault().LastWriteTime.ToString("yyyyMMddHHmmssff");
		}
	}
}
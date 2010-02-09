using System;
using System.Collections.Generic;
using System.Linq;

namespace Just.Core
{
	public class ContentManager
	{
		public static Dictionary<ContentType, Dictionary<string,string>> ContentData = new Dictionary<ContentType, Dictionary<string,string>>
		{
			{ 
				ContentType.JavaScripts, 
				new Dictionary<string,string>
				{
					{ "ContentRoot", "~/content/javascripts" },
					{ "Extension", "js" },
					{ "MIMEType", "text/javascripts" },
				}
			},
			{ 
				ContentType.Stylesheets, 
				new Dictionary<string,string>
				{
					{ "ContentRoot", "~/content/stylesheets" },
					{ "Extension", "css" },
					{ "MIMEType", "text/css" },
				}
			},
		};

		public static string GetContentRoot(ContentType type)
		{
			return ContentData.SingleOrDefault(d => d.Key == type).Value["ContentRoot"];
		}

		public static string GetExtension(ContentType type)
		{
			return ContentData.SingleOrDefault(d => d.Key == type).Value["Extension"];
		}

		public static string GetMimeType(ContentType type)
		{
			return ContentData.SingleOrDefault(d => d.Key == type).Value["MIMEType"];
		}
	}

	public enum ContentType
	{
		JavaScripts, Stylesheets
	}
}

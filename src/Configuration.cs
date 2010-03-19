using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Just.Core.Extensions;

namespace Just.Core
{
	public class Configuration
	{
		public static string JustFileName = "just.";
		public static bool ShowFileNameInComments;
		public static Dictionary<ContentType, bool> Minify = new Dictionary<ContentType, bool>
		{
			{ ContentType.JavaScripts, false },
			{ ContentType.Stylesheets, false },
		};

		public static string GetHandlerRoot(ContentType type)
		{
			return GetConfigSetting(String.Format("HandlerRoot.{0}", type), String.Format("~/content/{0}", type)).ToLower();
		}

		public static string GetHandlerExtension(ContentType type)
		{
			return GetConfigSetting(String.Format("HandlerExtension.{0}", type), String.Format("{0}", ContentManager.GetExtension(type))).ToLower();
		}

		internal static T GetConfigSetting<T>(string settingName, T defaultValue)
		{
			var setting = ConfigurationManager.AppSettings[String.Format("Just.{0}", settingName)];
			return String.IsNullOrEmpty(setting) ? defaultValue : setting.ParseValue(defaultValue);
		}
	}
}

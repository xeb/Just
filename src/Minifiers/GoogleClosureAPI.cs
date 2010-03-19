using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace Just.Core.Minifiers
{
	public class GoogleClosureAPI
	{
		public static string Compress(string script)
		{
			using (var response = Post("http://closure-compiler.appspot.com/compile", new NameValueCollection
			{
				{ "js_code", script },
				{ "compilation_level", "SIMPLE_OPTIMIZATIONS" },
				{ "output_format", "text" },
				{ "output_info", "complied_code" },
			}))
			{
				using (StreamReader reader = new StreamReader(response.GetResponseStream()))
				{
					return reader.ReadToEnd();
				}
			}
		}


		public static WebResponse Post(string url, NameValueCollection variables)
		{
			WebRequest req = WebRequest.Create(url);
			req.Method = "POST";
			req.ContentType = "application/x-www-form-urlencoded";

			var postData = new StringBuilder();

			foreach (var variable in variables.AllKeys)
			{
				postData.Append(variable).Append("=").Append(HttpUtility.UrlEncode(variables[variable])).Append("&");
			}

			byte[] bytes = Encoding.GetEncoding(1252).GetBytes(postData.ToString());
			req.ContentLength = bytes.Length;

			Stream outputStream = req.GetRequestStream();
			outputStream.Write(bytes, 0, bytes.Length);
			outputStream.Close();

			return req.GetResponse();
		}
	}
}

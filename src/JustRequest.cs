using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Just.Core
{
	public class JustRequest
	{
		public JustRequest(HttpContext context, ContentType contentType, Func<string,string> minifier)
		{
			Context = context;
			Extension = ContentManager.GetExtension(contentType);
			DirectoryName = ContentManager.GetContentRoot(contentType);
			MimeType = ContentManager.GetMimeType(contentType);
			Minifier = minifier;
			ContentType = contentType;
		}

		public HttpContext Context { get; set; }
		public ContentType ContentType { get; set; }
		public string Extension { get; set; }
		public string DirectoryName { get; set; }
		public string MimeType { get; set; }
		public Func<string, string> Minifier { get; set; }

		public void Process()
		{
			if (!Context.Request.Url.LocalPath.EndsWith(String.Concat(".", Extension), StringComparison.OrdinalIgnoreCase))
			{
				return;
			}

			var script = GetFileData(new DirectoryInfo(Context.Server.MapPath(DirectoryName)), ParseOrderList(Context.Request), ContentType);

			Context.Response.ContentType = MimeType;
			Context.Response.Write(script);
			Context.Response.End();
		}

		#region -- Private Methods --

		private static List<String> ParseOrderList(HttpRequest request)
		{
			var list = new List<string>();

			if (String.IsNullOrEmpty(request.QueryString["d"])) return list;

			list.AddRange(request.QueryString["d"].Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries));
			list.ForEach(s => s.Replace(",", ""));

			return list;
		}

		private string GetFileData(DirectoryInfo directory, IEnumerable<string> scriptOrderList, ContentType type)
		{
			var sb = new StringBuilder();
			var allFiles = directory.GetFiles("*." + ContentManager.GetExtension(type)).ToList();

			foreach (var fileName in scriptOrderList)
			{
				string value = fileName;

				if (value.Contains("*"))
				{
					var filesStartWith = allFiles.Where(f => f.Name.ToLower().StartsWith(value.Split('*')[0].ToLower())).ToList();
					if (filesStartWith.Count > 0)
					{
						foreach (var file in filesStartWith)
						{
							var text = File.ReadAllText(file.FullName);

							if (Minifier != null && Configuration.Minify[type])
							{
								text = Minifier(text);
							}

							if (Configuration.ShowFileNameInComments)
							{
								sb.AppendLine(String.Format("/*{0}*/", file.FullName));
							}

							sb.AppendLine(text);
							allFiles.Remove(file);
						}
					}
				}
				else
				{
					var file = allFiles.SingleOrDefault(f => f.Name.Equals(String.Concat(value, ".", type), StringComparison.OrdinalIgnoreCase));
					if (file == null)
					{
						continue;
					}

					if (Configuration.ShowFileNameInComments)
					{
						sb.AppendLine(String.Format("/*{0}*/", file.FullName));
					}

					sb.AppendLine(File.ReadAllText(file.FullName));
					allFiles.Remove(file);
				}
			}

			return sb.ToString();
		}

		#endregion
	}
}
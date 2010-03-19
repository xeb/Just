using System;
using System.Collections.Generic;
using System.Linq;

namespace Just.Core.Extensions
{
	public static class TypeExtensions
	{
		/// <summary>
		/// Get's the base type of the Nullable
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static Type GetNonNullableType(this Type type)
		{
			if (type != null)
			{
				if (type.FullName.StartsWith("System.Nullable`1[["))
				{
					string typeName = type.FullName.Replace("System.Nullable`1[[", "").Replace("]]", "");
					return Type.GetType(typeName);
				}
			}

			return type;
		}

		/// <summary>
		/// Returns a list of the fields for a type -- particularly useful for enums
		/// </summary>
		/// <example>
		/// typeof(Enum).ToFieldList()
		/// </example>
		/// <param name="type"></param>
		/// <returns></returns>
		public static List<String> ToFieldList(this Type type)
		{
			var list = new List<String>();

			foreach(var field in type.GetFields())
			{
				list.Add(field.Name);
			}

			return list;
		}

		/// <summary>
		/// Determines if the type implements the interfaceType
		/// </summary>
		/// <param name="type"></param>
		/// <param name="interfaceType"></param>
		/// <see cref="http://www.hanselman.com/blog/DoesATypeImplementAnInterface.aspx"/>
		/// <returns></returns>
		public static Boolean IsImplementationOf(this Type type, Type interfaceType)
		{
			return type.GetInterface(interfaceType.FullName) != null;
		}
	}
}
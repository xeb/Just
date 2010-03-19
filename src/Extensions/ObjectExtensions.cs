using System;
using System.Linq;

namespace Just.Core.Extensions
{
	public static class ObjectExtensions
	{
		/// <summary>
		/// Will perform the given <see cref="Func{T}" /> on the instance if instance is not null
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="TK"></typeparam>
		/// <param name="instance"></param>
		/// <param name="someAction"></param>
		/// <returns></returns>
		public static T SafeCall<T, TK>(this TK instance, Func<TK,T> someAction) where TK : class
		{
			return SafeCall(instance, someAction, default(T));
		}

		/// <summary>
		/// Will perform the given <see cref="Func{T}" /> on the instance if instance is not null.  Returns <see cref="T"/> defaultValue if instance is NULL.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="TK"></typeparam>
		/// <param name="instance"></param>
		/// <param name="someAction"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static T SafeCall<T,TK>(this TK instance, Func<TK,T> someAction, T defaultValue) where TK : class
		{
			return instance == null ? defaultValue : someAction(instance);
		}

		/// <summary>
		/// Parses the given value into <see cref="Type"/> of type
		/// </summary>
		/// <param name="value"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		public static object ParseValue(this object value, Type type)
		{
			try
			{
				if (type.GetNonNullableType().IsEnum)
				{
					return Enum.Parse(type.GetNonNullableType(), Convert.ToString(value));
				}

				if (type.IsEnum)
				{
					return Enum.Parse(type, Convert.ToString(value));
				}

				if(type.GetNonNullableType() != type)
				{
					if (value == null || String.IsNullOrEmpty(Convert.ToString(value)))
					{
						return null;
					}

					return Convert.ChangeType(value, type.GetNonNullableType());
				}

				return Convert.ChangeType(value, type);
			}
				// ReSharper disable EmptyGeneralCatchClause
			catch
				// ReSharper restore EmptyGeneralCatchClause
			{

			}

			return value;
		}

		/// <summary>
		/// Will attempt to parse the value of an object to T
		/// </summary>
		/// <example>
		/// bool debug = ConfigurationManager.AppSetting["debug"].ParseValue(true);
		/// int timeout = Request.QueryString["timeout"].ParseValue(60);
		/// DateRange? selection = Session["selection"].ParseValue(DateRange.Days);
		/// </example>
		/// <typeparam name="T"></typeparam>
		/// <param name="value"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static T ParseValue<T>(this object value, T defaultValue)
		{
			try
			{
				if (typeof(T).GetNonNullableType().IsEnum)
				{
					return (T)Enum.Parse(typeof(T).GetNonNullableType(), Convert.ToString(value));
				}

				if (typeof(T).IsEnum)
				{
					return (T)Enum.Parse(typeof(T), Convert.ToString(value));
				}

				return (T)Convert.ChangeType(value, typeof(T));
			}
				// ReSharper disable EmptyGeneralCatchClause
			catch
				// ReSharper restore EmptyGeneralCatchClause
			{

			}

			return defaultValue;
		}
	}
}
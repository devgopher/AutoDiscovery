/*
 * Пользователь: Igor.Evdokimov
 * Дата: 18.09.2015
 * Время: 13:47
 */
using System;

namespace AutoDiscovery.Tests.Utils
{
	/// <summary>
	/// Popular regular expressions
	/// </summary>
	public static class PopularRegex
	{
		public static string IpRegex {
			get {
				return @"\b(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\."+
					@"(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\."+
					@"(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\."+
					@"(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b";
			}
		}
	}
}

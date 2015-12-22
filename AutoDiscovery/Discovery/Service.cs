/*
 * Пользователь: Igor.Evdokimov
 * Дата: 22.12.2015
 * Время: 12:06
 */
using System;
using ManagedUPnP;
using ManagedUPnP.Components;

namespace AutoDiscovery.Discovery
{
	/// <summary>
	/// Description of Service.
	/// </summary>
	public static class ADService 
	{
		public static Service Instance {
			get {
				if ( serv == null ) {
				//serv = 
				}
				return serv;
			}
			private set {}
		}

		private static Service serv = null;

	}
}

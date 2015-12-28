/*
 * User: Igor
 * Date: 12/27/2015
 * Time: 18:22
 */
using System;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;

namespace AutoDiscovery.Utils
{
	/// <summary>
	/// Different utilities
	/// </summary>
	public class Utils
	{
		public Utils()
		{
		}
		
		public static string GetLocalIP()
		{
			var host = Dns.GetHostEntry(Dns.GetHostName());
			foreach (var ip in host.AddressList)
			{
				if (ip.AddressFamily == AddressFamily.InterNetwork)
				{
					return ip.ToString();
				}
			}
			throw new Exception("Local IP address was not found!");
		}
		
	}
}

/*
 * User: Igor
 * Date: 29.12.2015
 * Time: 23:51
 */
using System;
using System.Net;
using System.Net.Sockets;

namespace AutoDiscovery
{
	/// <summary>
	/// Several common variables
	/// </summary>
	public static class CommonEnvironment
	{
		public static readonly UdpClient udp_client;
		public static readonly int BROADCAST_PORT = 45200;
		public static readonly IPAddress bcast_address = IPAddress.Broadcast;
		
		static CommonEnvironment() {
			udp_client = new UdpClient(
				BROADCAST_PORT, 
				AddressFamily.InterNetwork );
			udp_client.EnableBroadcast = true;
		}
	}
}

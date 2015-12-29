/*
 * User: Igor
 * Date: 29.12.2015
 * Time: 23:51
 */
using System;
using AutoDiscovery;
using System.Net;
using System.Text;
using System.Net.Sockets;
using System.Collections.Generic;

namespace AutoDiscovery
{
	public static class CommonEnvironment
	{
		public static UdpClient udp_client;
		public static int BROADCAST_PORT = 45100;
		
		static CommonEnvironment() {
			udp_client = new UdpClient(
				BROADCAST_PORT, 
				AddressFamily.InterNetwork );
			udp_client.EnableBroadcast = true;
		}
	}
}

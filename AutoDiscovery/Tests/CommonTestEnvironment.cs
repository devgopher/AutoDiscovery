﻿/*
 * User: Igor
 * Date: 30.12.2015
 * Time: 0:14
 */
using System;

namespace AutoDiscovery.Tests
{
	/// <summary>
	/// Description of CommonTestEnvironment.
	/// </summary>
	public static class CommonTestEnvironment
	{
		public static void Disconnect() {
			if (CommonEnvironment.udp_client.Client.Connected)
				CommonEnvironment.udp_client.Close();
		
		}
	}
}
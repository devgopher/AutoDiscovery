/*
 * User: Igor
 * Date: 29.12.2015
 * Time: 23:39
 */
using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using NUnit.Framework;

namespace AutoDiscovery.Tests
{
	[TestFixture]
	public class ListenerTest
	{
		Listener listener = null;
		[Test]
		public void StartStopTest()
		{
			CommonTestEnvironment.Disconnect();
			
			bool message_registered = false;
			
			listener = new Listener( CommonEnvironment.udp_client );
			
			listener.Start(
				( x ) => { 
					message_registered = true;
				}
			);
			
			System.Threading.Thread.Sleep(5000);
			
			listener.Stop();
			CommonTestEnvironment.Disconnect();
		}

	}
}

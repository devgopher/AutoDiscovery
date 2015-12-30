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
using NUnit.Framework.Constraints;

namespace AutoDiscovery.Tests
{
	[TestFixture]
	public class ListenerTest
	{
		Listener listener = null;
		[Test]
		public void StartStopTest()
		{
			CommonTestEnvironment.ResetConnection();
			
			bool message_registered = false;
			
			// Starting a listener
			listener = new Listener( CommonEnvironment.udp_client );
			
			listener.Start(
				( x ) => {
					message_registered = true;
				}
			);

			// 1.5 sec delay to start
			System.Threading.Thread.Sleep(1500); 
			
			// Checking Start and Listening variables..
			Assert.True( listener.Listening );
			Assert.True( listener.Started );
			
			// stopping a listener
			listener.Stop();
			
			// Checking Start and Listening variables..
			Assert.False( listener.Listening );
			Assert.False( listener.Started );
			
			CommonTestEnvironment.ResetConnection();
		}

	}
}

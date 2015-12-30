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
			CommonTestEnvironment.Disconnect();
			
			bool message_registered = false;
			
			listener = new Listener( CommonEnvironment.udp_client );
			
			listener.Start(
				( x ) => {
					message_registered = true;
				}
			);
			
			Assert.True( listener.Listening );
			Assert.True( listener.Started );
			
			System.Threading.Thread.Sleep(5000);
			
			listener.Stop();
			
			Assert.False( listener.Listening );
			Assert.False( listener.Started );
			
			CommonTestEnvironment.Disconnect();
		}

	}
}

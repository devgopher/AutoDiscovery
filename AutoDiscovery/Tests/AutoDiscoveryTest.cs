/*
 * User: Igor
 * Date: 29.12.2015
 * Time: 23:33
 */
using System;
using NUnit.Framework;

namespace AutoDiscovery.Tests
{
	[TestFixture]
	public class AutoDiscoveryTest
	{
		AutoDiscovery ad = new AutoDiscovery( CommonEnvironment.udp_client );
		[Test]
		public void StartStopNodeTest()
		{
			CommonTestEnvironment.Disconnect();
			
			ad.StartNode();
			System.Threading.Thread.Sleep(300);
			Assert.True( ad.Hosts.Count > 0 );
			Assert.True( ad.Msgs.Count > 0 );
			

			System.Threading.Thread.Sleep(3000);
			ad.StopNode();
			
			Assert.True( ad.Hosts.Count == 0 );
			Assert.True( ad.Msgs.Count == 0 );
			
			CommonTestEnvironment.Disconnect();
		}
	}
}

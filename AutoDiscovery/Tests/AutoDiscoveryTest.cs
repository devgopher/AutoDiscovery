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

		/// <summary>
		/// A general AutoDiscovery start/stop test
		/// </summary>
		[Test]
		public void StartStopNodeTest()
		{
			CommonTestEnvironment.ResetConnection();
			// Starting a node
			ad.StartNode();
			System.Threading.Thread.Sleep(300);
			
			// Did we received something?
			// If we've no other nodes, we must "see"
			// our host and our messages. If no - 
			// something gone wrong!		
			Assert.True( ad.Hosts.Count > 0 );
			Assert.True( ad.Msgs.Count > 0 );
			
			// Waiting a bit...
			System.Threading.Thread.Sleep(3000);
			
			// Stopping a node
			ad.StopNode();
			
			// When we stopped a node, Hosts and Messages 
			// must be clean. If no - something happened!
			Assert.True( ad.Hosts.Count == 0 );
			Assert.True( ad.Msgs.Count == 0 );
			
			CommonTestEnvironment.ResetConnection();
		}
	}
}

/*
 * Пользователь: igor.evdokimov
 * Дата: 30.12.2015
 * Время: 16:24
 */
using System;
using NUnit.Framework;

namespace AutoDiscovery.Tests
{
	[TestFixture]
	public class NodeStateNotificatorTest
	{
		/// <summary>
		/// A generaL start/stop test
		/// </summary>
		[Test]
		public void StartStopTest()
		{
			// Starting a notificator
			var nsn = new NodeStateNotificator( CommonEnvironment.udp_client,
			                                   CommonEnvironment.bcast_address );
			nsn.Start();
			
			// check if started
			Assert.True( nsn.Started );
			
			nsn.Stop();
			// check if stopped
			Assert.False( nsn.Started );
		}
		
		/// <summary>
		/// A message sending test
		/// </summary>
		[Test]
		public void SendMsgTest() {
			// starting a notificator...
			var nsn = new NodeStateNotificator( CommonEnvironment.udp_client,
			                                   CommonEnvironment.bcast_address );
			nsn.Start();
			
			// sending a message
			int sent_bytes_count = nsn.SendMsg( "TEST" );
			
			// did we send anything?
			Assert.True( sent_bytes_count > 0 );
			
			// stopping a notificator
			nsn.Stop();			
		}
	}
}

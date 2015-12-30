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
		[Test]
		public void StartStopTest()
		{
			var nsn = new NodeStateNotificator( CommonEnvironment.udp_client,
			                                   CommonEnvironment.bcast_address );
			nsn.Start();
			Assert.True( nsn.Started );
			
			nsn.Stop();
			Assert.False( nsn.Started );
		}
		
		[Test]
		public void SendMsgTest() {
			var nsn = new NodeStateNotificator( CommonEnvironment.udp_client,
			                                   CommonEnvironment.bcast_address );
			nsn.Start();
			
			int sent_bytes_count = nsn.SendMsg( "TEST" );
			
			Assert.True( sent_bytes_count > 0 );
			
			nsn.Stop();
			
		}
	}
}

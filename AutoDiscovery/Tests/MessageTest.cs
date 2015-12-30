/*
 * Пользователь: igor.evdokimov
 * Дата: 30.12.2015
 * Время: 15:42
 */
using System;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace AutoDiscovery.Tests
{
	[TestFixture]
	public class MessageTest
	{
		[Test]
		[TestCase( "192.168.1.102:AUTODISCOVERY_ONAIR" )]
		[TestCase( "127.0.0.13:AUTODISCOVERY_ONAIR" )]
		public void GetTest( string input )
		{
			Regex common_rgx = new Regex(@"(.*)\:(.*)" );
			
			var ip_addr_part = common_rgx.Match(input).Groups[1].Value;
			var app_state_part = common_rgx.Match(input).Groups[2].Value;
			
			var msg = Message.Get( input );
			
			Assert.True(msg.FromIp == ip_addr_part);
			Assert.True(msg.Contains == app_state_part);
		}
	}
}

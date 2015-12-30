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
		/// <summary>
		/// Testing a Message.Get method
		/// </summary>
		/// <param name="input">A message in simple text form</param>
		[Test]
		[TestCase( "192.168.1.102:AUTODISCOVERY_ONAIR" )]
		[TestCase( "127.0.0.13:AUTODISCOVERY_ONAIR" )]
		public void GetTest( string input )
		{
			Regex common_rgx = new Regex(@"(.*)\:(.*)" );
			
			// parsing our message
			var ip_addr_part = common_rgx.Match(input).Groups[1].Value;
			var app_state_part = common_rgx.Match(input).Groups[2].Value;
			
			// Getting a Message object
			var msg = Message.Get( input );
			
			// Checking Ip address and reported State
			Assert.True(msg.FromIp == ip_addr_part);
			Assert.True(msg.Contains == app_state_part);
		}
	}
}

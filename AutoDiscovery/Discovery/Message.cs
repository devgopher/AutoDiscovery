/*
 * User: Igor
 * Date: 12/27/2015
 * Time: 18:33
 */
using System;

namespace AutoDiscovery
{
	/// <summary>
	/// Description of Message.
	/// </summary>
	public struct Message
	{
		public DateTime Received {get; private set;}
		public string FromIp {get; private set;}
		public string Contains {get; private set;}
		
		static public Message Get( string original ) {
			var input = original.Split(':');
			if ( input.Length == 2 ) {
				var ret = new Message();
				ret.Received = DateTime.Now;
				ret.FromIp = input[0];
				ret.Contains = input[1];
				return ret;
			} else				
				throw new Exception( "Incorrect message format!" );
		}
	}
}

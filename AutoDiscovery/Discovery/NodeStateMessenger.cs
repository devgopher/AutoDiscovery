/*
 * User: Igor
 * Date: 12/27/2015
 * Time: 14:39
 */
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace AutoDiscovery
{
	/// <summary>
	/// Description of NodeStateMessenger.
	/// </summary>
	public class NodeStateMessenger
	{
		// multicast messages
		private const string app_name = "AUTODISCOVERY";
		public const string start_bc_msg = app_name+"_START";
		public const string onair_bc_msg = app_name+"_ONAIR";
		public const string stop_bc_msg = app_name+"_STOP";
		private readonly UdpClient udp_client = null;
		private readonly Timer msg_sending_timer = null;
		private IPAddress broadcast_ip;
		private String local_ip;
		
		public void SendOnAir( object state ) {
			SendMsg( onair_bc_msg );
		}
		
		public int? SendMsg( string msg ) {
			var onair_bytes =
				AutoDiscovery.ascii_encoding.GetBytes(
					local_ip.ToString()+":"+msg);
			
			var multicast_ep = new IPEndPoint(
				broadcast_ip, 
				AutoDiscovery.BROADCAST_PORT );
			
			return (int?)(udp_client.Send( onair_bytes,
			                              onair_bytes.Length,
			                              multicast_ep ));
		}
		
		public NodeStateMessenger( UdpClient _client,
		                          IPAddress mult_ip )
		{
			if ( mult_ip == null )
				return;
			broadcast_ip = mult_ip;
			udp_client = _client;
			object state = null;
			local_ip = Utils.Utils.GetLocalIP();
			msg_sending_timer = new Timer( SendOnAir, state, -1, 200 );
		}
		
		public void Start() {
			if ( started == true )
				return;
			
			msg_sending_timer.Change( 0, 200 );
			
			SendMsg( start_bc_msg );
			started = true;
		}

		public void Stop() {
			if ( started == false )
				return;

			msg_sending_timer.Change( -1, 200 );
			started = false;
			
			SendMsg( stop_bc_msg );
		}
		
		private bool started = false;
	}
}

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
	/// A component for sending notifications about
	/// state of our node to other network nodes
	/// </summary>
	public class NodeStateNotificator
	{
		private const string app_name = "AUTODISCOVERY";
		
		#region Messages
		public const string start_bc_msg = app_name+"_START";
		public const string onair_bc_msg = app_name+"_ONAIR";
		public const string stop_bc_msg = app_name+"_STOP";
		#endregion
		
		private readonly UdpClient udp_client = null;
		private readonly Timer msg_sending_timer = null;
		private IPAddress broadcast_ip;
		private String local_ip;
		private bool started = false;
		
		public NodeStateNotificator( UdpClient _client,
		                            IPAddress mult_ip )
		{
			if ( mult_ip == null )
				return;
			broadcast_ip = mult_ip;
			udp_client = _client;
			object state = null;
			local_ip = Utils.Utils.GetLocalIP();
			msg_sending_timer = new Timer( SendOnAir, 
			                              state, 
			                              -1, 
			                              200 );
		}
		
		/// <summary>
		/// Sends "OnAir" notifications
		/// </summary>
		/// <param name="state"></param>
		public void SendOnAir( object state ) {
			SendMsg( onair_bc_msg );
		}
		
		/// <summary>
		/// Forming a full message for transmitting into network
		/// </summary>
		/// <param name="msg_content"></param>
		/// <returns></returns>
		private string FormFullMessage( string msg_content ) {
			return local_ip.ToString()+":"+msg_content;
		}
		
		/// <summary>
		/// Sends a notification
		/// </summary>
		/// <param name="msg_content">Notification contents</param>
		/// <returns></returns>
		public int? SendMsg( string msg_content ) {
			var onair_bytes =
				AutoDiscovery.ascii_encoding
				.GetBytes(FormFullMessage(msg_content));
			
			var multicast_ep = new IPEndPoint(
				broadcast_ip,
				AutoDiscovery.BROADCAST_PORT );
			
			return (int?)(udp_client.Send( onair_bytes,
			                              onair_bytes.Length,
			                              multicast_ep ));
		}
		
		/// <summary>
		/// Start notifying process
		/// </summary>
		public void Start() {
			if ( started == true )
				return;
			// starting timer
			msg_sending_timer.Change( 0, 200 );
			// sending "START" message
			SendMsg( start_bc_msg );
			started = true;
		}

		/// <summary>
		/// Stop notifying process
		/// </summary>
		public void Stop() {
			if ( started == false )
				return;
			// stopping timer
			msg_sending_timer.Change( -1, 200 );
			started = false;
			// sending "STOP" message			
			SendMsg( stop_bc_msg );
		}
	}
}

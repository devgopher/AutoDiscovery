/*
 * User: Igor
 * Date: 12/27/2015
 * Time: 15:24
 */
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace AutoDiscovery
{
	/// <summary>
	/// Description of Listener.
	/// </summary>
	public class Listener
	{
		private UdpClient listener_client = null;
		private IPEndPoint group_ep =
			new IPEndPoint(IPAddress.Any, AutoDiscovery.BROADCAST_PORT);
		private bool listening = false;
		private bool started = false;
		private Thread list_thread = null;
		
		public delegate void RegisterMessage( string message );
		public delegate void ProcessMessage( string message );
		
		public Listener( UdpClient _client )
		{
			listener_client = _client;
		}
		
		private void StartInner( RegisterMessage reg_proc ) {
			try {
				started = true;
				listening = true;
				while ( listening ) {
					// waiting for a message
					byte[] bytes = listener_client.Receive( ref group_ep );
					var response =
						AutoDiscovery.ascii_encoding.GetString( bytes, 0, bytes.Length );
					
					reg_proc( response );
				}
				
			} catch ( ThreadInterruptedException ex ) {
				// doing nothing
			} catch ( Exception ex ) {
				throw new Exception( "Listener error: "+ex.Message, ex );
			} finally {
				Stop();
			}
		}
		
		public void Start( RegisterMessage reg_proc ) {
			if ( started )
				return;
			list_thread = new Thread( ()=>StartInner(reg_proc) );
			list_thread.Start();
		}
		
		public void Stop() {
			if ( !started )
				return;
			
			started = false;
			listening = false;
			list_thread.Interrupt();
		}
	}
}

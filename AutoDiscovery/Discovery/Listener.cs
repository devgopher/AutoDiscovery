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
	/// A broadcast messages listener
	/// </summary>
	public class Listener
	{
		private UdpClient listener_client = null;
		private IPEndPoint group_ep =
			new IPEndPoint(IPAddress.Any, AutoDiscovery.BROADCAST_PORT);
		private bool listening = false;
		private bool started = false;
		
		private Thread listener_thread = null;
		
		/// <summary>
		/// A delegate for registering a message in our system 
		/// </summary>
		/// <param name="message"></param>
		public delegate void RegisterMessage( string message );
		
		/// <summary>
		/// A delegate for processing of a message
		/// </summary>
		/// <param name="message"></param>
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
					// registering an incoming message
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
		
		/// <summary>
		/// Start listening in a special thread
		/// </summary>
		/// <param name="reg_proc">A procedure for registering of a message</param>
		public void Start( RegisterMessage reg_proc ) {
			if ( started )
				return;
			listener_thread = new Thread( ()=>StartInner(reg_proc) );
			listener_thread.Start();
		}
		
		/// <summary>
		/// Stop listening
		/// </summary>
		public void Stop() {
			if ( !started )
				return;
			
			started = false;
			listening = false;
			listener_thread.Interrupt();
		}
	}
}

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
			new IPEndPoint( IPAddress.Any, CommonEnvironment.BROADCAST_PORT );
		public bool Listening { get; private set; }
		public bool Started{ get; private set; }
		
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
			SetUdpClient(_client);
			Listening = false;
			Started = false;
		}
		
		public void SetUdpClient( UdpClient _client ) {
			listener_client = _client;
		}
		
		/// <summary>
		/// Start listening in a special thread
		/// </summary>
		/// <param name="reg_proc">A procedure for registering of a message</param>
		public void Start( RegisterMessage reg_proc ) {
			if ( Started )
				return;
			listener_thread = new Thread(()=>ThreadProc(reg_proc));
			listener_thread.Start();
			Started = true;
		}
		
		/// <summary>
		/// Stop listening
		/// </summary>
		public void Stop() {
			if ( !Started )
				return;
			

			Listening = false;
			listener_thread.Interrupt();
			Started = false;
		}

		/// <summary>
		/// A listening procedure
		/// </summary>
		/// <param name="reg_proc"></param>
		private void ThreadProc( RegisterMessage reg_proc ) {
			try {
				Listening = true;
				while ( Listening ) {
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
	}
}
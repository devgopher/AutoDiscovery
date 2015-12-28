/*
 * Пользователь: Igor.Evdokimov
 * Дата: 22.12.2015
 * Время: 14:08
 */
using System;
using System.Net;
using System.Text;
using System.Net.Sockets;
using System.Collections.Generic;

namespace AutoDiscovery
{
	/// <summary>
	/// Main discovery functions
	/// </summary>
	public class AutoDiscovery
	{
		public static int BROADCAST_PORT = 45000;

		private IPAddress broadcast_addr = IPAddress.Broadcast;
		private readonly UdpClient udp_client = null;
		private NodeStateNotificator nsm;
		private Listener listener;
		public IpList Hosts { get; private set; }
		public List<Message> Msgs { get; private set; }
		
		public static Encoding ascii_encoding = Encoding.ASCII;
		
		public AutoDiscovery()
		{
			Hosts = new IpList();
			Msgs = new List<Message>();
			
			udp_client = new UdpClient( BROADCAST_PORT, 
			                           AddressFamily.InterNetwork );
			udp_client.EnableBroadcast = true;
			nsm = new NodeStateNotificator( udp_client, broadcast_addr );
			listener = new Listener( udp_client );
		}
		
		/// <summary>
		/// Start discovery process
		/// </summary>
		public void StartNode() {
			// Start status messaging
			nsm.Start();
			// Start listener
			listener.Start( RegisterMsg );
		}
		
		/// <summary>
		/// Stop discovery process
		/// </summary>
		public void StopNode() {
			// Stop status messaging
			nsm.Stop();
			// Stop listener
			listener.Stop();
			
			// Clear messages and hosts lists
			Hosts.Clear();
			Msgs.Clear();
		}
		
		/// <summary>
		/// Message registration
		/// </summary>
		/// <param name="input">Received full message</param>
		private void RegisterMsg( string input ) {
			// Getting a Message object
			var message = Message.Get(input);
			// Registering it in a messages list
			Msgs.Add( message );
			// Processing message by it's contents
			ProcessMessage( message );
		}
		
		/// <summary>
		/// Processing a received message
		/// </summary>
		/// <param name="message"></param>
		private void ProcessMessage( Message message ) {
			switch( message.Contains ) {
				case NodeStateNotificator.start_bc_msg:
				case NodeStateNotificator.onair_bc_msg:
					RegisterHost( message );
					break;
				case NodeStateNotificator.stop_bc_msg:
					RemoveHost( message );
					break;
			}
		}
		
		/// <summary>
		/// A new host registration
		/// </summary>
		/// <param name="message"></param>
		private void RegisterHost( Message message ) {
			if (!(Hosts.Contains( message.FromIp )))
				Hosts.Add( message.FromIp );
		}
		
		/// <summary>
		/// Remove host from a host list
		/// </summary>
		/// <param name="message"></param>
		private void RemoveHost( Message message ) {
			RemoveHost(message.FromIp);
		}
	
		/// <summary>
		/// Remove host from a host list
		/// </summary>
		/// <param name="host_ip"></param>		
		private void RemoveHost( string host_ip ) {
			if ( Hosts.Contains( host_ip ))
				Hosts.Remove( host_ip );
		}
	}
}

/*
 * Пользователь: Igor.Evdokimov
 * Дата: 22.12.2015
 * Время: 14:08
 */
 
 #region Annotation
 /*
  * AutoDiscovery is a test solution for seeking neighbour
  * nodes in a local network. A UDP was used protocol 
  * due to several reasons:
  * - low packet delay;
  * - multicasting/broadcasting support;
  * - simplicity of usage.
  */ 
 #endregion
 
using System;
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
		private readonly UdpClient udp_client = null;
		private NodeStateNotificator nsm = null;
		private Listener listener = null;
		private ActivityMonitor act_monitor = null;
		
		public IpList Hosts { get; private set; }
		public List<Message> Msgs { get; private set; }
		
		public static Encoding ascii_encoding = Encoding.ASCII;
		
		public AutoDiscovery()
		{
			Hosts = new IpList();
			Msgs = new List<Message>();
			
			udp_client = CommonEnvironment.udp_client;
			nsm = new NodeStateNotificator( udp_client, CommonEnvironment.bcast_address );
			listener = new Listener( udp_client );
			act_monitor = new ActivityMonitor( this, RemoveHostRange, RemoveMsgsRange );
		}

		public AutoDiscovery( UdpClient _udp_client ) : this()
		{
			udp_client = _udp_client;
			udp_client.EnableBroadcast = true;
			listener.SetUdpClient(_udp_client);
		}
		
		/// <summary>
		/// Start discovery process
		/// </summary>
		public void StartNode() {
			// Start status messaging
			nsm.Start();
			// Start listener
			listener.Start( RegisterMsg );
			// Start activity monitor
			act_monitor.Start();
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
			
			// Stop activity monitor
			act_monitor.Stop();
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
		/// <param name="message">a message</param>
		private void RegisterHost( Message message ) {
			if (!(Hosts.Contains( message.FromIp )))
				Hosts.Add( message.FromIp );
		}
		
		/// <summary>
		/// Remove host from a host list
		/// </summary>
		/// <param name="message">a message</param>
		private void RemoveHost( Message message ) {
			RemoveHost(message.FromIp);
		}
		
		/// <summary>
		/// Remove host from a host list
		/// </summary>
		/// <param name="host_ip">host ip</param>
		private void RemoveHost( string host_ip ) {
			Hosts.Remove( host_ip );
		}
		
		/// <summary>
		/// Remove host from a host list
		/// </summary>
		/// <param name="hosts">hosts list</param>
		private void RemoveHostRange(List<string> hosts ) {
			foreach ( var host in hosts )
				RemoveHost(host);
		}		
		
		/// <summary>
		/// Remove messages from a messages list
		/// </summary>
		/// <param name="old_msgs">Old messages list</param>
		private void RemoveMsgsRange(List<Message> old_msgs ) {
			foreach ( var msg in old_msgs ) {
				Msgs.Remove( msg );
			}
		}
	}
}

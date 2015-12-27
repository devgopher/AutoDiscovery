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
		private NodeStateMessenger nsm;
		private Listener listener;
		public IpList Hosts { get; private set; }
		public List<Message> Msgs { get; private set; }
		
		public static Encoding ascii_encoding = Encoding.ASCII;
		
		public AutoDiscovery()
		{
			Hosts = new IpList();
			Msgs = new List<Message>();
			
			udp_client = new UdpClient(BROADCAST_PORT, AddressFamily.InterNetwork);
			udp_client.EnableBroadcast = true;
			nsm = new NodeStateMessenger( udp_client, broadcast_addr );
			listener = new Listener( udp_client );
			udp_client.MulticastLoopback = false;
		}
		
		public void StartNode() {
			nsm.Start();
			listener.Start( RegisterMsg );
		}
		
		public void StopNode() {
			nsm.Stop();
			listener.Stop();
			
			Hosts.Clear();
			Msgs.Clear();
		}
		
		public void RegisterMsg( string input ) {
			var message = Message.Get(input);
			Msgs.Add( message );
			ProcessMessage( message );
		}
		
		private void ProcessMessage( Message message ) {
			switch( message.Contains ) {
				case NodeStateMessenger.start_bc_msg:
				case NodeStateMessenger.onair_bc_msg:
					RegisterHost( message );
					break;
				case NodeStateMessenger.stop_bc_msg:
					RemoveHost( message );
					break;
			}
		}
		
		private void RegisterHost( Message message ) {
			if (!(Hosts.Contains( message.FromIp )))
				Hosts.Add( message.FromIp );
		}
		
		private void RemoveHost( Message message ) {
			if ( Hosts.Contains( message.FromIp ))
				Hosts.Remove( message.FromIp );
		}
	}
}

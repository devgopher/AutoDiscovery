/*
 * Пользователь: Igor.Evdokimov
 * Дата: 22.12.2015
 * Время: 14:08
 */
using System;
using ManagedUPnP;
using ManagedUPnP.Components;
using System.Collections.Generic;
using System.IO;

namespace AutoDiscovery
{
	/// <summary>
	/// Main discovery functions
	/// </summary>
	public class AutoDiscovery
	{
		public AutoDiscovery()
		{
			ip_list.Added += OutputItem;
			output_stream = new MemoryStream(1024);
			output_stream_writer = new StreamWriter(output_stream);
		}
		
		
		private void ScanServ() {
			service_list.Clear();
			service_list.AddRange( ManagedUPnP.Discovery.FindServices( AddressFamilyFlags.IPv4, false ) );
		}
		
		private List<string> GetAdapterIPs( Device device ) {
			foreach ( var adapter_ip in device.AdapterIPAddresses ) {
				if ( !ip_list.Contains( adapter_ip.ToString() ) ) {
					ip_list.Add( adapter_ip.ToString() );
				}
			}
			
			return ip_list;
		}
		
		public void Start() {
			
			ScanServ();
			foreach (  var serv in service_list ) {
				var ad_addrs = GetAdapterIPs( serv.Device );
			}
		}
		
		public void OutputItem( Object  sender, IpListEventArgs args ) {
		//	output_stream
		}
		
		private readonly Services service_list = new Services();
		private readonly IpList ip_list = new IpList();
		private readonly MemoryStream output_stream = null;
		private readonly StreamWriter output_stream_writer = null;
	}
}

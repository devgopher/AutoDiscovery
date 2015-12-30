/*
 * Пользователь: Igor.Evdokimov
 * Дата: 22.12.2015
 * Время: 11:30
 */
using System;
using System.Windows;
using System.Windows.Threading;

namespace AutoDiscovery
{
	/// <summary>
	/// Interaction logic for ADWindow.xaml
	/// </summary>
	public partial class ADWindow : Window
	{
		private AutoDiscovery ad = new AutoDiscovery();
		
		public ADWindow()
		{
			InitializeComponent();
			this.Closed += this.Window_Closed;
			
			ad.Hosts.Added += IpList_Updated;
			ad.Hosts.Removed += IpList_Updated;
		}
		
		void Start_Click(object sender, RoutedEventArgs e)
		{
			local_ip.Text = Utils.Utils.GetLocalIP();
			ad.StartNode();
			
			stop_button.IsEnabled = true;
			start_button.IsEnabled = false;
		}
		
		void Stop_Click(object sender, RoutedEventArgs e)
		{
			discovery_results.Clear();
			ad.StopNode();
			
			stop_button.IsEnabled = false;
			start_button.IsEnabled = true;
		}
		
		void Window_Closed(object sender, EventArgs e)
		{
			ad.StopNode();
		}
		
		void IpList_Updated( object sender,
		                    IpListEventArgs e )
		{
			var timer_disp_delegate =
				new Action (
					() => {
						discovery_results.Clear();
						var ip_list = ad.Hosts;
						foreach ( var ip in ip_list ) {
							discovery_results.Text += "\r\n"+ip;
						}
					}
				);
			
			Dispatcher.Invoke(
				timer_disp_delegate
			);

		}
	}
}
/*
 * Пользователь: Igor.Evdokimov
 * Дата: 22.12.2015
 * Время: 11:30
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.IO;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace AutoDiscovery
{
	/// <summary>
	/// Interaction logic for ADWindow.xaml
	/// </summary>
	public partial class ADWindow : Window
	{
		public ADWindow()
		{
			InitializeComponent();
		}
		
		void Start_Click(object sender, RoutedEventArgs e)
		{			
			AutoDiscovery ad = new AutoDiscovery();
			
			MemoryStream ms = new MemoryStream();
			StreamWriter sw = new StreamWriter( ms );
		
			ad.Start();
		//	discovery_results.Tex
			
			
		
		}
	}
}
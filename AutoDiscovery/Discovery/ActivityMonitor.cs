/*
 * User: Igor
 * Date: 29.12.2015
 * Time: 0:04
 */
using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;


namespace AutoDiscovery
{
	/// <summary>
	/// Nodes activity monitor
	/// </summary>
	public class ActivityMonitor
	{
		private Timer am_timer = null;
		private const int am_timer_int = 200;
		
		private Timer old_messages_timer = null;
		
		#if DEBUG
		private const int old_messages_timer_int = 600;
		#else
		private const int old_messages_timer_int = 6000;
		#endif
		
		public delegate void InactiveHostsHandler( List<string> inactive_hosts );
		public event InactiveHostsHandler HasInactiveHosts;
		
		public delegate void OldMessagesHandler( List<Message> old_messages );
		public event OldMessagesHandler HasOldMessages;
		
		private AutoDiscovery ad_obj = null;
		
		public ActivityMonitor( AutoDiscovery ad,
		                       InactiveHostsHandler inh,
		                       OldMessagesHandler omh ) {
			ad_obj = ad;
			if ( inh != null )
				HasInactiveHosts += inh;
			if ( omh != null )
				HasOldMessages += omh;
			
			// starting timers...
			// activity monitor general timer...
			am_timer = new Timer( AMTimerProc, 5, -1, am_timer_int );		

			// old messages tracking timer			
			old_messages_timer = new Timer( OMTimerProc, null, -1, old_messages_timer_int );
		}
		
		#region TimerProcs
		private void AMTimerProc( object state ) {
			if ( state is int ) {
				GetInactiveHosts((int)state);
			}
		}
		
		private void OMTimerProc( object state ) {
			GetOldMessages();
		}
		#endregion
		
		/// <summary>
		/// Start tracking process...
		/// </summary>
		public void Start() {
			// Switching timers on
			am_timer.Change( 0, am_timer_int  );
			old_messages_timer.Change( 0, old_messages_timer_int );
		}
		
		/// <summary>
		/// Stop tracking process
		/// </summary>
		public void Stop() {
			// switching off timers
			am_timer.Change( -1, am_timer_int  );
			old_messages_timer.Change( -1, old_messages_timer_int );
		}
		
		/// <summary>
		/// Removes messages, older, than 2 min
		/// </summary>
		private void GetOldMessages() {
			try {
				var msgs = ad_obj.Msgs;
				var now = DateTime.Now;
				
				var old_msgs = msgs
					.Select( (x) => x )
					.Where(
						(x) => {
							return ( (now - x.Received).TotalSeconds >= 120 );
						}
					);
				
				if ( old_msgs.Any() && HasOldMessages != null )
					HasOldMessages( old_msgs.ToList() );
			} catch ( Exception ex ) {
				// TODO: throws a new exception
			}
		}
		
		/// <summary>
		/// Start a new tracking activity iteration
		/// </summary>
		/// <param name="ad_obj">AutoDiscovery object for tracking</param>
		/// <param name="max_inactivity_time">A maximum host inactivity interval, sec</param>
		/// <returns>Inactive hosts list</returns>
		private void GetInactiveHosts( int max_inactivity_time ) {
			try {
				var ret = new List<String>();
				var current_time = DateTime.Now;
				var hosts = ad_obj.Hosts;
				var msgs = ad_obj.Msgs;
				
				foreach ( var host in hosts ) {
					var host_msgs = msgs.Select( (x) => x ).Where((x)=> x.FromIp==host );
					if ( host_msgs.Any() ) {
						var last_msg = host_msgs.Last();
						
						var time_difference = (current_time - last_msg.Received).TotalSeconds;
						
						if ( time_difference >= max_inactivity_time )
							ret.Add( host );
					}
				}
				
				if ( ret.Any() && HasInactiveHosts != null )
					HasInactiveHosts( ret );
				
			} catch ( Exception ex ) {
				// TODO: throws a new exception
			}
		}
	}
}

/*
 * Пользователь: Igor.Evdokimov
 * Дата: 12/22/2015
 * Время: 15:16
 */
using System;
using System.Collections.Generic;

namespace AutoDiscovery
{
	public class IpListEventArgs : EventArgs {
		
		public IpListEventArgs( string _item ) : base() {
			Item = _item;
		}
		public IpListEventArgs() : this(null) {

		}
		
		public string Item { get; set; }
	}
	
	/// <summary>
	/// IpList - is a List class realization
	/// with an ability to raise events on 
	/// "Add" and "Remove" actions
	/// </summary>
	public class IpList : List<String>
	{
		public IpList()
		{
		}
		
		public new void Add( String item ) {
			base.Add(item);
			if ( Added != null )
				Added( this, new IpListEventArgs(item));
		}
		
		public new void AddRange( IEnumerable<String> items ) {
			foreach ( var item in items ) {
				base.Add(item);
				if ( Added != null )
					Added( this, new IpListEventArgs(item));
			}
		}

		public new void Remove( String item ) {
			base.Remove(item);
			if ( Removed != null )
				Removed( this, new IpListEventArgs(item));
		}
		
		public new void RemoveRange( IEnumerable<String> items ) {
			foreach ( var item in items ) {
				base.Remove(item);
				if ( Removed != null )
					Removed( this, new IpListEventArgs(item));
			}
		}
		
		public delegate void ListChangedHandler( object sender, IpListEventArgs a );
		
		public event ListChangedHandler Added;
		public event ListChangedHandler Removed;
	}
}

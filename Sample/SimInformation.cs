
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V7.App;

namespace Sample
{
	[Activity (Label = "Sim")]			
	public class SimInformation : AppCompatActivity
	{
		Android.Support.V7.Widget.Toolbar _toolbar { get; set; }

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			SetContentView (Resource.Layout.activity_sim_information);

			//toolbar
			_toolbar = FindViewById<Android.Support.V7.Widget.Toolbar> (Resource.Id.toolbar);
			if (_toolbar != null) {
				SetSupportActionBar (_toolbar);
				SupportActionBar.SetDisplayHomeAsUpEnabled (true);
				SupportActionBar.SetHomeButtonEnabled (true);
				//SupportActionBar
			}		
		}

		public override bool OnOptionsItemSelected (IMenuItem item)
		{
			// Handle presses on the action bar items
			switch (item.ItemId) {
			case Android.Resource.Id.Home:
				OnBackPressed ();
				return true;

			default:
				return base.OnOptionsItemSelected (item);
			}
		}
	}
}


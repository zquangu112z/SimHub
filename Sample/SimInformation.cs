
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
using System.Dynamic;
using System.Threading.Tasks;
using System.Json;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace Sample
{
	[Activity (Label = "Sim")]			
	public class SimInformation : AppCompatActivity
	{
		//toolbar
		Android.Support.V7.Widget.Toolbar _toolbar { get; set; }

		//layout component
		TextView _textViewNumber;
		TextView _textViewPrice;
		TextView _textViewMade;
		TextView _textViewOwner;
		TextView _textViewOwnerPhone;

		Bundle _sim;//incoming data


		SimHubOwner.RootObject _owner;


		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			SetContentView (Resource.Layout.activity_sim_information);

			_sim = Intent.GetBundleExtra ("sim");

			string simOwnerId = _sim.GetString ("sim_owner_id");


			//toolbar
			_toolbar = FindViewById<Android.Support.V7.Widget.Toolbar> (Resource.Id.toolbar);
			if (_toolbar != null) {
				SetSupportActionBar (_toolbar);
				SupportActionBar.SetDisplayHomeAsUpEnabled (true);
				SupportActionBar.SetHomeButtonEnabled (true);
				//SupportActionBar
			}	

			//init component
			_textViewNumber = FindViewById<TextView>(Resource.Id.tv_sim_number);
			_textViewPrice = FindViewById<TextView> (Resource.Id.tv_sim_price);
			_textViewMade = FindViewById<TextView> (Resource.Id.tv_sim_made);
			_textViewOwner = FindViewById<TextView> (Resource.Id.tv_sim_owner);
			_textViewOwnerPhone = FindViewById<TextView> (Resource.Id.tv_sim_ownerphone);
			//set value
			_textViewNumber.Text = _sim.GetString("sim_number");
			_textViewPrice.Text = _sim.GetString("sim_price");
			_textViewMade.Text = _sim.GetString("sim_made");
			//_textViewOwner.Text = _sim.GetString("sim_owner");
			//_textViewOwnerPhone.Text = _sim.GetString ("sim_owner_phone");
			updateLayout (simOwnerId);

		}
		private async void updateLayout(string ownerId){
			JsonValue jsonDoc = await FetchDataAsync ("http://simhub.somee.com/owners/"+ownerId);
			List<SimHubOwner.RootObject> listUsers = JsonConvert.DeserializeObject<List<SimHubOwner.RootObject>>(jsonDoc.ToString());
			_owner = listUsers [0];
			_textViewOwner.Text = _owner.Name;
			_textViewOwnerPhone.Text = _owner.Phone;
			
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

		private async Task<JsonValue> FetchDataAsync (string url)
		{
			// Create an HTTP web request using the URL:
			HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create (new Uri (url));
			request.ContentType = "application/json";
			request.Method = "GET";

			// Send the request to the server and wait for the response:
			using (WebResponse response = await request.GetResponseAsync ()) {
				// Get a stream representation of the HTTP web response:
				using (Stream stream = response.GetResponseStream ()) {
					// Use this stream to build a JSON document object:
					JsonValue jsonDoc = await Task.Run (() => JsonObject.Load (stream));

					Console.Out.WriteLine ("-------------------------------Response: {0}", jsonDoc.ToString ());

					// Return the JSON document:
					return jsonDoc;
				}
			}
		}
	}
}


using System;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using com.refractored;
using Java.Interop;
using Java.Lang;
using Fragment = Android.Support.V4.App.Fragment;
using FragmentManager = Android.Support.V4.App.FragmentManager;
using String = Java.Lang.String;
using Android.Widget;
using System.Threading.Tasks;
using System.Net;
using System.Json;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using Java.Math;

namespace Sample
{
	[Activity (Label = "SimHub")] //, MainLauncher = true
    public class MainActivity : BaseActivity, IOnTabReselectedListener, ViewPager.IOnPageChangeListener
	{

		private Android.Support.V7.Widget.SearchView _searchView;


		private MyPagerAdapter adapter;
		//private int count = 1;
		private int currentColor;
		private Drawable oldBackground;
		private ViewPager pager;
		private PagerSlidingTabStrip tabs;

		public static List<RootObject>  simsMain = new List<RootObject>();

		//Owner
		//SimHubOwner.RootObject _owner;

		FinishBroadcastReceiver _finishBroadcastReciever;

		protected override int LayoutResource {
			get { return Resource.Layout.activity_main; }
		}

		public void OnPageScrollStateChanged (int state)
		{
			Console.WriteLine ("Page scroll state changed: " + state);
		}

		public void OnPageScrolled (int position, float positionOffset, int positionOffsetPixels)
		{
			Console.WriteLine ("Page Scrolled");
		}

		public void OnPageSelected (int position)
		{
			Console.WriteLine ("Page selected: " + position);
		}

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);


			//Get data from loginactivity


			adapter = new MyPagerAdapter (SupportFragmentManager);
			pager = FindViewById<ViewPager> (Resource.Id.pager);
			tabs = FindViewById<PagerSlidingTabStrip> (Resource.Id.tabs);
			pager.Adapter = adapter;
			tabs.SetViewPager (pager);

			var pageMargin = (int)TypedValue.ApplyDimension (ComplexUnitType.Dip, 4, Resources.DisplayMetrics);
			pager.PageMargin = pageMargin;
			pager.CurrentItem = 1;
			tabs.OnTabReselectedListener = this;
			tabs.OnPageChangeListener = this;

			SupportActionBar.SetDisplayHomeAsUpEnabled (false);
			SupportActionBar.SetHomeButtonEnabled (false);

			ChangeColor (Resources.GetColor (Resource.Color.green));


			//---------------
			//De tranh tinh trang dang o man hinh login nhan back lai tro ve man hinh mainactivity
			_finishBroadcastReciever = new FinishBroadcastReceiver (this);
			RegisterReceiver (_finishBroadcastReciever, new IntentFilter ("finish_main_activity"));
			//-----------------
		}


		//TODO rmv
		private void ChangeColor (Color newColor)
		{
			tabs.SetBackgroundColor (newColor);

			// change ActionBar color just if an ActionBar is available
			Drawable colorDrawable = new ColorDrawable (newColor);
			Drawable bottomDrawable = new ColorDrawable (Resources.GetColor (Android.Resource.Color.Transparent));
			var ld = new LayerDrawable (new[] { colorDrawable, bottomDrawable });
			if (oldBackground == null) {
				SupportActionBar.SetBackgroundDrawable (ld);
			} else {
				var td = new TransitionDrawable (new[] { oldBackground, ld });
				SupportActionBar.SetBackgroundDrawable (td);
				td.StartTransition (200);
			}

			oldBackground = ld;
			currentColor = newColor;
		}


		protected override void OnSaveInstanceState (Bundle outState)
		{
			base.OnSaveInstanceState (outState);
			//outState.PutInt("currentColor", currentColor);
		}

		protected override void OnRestoreInstanceState (Bundle savedInstanceState)
		{
			base.OnRestoreInstanceState (savedInstanceState);
			//currentColor = savedInstanceState.GetInt("currentColor");
			//ChangeColor(new Color(currentColor));
		}

		#region IOnTabReselectedListener implementation

		public void OnTabReselected (int position)
		{
			//TODO sort list tai day
			Toast.MakeText (this, "Tab reselected: " + position + "| Xin loi, tinh nang sap xep chua hoan thanh", ToastLength.Short).Show ();
		}

		#endregion

		#region menu

		public override bool OnCreateOptionsMenu (IMenu menu)
		{	
			//new code
			var inflater = MenuInflater;
			inflater.Inflate (Resource.Menu.main, menu);
			var item = menu.FindItem (Resource.Id.action_search);

			var searchView = MenuItemCompat.GetActionView (item);
			_searchView = searchView.JavaCast<Android.Support.V7.Widget.SearchView> ();

//			TODO su kien 
//			_searchView.QueryTextChange += (s, e) => {
//				
//			};

			_searchView.QueryTextSubmit += async (s, e) => {
				//TODO  Handle enter/search button on keyboard here
				double n;
				bool isNumeric = double.TryParse(e.Query, out n);
				if(isNumeric == false){
					Toast.MakeText (this, "Invalid Input, please check your input if it is numberic or not", ToastLength.Short).Show ();
				}else{
					Toast.MakeText (this, "Searched for: " + e.Query, ToastLength.Short).Show ();
					e.Handled = true;

					//list sim
					string urlRequest = LoginActivity._rootURL + "sims?end=" + e.Query;     //    /" +e.Query;
					JsonValue jsonDoc = await FetchDataAsync (urlRequest);
					Console.WriteLine("-----------------" + jsonDoc.ToString()); 

					//TODO chua the cap nhap du lieu 
					//du lieu moi
					simsMain = JsonConvert.DeserializeObject<List<RootObject>> (jsonDoc.ToString());

					//TODO NOtify
					pager.Adapter.NotifyDataSetChanged();
				}
			};

			MenuItemCompat.SetOnActionExpandListener (item, new SearchViewExpandListener (this));

			return true;
		}

		public override bool OnOptionsItemSelected (IMenuItem item)
		{
			// Handle presses on the action bar items
			switch (item.ItemId) {
			case Resource.Id.action_contact:
				var intent = new Intent (this, typeof(UserInformation));
				StartActivity (intent);
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
		#endregion
	}



	public class SearchViewExpandListener : Java.Lang.Object, MenuItemCompat.IOnActionExpandListener
	{
		Activity _activity;

		public SearchViewExpandListener (Activity a)//IFilterable adapter
		{
			_activity = a;

			//TODO rmv Toast
			Toast.MakeText (_activity, "SearchViewExpandListener", ToastLength.Short).Show ();
		}

		public bool OnMenuItemActionCollapse (IMenuItem item)
		{
			//TODO rmv Toast
			Toast.MakeText (_activity, "OnMenuItemActionCollapse", ToastLength.Short).Show ();
			return true;
		}

		public bool OnMenuItemActionExpand (IMenuItem item)
		{
			//TODO rmv Toast
			Toast.MakeText (_activity, "OnMenuItemActionExpand", ToastLength.Short).Show ();
			return true;
		}
	}



	//De tranh tinh trang dang o man hinh login nhan back lai tro ve man hinh mainactivity
	public class FinishBroadcastReceiver : BroadcastReceiver
	{
		Activity _activity;

		public FinishBroadcastReceiver ()
		{
		}

		public FinishBroadcastReceiver (Activity a)
		{
			_activity = a;
		}

		public override void OnReceive (Context context, Intent intent)
		{
			string action = intent.Action;
			if (action == "finish_main_activity") {
				_activity.Finish ();
				// DO WHATEVER YOU WANT.
			}
		}
	}
}
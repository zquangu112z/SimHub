
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
using Android.Support.V7.Widget;
using System.ComponentModel;
using Android.Graphics;
using Android.Graphics.Drawables;

namespace Sample
{
	[Activity (Label = "UserInformation")]			
	public class UserInformation : AppCompatActivity
	{
		TextView _textViewUserName;
		TextView _textViewUserAge;
		TextView _textViewUserPhone;
		TextView _textViewUserAddress;
		TextView _textViewUserDateJoined;
		TextView _textViewUserSign;
		TextView _textViewUserStatus;
		Button _buttonLicense;
		Button _buttonLogout;

		public Android.Support.V7.Widget.Toolbar _toolbar { get; set; }

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			SetContentView (Resource.Layout.acivity_userinformation);
			//init
			_textViewUserName = FindViewById<TextView> (Resource.Id.tv_name);
			_textViewUserAge = FindViewById<TextView> (Resource.Id.tv_age);
			_textViewUserPhone = FindViewById<TextView> (Resource.Id.tv_phone);
			_textViewUserAddress = FindViewById<TextView> (Resource.Id.tv_address);
			_textViewUserDateJoined = FindViewById<TextView> (Resource.Id.tv_date_joined);
			_textViewUserSign = FindViewById<TextView> (Resource.Id.tv_sign);
			_textViewUserStatus = FindViewById<TextView> (Resource.Id.tv_status);
			_buttonLicense = FindViewById<Button> (Resource.Id.btn_license);
			_buttonLogout = FindViewById<Button> (Resource.Id.btn_logout);


			//toolbar
			_toolbar = FindViewById<Android.Support.V7.Widget.Toolbar> (Resource.Id.toolbar);
			if (_toolbar != null) {
				SetSupportActionBar (_toolbar);
				SupportActionBar.SetDisplayHomeAsUpEnabled (true);
				SupportActionBar.SetHomeButtonEnabled (true);
				//SupportActionBar
			}			
				

			//TODO: get user information
			ISharedPreferences prefs = Application.Context.GetSharedPreferences (LoginActivity._statusLogin, FileCreationMode.Private);
			SimHubOwner.RootObject owner = new SimHubOwner.RootObject();
			owner.Name = prefs.GetString("owner_name", "error 404");
			owner.Age = prefs.GetString("owner_age","error 404");
			owner.Address = prefs.GetString("owner_address","error 404");
			owner.Id = Int32.Parse(prefs.GetString("owner_id","69"));//69 means error404
			owner.DateJoined = prefs.GetString("owner_datejoined","error 404");
			owner.Phone = prefs.GetString("owner_phone","error 404");
			owner.Sign = prefs.GetString("owner_sign", "error 404");


			//set layout value
			_textViewUserName.Text = owner.Name;
			_textViewUserAge.Text = owner.Age;
			_textViewUserPhone.Text = owner.Phone;
			_textViewUserAddress.Text = owner.Address;
			_textViewUserDateJoined.Text = owner.DateJoined;
			_textViewUserSign.Text = owner.Sign;

			//TODO rmv
			if(true){//license
				_textViewUserStatus.Text = "licensed";
				_buttonLicense.Visibility = Android.Views.ViewStates.Invisible;

			}else{
				_textViewUserStatus.Text = "limited";
				_buttonLicense.Visibility = Android.Views.ViewStates.Visible;
			}

			//handle event logout
			_buttonLogout.Click += (object sender, EventArgs e) => {
				savingLoginStatus(false);

				//TODO De tranh tinh trang dang o man hinh login nhan back lai tro ve man hinh mainactivity
				//SetResult(31);
				this.Finish();

				//go back login activity
				Intent intent = new Intent (this, typeof(LoginActivity));
				StartActivity (intent);

				Intent intent2 = new Intent("finish_main_activity");
				SendBroadcast(intent2);

				MainActivity.simsMain.Clear();
			};
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


		/**
		 * TODO Luu trang thai, luc login thanh cong
		 */
		private void savingLoginStatus (bool b)
		{
			ISharedPreferences prefs = Application.Context.GetSharedPreferences (LoginActivity._statusLogin, FileCreationMode.Private);
			ISharedPreferencesEditor editor = prefs.Edit ();
			editor.PutBoolean ("login", b);
			editor.Apply ();    
		}




	}
}


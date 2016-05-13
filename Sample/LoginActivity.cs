
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
using Android.Preferences;
using System.Threading.Tasks;
using System.Net;
using System.Json;
using System.IO;
using Newtonsoft.Json;
using Android.Drm;
using System.Threading;

namespace Sample
{
	[Activity (Label = "SimHub", MainLauncher = true, Icon = "@drawable/ic_launcher_sim")]			
	public class LoginActivity : Activity
	{
		public static string _statusLogin = "status_login";
//shared preference key
		public static string _rootURL = "http://simhub.somee.com/";
		//home
		public bool _succesLogin = false;
		Button _buttonLogin;
		EditText _edittextUsername;
		EditText _edittextPassword;

		//Owner
		SimHubOwner.RootObject _owner;

		//for timeout feature 
		CancellationTokenSource cancellationTokenSource;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			bool loginStatus = readLoginStatus ();
			if (loginStatus == null || loginStatus == true) {
				this.Finish ();
				Intent intent = new Intent (this, typeof(MainActivity));
				StartActivity (intent);
			}

			SetContentView (Resource.Layout.activity_login);



			//init layout
			_buttonLogin = FindViewById<Button> (Resource.Id.btn_login);
			_edittextUsername = FindViewById<EditText> (Resource.Id.edt_username);
			_edittextPassword = FindViewById<EditText> (Resource.Id.edt_password);

			_buttonLogin.Click += (object sender, EventArgs e) => {
				//Task<bool> login = excuteLogin (_edittextUsername.Text, _edittextPassword.Text);
				excuteLogin (_edittextUsername.Text, _edittextPassword.Text);

			};
		}



		/**
		 * kiem tra dang nhap
		 * ok: return true;
		 * fail: return false;
		 */
		private async Task<bool>  excuteLogin (string username, string password)
		{
			var loginProgressDialog = ProgressDialog.Show(this, "Please wait...", "Checking account info...", true);

			//TODO get json, then check
			//example
			//check if the edittext content null or invalid input
			if (username == " " || password.Length < 7 || password == " ") {
				//succesLogin = false;
				loginProgressDialog.Dismiss();
				//Toast
				Toast.MakeText (this, "invalid input", ToastLength.Short).Show ();

				return false;
			}
			string urlRequest = _rootURL + "owners?username="+username;

			//TODO rmv Log
			Console.WriteLine("----------" + urlRequest);
			JsonValue jsonDoc =  await FetchDataAsync (urlRequest);
			Console.WriteLine("----------" + jsonDoc.ToString());
			//SimHubOwner.RootObject root = JsonConvert.DeserializeObject<SimHubOwner.RootObject> (jsonDoc.ToString());
			_owner = new SimHubOwner.RootObject();
			Console.WriteLine ("----------" + _owner.Name);
			List<SimHubOwner.RootObject> listUsers = JsonConvert.DeserializeObject<List<SimHubOwner.RootObject>>(jsonDoc.ToString());
			_owner = listUsers [0];
			if(_owner == null){
				_succesLogin = false;
				Console.WriteLine ("----------" + "_owner is null");
				//loginProgressDialog.Dismiss ();
			}else{
				Console.WriteLine ("----------" + "_owner is not null");
				if(_owner.Password == password){
					_succesLogin = true;
				}else{
					_succesLogin = false;
					//loginProgressDialog.Dismiss ();
				}
			}




			//TODO: handle login success event
			if (_succesLogin) {//TODO rmv login
				//ngan tinh trang user nhan nut back se tro ve trang login
				this.Finish ();
				//save loginStatus
				savingLoginStatus (true);
				//start main activity
				Intent intent = new Intent (this, typeof(MainActivity));
				StartActivity (intent);
			} else {//  = false
				//save loginStatus
				savingLoginStatus (false);
				//TODO: check if wrong passcode or wrong username
				//Toast
				Toast.MakeText (this, "wrong passcode or username", ToastLength.Short).Show ();
			}

			Console.WriteLine("----------_succesLogin" + _succesLogin.ToString());
			loginProgressDialog.Dismiss ();

			return true;//khong quan trong gia tri tra ve
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


		/**
		 * TODO Luu trang thai, luc login thanh cong
		 */
		private void savingLoginStatus (bool b)
		{
			ISharedPreferences prefs = Application.Context.GetSharedPreferences (_statusLogin, FileCreationMode.Private);
			ISharedPreferencesEditor editor = prefs.Edit ();
			//luu trang thai dang nhap
			editor.PutBoolean ("login", b);

			//luu thong tin user
			editor.PutString ("owner_name", _owner.Name);
			editor.PutString ("owner_age", _owner.Age);
			editor.PutString ("owner_address", _owner.Address);
			editor.PutString ("owner_id", _owner.Id.ToString());
			editor.PutString ("owner_datejoined", _owner.DateJoined);
			editor.PutString ("owner_phone", _owner.Phone);
			editor.PutString ("owner_sign", _owner.Sign);

			editor.Apply ();    
		}

		/**
		 * TODO Doc trang thai luc khoi dong ung dung
		 */
		private bool readLoginStatus ()
		{
			ISharedPreferences prefs = Application.Context.GetSharedPreferences (_statusLogin, FileCreationMode.Private);
			return prefs.GetBoolean ("login", false);//arg2: defaultValue =)) 
		}
	}
}


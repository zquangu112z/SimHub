//
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//
//using Android.App;
//using Android.Content;
//using Android.OS;
//using Android.Runtime;
//using Android.Util;
//using Android.Views;
//using Android.Widget;
//
//namespace Sample
//{
//	public class ListSimFragment : Fragment
//	{
//		public override void OnCreate (Bundle savedInstanceState)
//		{
//			base.OnCreate (savedInstanceState);
//
//			// Create your fragment here
//		}
//
//		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
//		{
//			// Use this to return your custom view for this Fragment
//			 return inflater.Inflate(Resource.Layout.fragment_listsim, container, false);
//
//			//return base.OnCreateView (inflater, container, savedInstanceState);
//		}
//	}
//}
using Android.Support.V4.App;
using Android.OS;
using Android.Support.V4.View;


using Android.Widget;
using System.Collections.Generic;
using System.ComponentModel;
using Android.Content;

namespace Sample
{
	public class ListSimFragment : Fragment
	{
		private int position;

		ListView listSim;
		//List<RootObject> sims;
		List<RootObject> simsAfterClassify;

		public static ListSimFragment NewInstance(int position)
		{
			var f = new ListSimFragment ();
			var b = new Bundle ();
			b.PutInt("position", position);
			f.Arguments = b;
			return f;
		}

		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			position = Arguments.GetInt ("position");
		}


		public override Android.Views.View OnCreateView (Android.Views.LayoutInflater inflater, Android.Views.ViewGroup container, Bundle savedInstanceState)
		{
			var root = inflater.Inflate(Resource.Layout.fragment_listsim, container, false);

			//khoi tao du lieu : 0 viettel, 1 mobifone, 2 vinaphone, 3 another
			//initData (position);

			listSim = root.FindViewById<ListView>(Resource.Id.lv_listsim); // get reference to the ListView in the layout
			// populate the listview with data
			simsAfterClassify = classifySim (MainActivity.simsMain, position);
			listSim.Adapter = new ListSimAdapter(this.Activity, simsAfterClassify);
//			listSim.Adapter = new ListSimAdapter(this.Activity, MainActivity.simsMain);
			//((BaseAdapter)listSim.Adapter).NotifyDataSetChanged ();
			listSim.ItemClick += OnListItemClick;  // to be defined


			ViewCompat.SetElevation(root, 50);
			return root;
		}
		public override void OnResume(){
			base.OnResume ();

		}

		//TODO onItemClick
		void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
		{
			var listView = sender as ListView;
			var t = simsAfterClassify[e.Position];
			Android.Widget.Toast.MakeText(this.Activity, t.Number, Android.Widget.ToastLength.Short).Show();

			var intent = new Intent (this.Activity, typeof(SimInformation));
			StartActivity (intent);
		}

		/**
		 * Phan loai sims de hien thi len listview: vi listsim dau vao thuoc ca 3 loai sim
		 * list: tat ca cac sim
		 * type: 0 viettel, 1 mobifone, 2 vinaphone, 3 another
		 */
		private List<RootObject> classifySim(List<RootObject> listIn, int type){
			List<RootObject> listOut = new List<RootObject>();
			switch(type){
			case 0: //viettel
				foreach(RootObject sim in listIn){
					if(sim.Made == "viettel"){
						listOut.Add (sim);
					}
				}
				break;
			case 1: //mobifone
				foreach(RootObject sim in listIn){
					if(sim.Made == "mobifone"){
						listOut.Add (sim);
					}
				}
				break;
			
			case 2: //vinaphone
				foreach (RootObject sim in listIn) {
					if (sim.Made == "vinaphone") {
						listOut.Add (sim);
					}
				}
			break;
			case 3: //mobifone
			foreach(RootObject sim in listIn){
				if(sim.Made == "another"){
					listOut.Add (sim);
				}
			}
			break;

		}
			return listOut;
		}




		/**
		 * Khoi tao du lieu
		 * */
		private void initData(int pos){
			//TODO: load du lieu theo pos
			//0 viettel, 1 mobifone, 2 vinaphone, 3 another
			MainActivity.simsMain = new List<RootObject>();

			string[] number = { "123456123", "123456456", "123456789", "123456234", "123456567"};
			string[] price = { "1000", "1000", "2000", "3000", "4000"};
			string[] made = { "viettel", "mobifone", "vinaphone", "mobifone", "viettel" };
			string[] owner = { "ngu", "ngu", "ngu", "ngu", "ngu" };


			for (int i = 0; i < number.Length; i++){
				RootObject sim = new RootObject ();
				sim.Number = number [i];
				sim.Price = price [i];
				sim.Made = made [i];
				//sim.Owner = owner [i];
				//RootObject sim = new RootObject (number [i], price [i], owner [i], made [i]);
				MainActivity.simsMain.Add (sim);
			}
		}
	}
}



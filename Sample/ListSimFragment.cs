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

namespace Sample
{
	public class ListSimFragment : Fragment
	{
		private int position;
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
			var text = root.FindViewById<TextView> (Resource.Id.tv_listsim);
			text.Text += position;
			ViewCompat.SetElevation(root, 50);
			return root;
		}
	}
}



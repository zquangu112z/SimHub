using System;
using System;
using Android.Widget;
using System.Collections.Generic;
using Android.App;
using Android.Views;

namespace Sample
{
	//RootObject is Sim class
	public class ListSimAdapter : BaseAdapter<RootObject>
	{
		List<RootObject> items;
		Activity context;

		public ListSimAdapter (Activity context, List<RootObject> items)
			: base ()
		{
			this.context = context;
			this.items = items;

		}
		//		public ListSimAdapter(Fragment context, List<Sim> items)
		//			: base()
		//		{
		//			this.context = context.Activity;
		//			this.items = items;
		//		}
		public override long GetItemId (int position)
		{
			return position;
		}

		public override RootObject this [int position] {
			get { return items [position]; }
		}

		public override int Count {
			get { 
				return items.Count;
			}
		}

		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			var item = items [position];
			View view = convertView;
			if (view == null) // no view to re-use, create new
				view = context.LayoutInflater.Inflate (Resource.Layout.item_listsim, null);
			view.FindViewById<TextView> (Resource.Id.tv_number).Text = item.Number;
			view.FindViewById<TextView> (Resource.Id.tv_price).Text = item.Price;
			return view;
		}
	}
}

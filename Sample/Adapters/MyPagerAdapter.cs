using System;
using Android.Support.V4.App;
using Java.Lang;

namespace Sample
{
	//linked with tab layout
	public class MyPagerAdapter : FragmentPagerAdapter
	{
		private readonly string[] Titles = {
			"Viettel", "Mobifone", "Vinaphone", "Another"
		};

		public MyPagerAdapter (FragmentManager fm) : base (fm)
		{
		}

		public override ICharSequence GetPageTitleFormatted (int position)
		{
			return new Java.Lang.String (Titles [position]);
		}

		#region implemented abstract members of PagerAdapter

		public override int Count {
			get { return Titles.Length; }
		}

		#endregion

		#region implemented abstract members of FragmentPagerAdapter

		public override Fragment GetItem (int position)
		{
			return ListSimFragment.NewInstance (position);
		}
		#endregion

		public override int GetItemPosition (Java.Lang.Object objectValue)
		{
			//notify datasetchanged in current fragment
			return PositionNone;
		}
	}
}


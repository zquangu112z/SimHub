using System;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Util;
using Android.Views;
using Android.Widget;
using com.refractored;
using Java.Interop;
using Java.Lang;
using Fragment = Android.Support.V4.App.Fragment;
using FragmentManager = Android.Support.V4.App.FragmentManager;
using String = Java.Lang.String;

namespace Sample
{
    [Activity(Label = "SimHub", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : BaseActivity, IOnTabReselectedListener, ViewPager.IOnPageChangeListener
    {
        private MyPagerAdapter adapter;
        //private int count = 1;
        private int currentColor;
        private Drawable oldBackground;
        private ViewPager pager;
        private PagerSlidingTabStrip tabs;

        protected override int LayoutResource
        {
            get { return Resource.Layout.activity_main; }
        }

        public void OnPageScrollStateChanged(int state)
        {
            Console.WriteLine("Page scroll state changed: " + state);
        }

        public void OnPageScrolled(int position, float positionOffset, int positionOffsetPixels)
        {
            Console.WriteLine("Page Scrolled");
        }

        public void OnPageSelected(int position)
        {
            Console.WriteLine("Page selected: " + position);
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            adapter = new MyPagerAdapter(SupportFragmentManager);
            pager = FindViewById<ViewPager>(Resource.Id.pager);
            tabs = FindViewById<PagerSlidingTabStrip>(Resource.Id.tabs);
            pager.Adapter = adapter;
            tabs.SetViewPager(pager);

            var pageMargin = (int) TypedValue.ApplyDimension(ComplexUnitType.Dip, 4, Resources.DisplayMetrics);
            pager.PageMargin = pageMargin;
            pager.CurrentItem = 1;
            tabs.OnTabReselectedListener = this;
            tabs.OnPageChangeListener = this;

            SupportActionBar.SetDisplayHomeAsUpEnabled(false);
            SupportActionBar.SetHomeButtonEnabled(false);

            ChangeColor(Resources.GetColor(Resource.Color.green));
        }

        private void ChangeColor(Color newColor)
        {
            tabs.SetBackgroundColor(newColor);

            // change ActionBar color just if an ActionBar is available
            Drawable colorDrawable = new ColorDrawable(newColor);
            Drawable bottomDrawable = new ColorDrawable(Resources.GetColor(Android.Resource.Color.Transparent));
            var ld = new LayerDrawable(new[] {colorDrawable, bottomDrawable});
            if (oldBackground == null)
            {
                SupportActionBar.SetBackgroundDrawable(ld);
            }
            else
            {
                var td = new TransitionDrawable(new[] {oldBackground, ld});
                SupportActionBar.SetBackgroundDrawable(td);
                td.StartTransition(200);
            }

            oldBackground = ld;
            currentColor = newColor;
        }


        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
            //outState.PutInt("currentColor", currentColor);
        }

        protected override void OnRestoreInstanceState(Bundle savedInstanceState)
        {
            base.OnRestoreInstanceState(savedInstanceState);
            //currentColor = savedInstanceState.GetInt("currentColor");
            //ChangeColor(new Color(currentColor));
        }

        #region IOnTabReselectedListener implementation

        public void OnTabReselected(int position)
        {
			//TODO sort list tai day
            Toast.MakeText(this, "Tab reselected: " + position + "| Xin loi, tinh nang sap xep chua hoan thanh", ToastLength.Short).Show();
        }

        #endregion

        #region menu

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            var inflater = MenuInflater;
            inflater.Inflate(Resource.Menu.main, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            // Handle presses on the action bar items
            switch (item.ItemId)
            {

//                case Resource.Id.action_icons:
//
//                    var intent = new Intent(this, typeof(SecondActivity));
//                    StartActivity(intent);
//                    return true;
			case Resource.Id.action_contact:
				var intent = new Intent (this, typeof(UserInformation));
				StartActivity (intent);
				return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        #endregion
    }

    public class MyPagerAdapter : FragmentPagerAdapter
    {
        private readonly string[] Titles =
        {
            "Viettel", "Mobifone", "Vinaphone", "Another"
        };

        public MyPagerAdapter(FragmentManager fm) : base(fm)
        {
        }

        public override ICharSequence GetPageTitleFormatted(int position)
        {
            return new String(Titles[position]);
        }

        #region implemented abstract members of PagerAdapter

        public override int Count
        {
            get { return Titles.Length; }
        }

        #endregion

        #region implemented abstract members of FragmentPagerAdapter

        public override Fragment GetItem(int position)
        {
			return ListSimFragment.NewInstance(position);
        }

        #endregion
    }
}
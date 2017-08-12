﻿using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content;
using SQLite;
using System.IO;

namespace App1.Droid
{
	[Activity (Theme="@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
        protected override void OnCreate (Bundle bundle)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar; 

			base.OnCreate (bundle);

            sqliteConn = new SQLiteConnection(dbPath);
			global::Xamarin.Forms.Forms.Init (this, bundle);
			LoadApplication (new App1.App ());
		}
	}
}


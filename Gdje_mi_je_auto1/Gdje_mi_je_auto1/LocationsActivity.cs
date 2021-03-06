﻿using System.IO;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Gdje_mi_je_auto1
{
	[Activity (ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait,NoHistory = true)]			
	public class LocationsActivity : Activity
	{
		private TextView textLocations;
		private Button btnClear;
		private string locationsFile;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.Locations);

			textLocations = FindViewById<TextView> (Resource.Id.textLocations);

			btnClear = FindViewById<Button> (Resource.Id.locationsClear);

			btnClear.Click += delegate {
				File.WriteAllText(locationsFile,string.Empty);
				textLocations.Text = "";
			};

			locationsFile = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
			locationsFile = Path.Combine(locationsFile, "locations.txt");

			if (!File.Exists(locationsFile))
				File.Create(locationsFile);


			textLocations.Text = File.ReadAllText(locationsFile);

		}
	}
}


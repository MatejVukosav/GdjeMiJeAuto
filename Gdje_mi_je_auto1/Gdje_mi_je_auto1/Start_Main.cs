
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

namespace Gdje_mi_je_auto1
{
	[Activity (Label = "Start_Main",MainLauncher = true)]			
	public class Start_Main : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.Start);
			//komentar testni
			//test 2

			Button VUKIB = FindViewById<Button> (Resource.Id.VUKIB);
			Button FILIPB = FindViewById<Button> (Resource.Id.FILIPB);


			//Pokreni SMS
			VUKIB.Click += delegate {
				var activity_start_sms_main = new Intent (this, typeof(Pay_Main));
				StartActivity (activity_start_sms_main);
			};

			FILIPB.Click += delegate {
				var activity_start_sms_main = new Intent (this, typeof(GPS_Main));
				StartActivity (activity_start_sms_main);
			};


		}
	}
}


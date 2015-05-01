using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

/*
 * Početni prozor za plaćanje. Sadrži odabir SMS ili Automat
 * */

namespace Part_Posalji
{
	[Activity (MainLauncher = true,Icon = "@drawable/main_icon")] //MainLauncher = true,
	public class Pay_Main:Activity
    {
       

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.Pay_Main);

			Button SMS_Main = FindViewById<Button> (Resource.Id.Pay_SMS_Main);
			Button MainAutomat = FindViewById<Button> (Resource.Id.Pay_Automat_Main);

	
			//Pokreni SMS
			SMS_Main.Click += delegate {
				var activity_start_sms_main = new Intent (this, typeof(Pay_SMS_Main));
				StartActivity (activity_start_sms_main);
			};
				
			//Pokreni automat
			MainAutomat.Click += delegate {
				var activity_start_automat = new Intent (this, typeof(Pay_Automat_Main));
				StartActivity (activity_start_automat);
			};




		}
	}
}



	



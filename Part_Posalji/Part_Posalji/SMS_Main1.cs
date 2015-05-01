using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Telephony.Gsm;


/*
 * @Matej Vukosav 04.04.2015
 * Aplikacija za slanje SMS poruka.
 * 
 * */
namespace Part_Posalji
{
	[Activity (Label = "Part_Posalji")]
	public class SMS_Main : Activity
	{

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.SMS_Main);

			Button sms = FindViewById<Button> (Resource.Id.SMS);

			sms.Click += delegate {
				var activity_slanje_sms=new Intent (this,typeof(SMS_Main_Sending));
				activity_slanje_sms.PutExtra ("MyData","Data from MainActivity");
				StartActivity (activity_slanje_sms);
			};


		}
	}
}



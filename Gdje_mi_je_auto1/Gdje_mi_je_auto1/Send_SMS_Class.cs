using System;
using Android.Telephony;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Text;
using Android.Graphics;


namespace Gdje_mi_je_auto1
{
	/*
	 * Testna klasa koja je sluzila za brzinsko slanje sms poruke.
	 * */
	public class Send_SMS_Class:Activity
	{
		Android.Telephony.SmsManager smsManager = Android.Telephony.SmsManager.Default;

		protected override void OnCreate (Bundle bundle)
		{
			//RequestWindowFeature(WindowFeatures.NoTitle);
			base.OnCreate (bundle);
		}

		public void Send_SMS (bool valid_check,string phone_number,string sms_message)
		{
					
			if(valid_check){
				smsManager.SendTextMessage (phone_number,null,sms_message,null,null);
				Toast.MakeText(ApplicationContext,"SMS je poslan.",ToastLength.Short).Show ();

				//vracanje u Main
				var activity_pay_main=new Intent (this,typeof(Pay_Main));
				StartActivity (activity_pay_main);
			}
			else
			{
				Toast.MakeText (ApplicationContext,"Unos pogresan.",ToastLength.Short).Show ();
			}

		}
	}
}


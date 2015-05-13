using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Util;
using System.Collections.Generic;

/*
 * Početni prozor za plaćanje. Sadrži odabir SMS ili Automat
 * */

namespace Gdje_mi_je_auto1
{
	[Activity (Icon = "@drawable/main_icon",MainLauncher = true)] 
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


			#region postavke
			ToggleButton tb1 = FindViewById<ToggleButton> (Resource.Id.toggleButton1);
			ToggleButton tb2 = FindViewById<ToggleButton> (Resource.Id.toggleButton2);




			tb1.CheckedChange += delegate {
				if(tb1.Checked){
					Fill_ListView_With_Data.Enable_message_update=true;
					tb2.Checked=false;
				}

			};
			tb2.CheckedChange += delegate {
				if(tb2.Checked){
					Fill_ListView_With_Data.DeleteHistory ();
					tb1.Checked=false;
				}

			};

			#endregion

		}
	}
}



	



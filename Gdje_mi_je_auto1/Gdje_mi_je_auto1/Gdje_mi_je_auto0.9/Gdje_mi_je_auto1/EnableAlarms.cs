
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
	[Activity (NoHistory = true,ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]			
	public class EnableAlarms : Activity
	{


		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.EnableAlarms);

			Button btnAlarmEnable = FindViewById<Button> (Resource.Id.alarmEnableBtn);
			TextView txtAlarmEnable = FindViewById<TextView> (Resource.Id.AlarmEnableTxt);

			var prefs = Application.Context.GetSharedPreferences("MySharedPrefs", FileCreationMode.Private);
			var alarmEnabled=prefs.GetBoolean ("MyAlarmValue", true);
			if (alarmEnabled) {
				txtAlarmEnable.Text = "Alarm je upaljen.";
			} else {
				txtAlarmEnable.Text = "Alarm je ugašen.";
			}
				
			btnAlarmEnable.Click += delegate {
				CreateAddProjectDialog();
			};

		}

		private void CreateAddProjectDialog() { 
			var alert = new AlertDialog.Builder (this);
			alert.SetTitle ("Želite li upaliti alarm?");
			//alert.SetView (layoutProperties);
			alert.SetCancelable (false);
			alert.SetPositiveButton("Da", HandlePositiveButtonClick);
			alert.SetNegativeButton("Ne", HandelNegativeButtonClick);
			var dialog = alert.Create();
			dialog.Show();
		}

		private void HandlePositiveButtonClick (object sender, EventArgs e) {

			var prefs = Application.Context.GetSharedPreferences ("MySharedPrefs", FileCreationMode.Private);
			var prefsEdit = prefs.Edit ();
			prefsEdit.PutBoolean ("MyAlarmValue", true).Commit ();
			var activity_EA=new Intent (this,typeof(EnableAlarms));
			StartActivity (activity_EA);

		
		}

		private void HandelNegativeButtonClick (object sender, EventArgs e) {
			var prefs = Application.Context.GetSharedPreferences ("MySharedPrefs", FileCreationMode.Private);
			var prefsEdit = prefs.Edit ();
			prefsEdit.PutBoolean ("MyAlarmValue", false).Commit ();
			var activity_EA=new Intent (this,typeof(EnableAlarms));
			StartActivity (activity_EA);
		}




	}
}


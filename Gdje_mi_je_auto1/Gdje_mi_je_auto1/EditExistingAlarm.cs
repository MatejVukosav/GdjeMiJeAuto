
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
	public class EditExistingAlarm : Activity
	{
		private Button btnUpali;
		private Button btnUgasi;


		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.EditExistingAlarm);


			btnUpali = FindViewById<Button> (Resource.Id.btnUpali);
			btnUgasi = FindViewById<Button> (Resource.Id.btnUgasi);

			String idAlarma = Intent.GetStringExtra ("idAlarma");
			int idAlarmaInt = Convert.ToInt32 (idAlarma);
			int i = 0;
			foreach (KeyValuePair<string,string>pair in AlarmMain.AlarmiDictionary) {
				if (i == idAlarmaInt) {
					idAlarmaInt = Convert.ToInt32 (pair.Key);
					break;
				}

			}

			btnUgasi.Click+=delegate {
				// ugasi alarm, (ali ne obrisat)
				Alarms.cancelAlarm (idAlarmaInt);
				var activityAM = new Intent (this, typeof(AlarmMain));
				StartActivity (activityAM);
			};
			
			btnUpali.Click+=delegate {
				//upali alarm
				Alarms.resumeAlarm (idAlarmaInt);
				var activityAM = new Intent (this, typeof(AlarmMain));
				StartActivity (activityAM);
			};


		}

	}
}


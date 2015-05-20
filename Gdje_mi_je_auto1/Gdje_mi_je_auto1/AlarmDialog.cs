
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
using Android.Util;

namespace Gdje_mi_je_auto1
{
	/*
	 * Klasa koja otvara dialog te obavijestava korisnika da je alarm istekao. 
	 * */
	[Activity (Label = "Alarm",NoHistory =true)]			
	public class AlarmDialog:Activity
	{

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.AlarmAlertDialog);
			CreateAlarmDialog ();

			//usporeduje vremena alarma i onaj koji je istekao obrise.
			foreach (KeyValuePair<string,string>pair in AlarmMain.AlarmiDictionary) {
				String[] ureditAlarme = pair.Value.Split ('&');
				String vrijeme = ureditAlarme [0] + ureditAlarme [1];
				String TimeH=DateTime.Now.Hour.ToString ();
				String TimeM=DateTime.Now.Minute.ToString ();
				String Time=TimeH+":"+TimeM;
				if (vrijeme.Equals (Time)) {
					Alarms.deleteAlarm(Convert.ToInt32 (pair.Key));
				}
			}
		}

		/*
		 * Metoda koja otvara dialog o isteku parkinga.
		 * */
		public void CreateAlarmDialog() { 

			//Toast.MakeText (this, "Alarm!", ToastLength.Long).Show ();
			var builder = new AlertDialog.Builder (this);
			builder.SetTitle ("Alert title");
			builder.SetCancelable (false);
			builder.SetMessage ("Istekao parking");
			builder.SetPositiveButton ("OK",(s, ev) => {

			});

			var alarmDialog = builder.Create ();
			try{
				alarmDialog.Show ();

			}catch(System.Exception e)
			{
				Log.Debug ("greska",e.ToString ());
			}




		}

	}
}


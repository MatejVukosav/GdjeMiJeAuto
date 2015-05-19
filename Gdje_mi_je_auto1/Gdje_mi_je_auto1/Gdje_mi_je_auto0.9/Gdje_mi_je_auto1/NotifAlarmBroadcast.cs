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
	[BroadcastReceiver]
	public class NotifAlarmBroadcast : BroadcastReceiver
	{
		private string rega;
		private string zona;
		Vibrator vibrator;


		public override void OnReceive (Context context, Intent intent)
		{
			Log.Debug ("bee", "1");
//			Alarms.playSound ();
			var builder = new AlertDialog.Builder (context);
			builder.SetTitle ("Istek parkinga");
			//builder.SetMessage (String.Format("Isteklo je vrijeme parkinga.", System.Environment.NewLine));
			Log.Debug ("bee", "2");
			var dialog1 = builder.Create ();
			builder.SetPositiveButton ("OK", delegate { 
				//		Alarms.stopSound();
				Log.Debug ("bee", "3");
				dialog1.Dismiss();
			});

			vibrator = (Vibrator)context.GetSystemService (Context.VibratorService);
			vibrator.Vibrate (500);
			Log.Debug ("bee", "4");

			Log.Debug ("bee", "5");
			dialog1.Show ();
			Log.Debug ("bee", "6");

		}
	}
}


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
	[BroadcastReceiver]
	public class NotifAlarmBroadcast : BroadcastReceiver
	{
		private string rega;
		private string zona;
		Vibrator vibrator;
		Dialog dialog;

		public override void OnReceive (Context context, Intent intent)
		{
			AlertDialog.Builder builder = new AlertDialog.Builder (context);
			builder.SetTitle ("Istek parkinga");
			builder.SetMessage (String.Format("Isteklo je vrijeme parkinga.", System.Environment.NewLine));
			builder.SetPositiveButton ("OK", delegate { dialog.Dismiss();
			});

			vibrator = (Vibrator)context.GetSystemService (Context.VibratorService);
			vibrator.Vibrate (500);

			dialog = builder.Create ();
			dialog.Show ();


		}
	}
}


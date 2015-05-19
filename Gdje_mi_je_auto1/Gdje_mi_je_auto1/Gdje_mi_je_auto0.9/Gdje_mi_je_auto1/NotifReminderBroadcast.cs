
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Util;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Gdje_mi_je_auto1
{
	[BroadcastReceiver]
	public class NotifReminderBroadcast : BroadcastReceiver
	{
		public override void OnReceive (Context context, Intent intent)
		{
			Log.Debug ("notif", "1");

			NotificationManager notifManager = (NotificationManager)Application.Context.GetSystemService (Context.NotificationService);

			var notIntent = new Intent (context, typeof(Alarms));
			var contentIntent = PendingIntent.GetActivity (context, 0, notIntent, PendingIntentFlags.CancelCurrent);

			Log.Debug ("notif", "2");
			var builder = new Notification.Builder (context)
				.SetContentTitle ("Istek roka")
				.SetContentText (String.Format ("Za {0} minuta će isteći vrijeme parkinga.", Alarms.mindif)) //TODO promijeniti klasu iz Alarms u onu od settingsa
				.SetDefaults (NotificationDefaults.All)
				.SetContentIntent (contentIntent);


			Log.Debug ("notif", "3");
			notifManager.Notify (Alarms.reqcode, builder.Build ());
			Log.Debug ("notif", "4");
			Toast.MakeText (Application.Context, "Podsjetnik!", ToastLength.Long).Show ();
		}
	}
}


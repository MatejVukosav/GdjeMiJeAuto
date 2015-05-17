
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
	public class NotifReminderBroadcast : BroadcastReceiver
	{
		public override void OnReceive (Context context, Intent intent)
		{
			var notifManager = NotificationManager.FromContext (context);

			var builder = new Notification.Builder (context)
				.SetAutoCancel (true)
				.SetContentTitle ("Istek roka")
				.SetContentText (String.Format ("Za {0} minuta će isteći vrijeme parkinga.", Alarms.mindif)) //TODO promijeniti klasu iz Alarms u onu od settingsa
				.SetDefaults(NotificationDefaults.All);

			var notification = builder.Build ();
			notifManager.Notify (0, notification);
		}
	}
}


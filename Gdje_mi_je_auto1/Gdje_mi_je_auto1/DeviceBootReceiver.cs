using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Java.Lang;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Gdje_mi_je_auto1
{
	[BroadcastReceiver]
	public class DeviceBootReceiver : BroadcastReceiver
	{
		public override void OnReceive (Context context, Intent intent)
		{
			if (intent.Action == "BOOT_COMPLETED")
			{
				Intent explicitIntent = new Intent(context, typeof(Alarms));
				context.StartActivity (explicitIntent);

			}
		}
	}
}


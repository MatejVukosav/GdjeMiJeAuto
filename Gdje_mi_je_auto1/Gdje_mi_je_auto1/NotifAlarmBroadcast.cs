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
		
		public override void OnReceive (Context context, Intent intent)
		{


			Intent intent2= new Intent(context, typeof(AlarmDialog));
			intent2.SetFlags(ActivityFlags.NewTask);
			context.StartActivity(intent2);
	


		}


	}
}


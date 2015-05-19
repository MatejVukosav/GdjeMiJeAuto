
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
	[Activity (Label = "Alarm",ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]		
	public class AlarmMain : Activity
	{
		ListView listview;

		List<string> listaAlarma = new List<string> ();

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.Alarm_Main);

			listview = FindViewById<ListView> (Resource.Id.AlarmList);


			//TODO napunit listu alarma. listaAlarma=

			BaseAdapter alarms=new BaseAdapterKlasa(this,listaAlarma.ToArray ());

			AlarmAdapter sectionAdapter = new AlarmAdapter(this);
			sectionAdapter.AddSection ("Uključeni alarmi",alarms);

			var prefs = Application.Context.GetSharedPreferences("MySharedPrefs", FileCreationMode.Private);
			var alarmEnabled=prefs.GetBoolean ("MyAlarmValue", true);

			if (alarmEnabled==false) {
				//TODO ugasi sve alarme
			} 

			listview.Adapter = sectionAdapter;
			listview.ItemClick += OnListItemClick;
		}

		void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
		{
			if (e.Position != 0) {
				var activityEEA = new Intent (this, typeof(EditExistingAlarm));
				StartActivity (activityEEA);
			}
		}
	}
}


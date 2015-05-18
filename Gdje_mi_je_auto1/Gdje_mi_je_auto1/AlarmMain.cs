
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
		const int AlarmNovi=0;
		const int Empty=1;
		List<string> listaAlarma = new List<string> ();

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.Alarm_Main);

			listview = FindViewById<ListView> (Resource.Id.AlarmList);

			String[] AlarmAdd=new String[1];

			//TODO napunit listu alarma. listaAlarma=

			BaseAdapter alarmMain = new BaseAdapterKlasa (this, AlarmAdd); //string[]
			BaseAdapter alarms=new BaseAdapterKlasa(this,listaAlarma.ToArray ());

			AlarmAdapter sectionAdapter = new AlarmAdapter(this);
			sectionAdapter.AddSection("Dodaj novi alarm  +" , alarmMain);
			sectionAdapter.AddSection ("Uključeni alarmi",alarms);

			listview.Adapter = sectionAdapter;
			listview.ItemClick += OnListItemClick;
		}

		void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
		{
			if (e.Position == AlarmNovi) {

				var activityANA = new Intent (this, typeof(AddNewAlarm));
				StartActivity (activityANA);
			}else if (e.Position == Empty) {
			//to je prazna linija radi lijepseg izgleda
			}else if (e.Position == 2) {
			//To je naslov Ukljuceni alarmi

			}else{

				var activityEEA = new Intent (this, typeof(EditExistingAlarm));
				StartActivity (activityEEA);
			}
		}
	}
}


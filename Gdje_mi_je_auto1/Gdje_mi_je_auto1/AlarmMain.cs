
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
		public static Dictionary<string, string> AlarmiDictionary=new Dictionary<string,string>();
		List<string> listaAlarma = new List<string> ();

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.Alarm_Main);

			listview = FindViewById<ListView> (Resource.Id.AlarmList);



			var prefsDict = Application.Context.GetSharedPreferences("MySharedPrefs", FileCreationMode.Private);
			String dictString = prefsDict.GetString("MyAktivniAlarmiPrefs", "");
			try{
			AlarmiDictionary = dictString.Split(',').Select(p => p.Trim()
				.Split(':'))
				.ToDictionary(p => p[0], p => p[1]);
			}catch(Exception){
			}

			foreach (KeyValuePair<string,string>pair in AlarmiDictionary) {
				String[] ureditAlarme = pair.Value.Split ('&');
				listaAlarma.Add("Zona: "+ureditAlarme[2]+" : "+ureditAlarme[0]+":"+ureditAlarme[1]);
			}



			BaseAdapter alarms=new BaseAdapterKlasa(this,listaAlarma.ToArray ());

			AlarmAdapter sectionAdapter = new AlarmAdapter(this);
			sectionAdapter.AddSection ("Uključeni alarmi",alarms);


			listview.Adapter = sectionAdapter;
			listview.ItemClick += OnListItemClick;
		}

	

		void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
		{
			if (e.Position != 0) {
				
				var activityEEA = new Intent (this, typeof(EditExistingAlarm));
				activityEEA.PutExtra ("idAlarma",e.Id);
				StartActivity (activityEEA);
			}
		}

	


	}
}


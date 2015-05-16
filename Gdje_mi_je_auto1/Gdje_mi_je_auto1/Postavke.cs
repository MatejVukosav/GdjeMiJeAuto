
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
	[Activity (Label = "Postavke" )]			
	public class Postavke : Activity
	{
		ListView listview;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.Settings_Main);

			listview = FindViewById<ListView> (Resource.Id.SettingsList);

			String[] map=new String[2];
			map[0]="Location update density";
			map[1]="Speed to switch between node";


			String[] pay=new String[2];
			pay[0]="Promijeni sliku registracije";
			pay[1]="Odaberi registraciju";

			String[] alarm=new String[3];
			alarm[0]="Odaberi zvuk alarma";
			alarm[1]="Odaberi zvuk podsjetnika";
			alarm[2]="Odaberi vrijeme podsjetnika";

			BaseAdapter mapB = new BaseAdapterKlasa (this, map);
			BaseAdapter payB = new BaseAdapterKlasa (this, pay);
			BaseAdapter alarmB = new BaseAdapterKlasa (this, alarm);


			SettingsAdapter sectionAdapter = new SettingsAdapter(this);
			sectionAdapter.AddSection("MAPA",   mapB);
			sectionAdapter.AddSection("PLAĆANJE",   payB);
			sectionAdapter.AddSection("ALARM",   alarmB);

			listview.Adapter = sectionAdapter;
		}
	}
}


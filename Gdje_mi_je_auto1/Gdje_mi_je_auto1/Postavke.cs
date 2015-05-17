
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
	[Activity (Label = "Postavke",Icon = "@drawable/Setting_icon" )]			
	public class Postavke : Activity
	{
		ListView listview;
		const int Mapa=0;
		const int Mapa0=1;
		const int Mapa1=2;
		const int Placanje=3;
		const int Placanje0=4;
		const int Placanje1=5;
		const int Placanje2=6;
		const int Alarm=7;
		const int Alarm0=8;
		const int Alarm1=9;
		const int Alarm2=10;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.Settings_Main);

			listview = FindViewById<ListView> (Resource.Id.SettingsList);


			String[] map=new String[2];
			map[0]="Brzina osvježavanja lokacije.";
			map[1]="Brzina prebacivanja postavki.";


			String[] pay=new String[3];
			pay[0]="Promijeni sliku registracije";
			pay[1]="Odaberi registraciju";
			pay[2]="Poruke iz inboxa";

			String[] alarm=new String[3];
			alarm[0]="Odaberi zvuk alarma";
			alarm[1]="Odaberi zvuk podsjetnika";
			alarm[2]="Odaberi vrijeme podsjetnika";

			BaseAdapter mapB = new BaseAdapterKlasa (this, map);
			BaseAdapter payB = new BaseAdapterKlasa (this, pay);
			BaseAdapter alarmB = new BaseAdapterKlasa (this, alarm);

//			ImageView ivM=FindViewById<ImageView> (Resource.Id.SettingsImage);	
//			ivM.SetImageResource (Resource.Drawable.ociscena_rega);
//			ImageView ivP=FindViewById<ImageView> (Resource.Id.SettingsImage);	
//			ivP.SetImageResource (Resource.Drawable.main_icon);
//
//			ImageView ivA=FindViewById<ImageView> (Resource.Id.SettingsImage);	
//			ivA.SetImageResource (Resource.Drawable.Icon);

			SettingsAdapter sectionAdapter = new SettingsAdapter(this);
			sectionAdapter.AddSection("Mapa", mapB);
			sectionAdapter.AddSection("Plaćanje",payB);
			sectionAdapter.AddSection("Alarm", alarmB);

			listview.Adapter = sectionAdapter;
			listview.ItemClick += OnListItemClick;
		}

		void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
		{
			if (e.Position == Mapa0) {
				var activityLUD = new Intent (this, typeof(LocationUpdateDensity));
				StartActivity (activityLUD);

			} else if (e.Position == Mapa1) {
				var activity_SBN = new Intent (this, typeof(SpeedBetweenNodes));
				StartActivity (activity_SBN);
			
			} else if (e.Position == Placanje0) {
				var activity_CRP = new Intent (this, typeof(ChangeRegistrationPicture));
				StartActivity (activity_CRP);

			} else if (e.Position == Placanje1) {
				var activity_CR = new Intent (this, typeof(ChooseRegistration));
				StartActivity (activity_CR);

			}else if (e.Position == Placanje2) {
				var activity_MFIS = new Intent (this, typeof(MessageFromInboxSettings));
				StartActivity (activity_MFIS);

			} else if (e.Position == Alarm0) {
				var activity_CAS = new Intent (this, typeof(ChooseAlarmSound));
				StartActivity (activity_CAS);

			} else if (e.Position == Alarm1) {
				var activity_CRS = new Intent (this, typeof(ChooseReminderSound));
				StartActivity (activity_CRS);

			} else if (e.Position == Alarm2) {
				var activity_CRT = new Intent (this, typeof(ChooseReminderTime));
				StartActivity (activity_CRT);

			} else {
				//throw new LayoutNotConnected ();
			}
				

		}




	}
}


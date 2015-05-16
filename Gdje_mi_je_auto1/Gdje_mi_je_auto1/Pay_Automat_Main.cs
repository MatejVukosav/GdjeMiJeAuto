﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Threading;
using Android.Util;
using System.IO;
using Android.Telephony;
using Android.Database;
using System.Windows;
using System.Runtime.InteropServices;


/*
 * Klasa u kojoj se sprema vrijeme plaćanja parking karte na automatu. 
 */
namespace Gdje_mi_je_auto1
{

	[Activity ( Label = "Automat",Icon = "@drawable/main_icon",NoHistory = true,ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]			
	public class Pay_Automat_Main : Activity
	{
		private Button btn_change_time;
		private EditText text_time_screen;
		private Button btn_save_time;
		//private string[] items;
		ListView listView;

		private int hour;
		private int minute;
		private bool valid_check_automat=false;

		ListViewAdapter ls = new ListViewAdapter ();

		private static List<string> spinnerList = new List<string> ();

		static String message_Path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
		String message_Data = System.IO.Path.Combine(message_Path, "Message_data.txt");

		protected override void  OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.Pay_Automat_Main_Enter);

			text_time_screen = FindViewById<EditText> (Resource.Id.edit_time_automat);
			btn_save_time = FindViewById<Button> (Resource.Id.btn_save_time);
			btn_change_time = FindViewById<Button> (Resource.Id.btn_change_time);

			listView = FindViewById<ListView> (Resource.Id.List_SMS_Main_History);

			Spinner spinnerPayA = FindViewById<Spinner> (Resource.Id.zoneSpinner);
			spinnerList=FillSpinnerWithData ();

			ArrayAdapter<string> spinnerArrayAdapter = new ArrayAdapter<string> (this, Android.Resource.Layout.SimpleSpinnerItem, spinnerList);
			spinnerArrayAdapter.SetDropDownViewResource (Android.Resource.Layout.SimpleSpinnerDropDownItem);
			spinnerPayA.Adapter = spinnerArrayAdapter;

			//TODO kad se klikne izvan dialoga, da se dialog ugasi
			

			#region PuniListu
			//Log.Debug ("ON CREATE","DULJINA"+new FileInfo(message_Data).Length+" -- "+Enable_message_update);
			long duljina=0;
			try{
				duljina = new FileInfo (message_Data).Length;
			}catch(Exception e){

				Log.Debug ("Pay_SMS_Main","FILE INFO krivo učitava "+e.ToString ());
			}
			Pay_SMS_Main psm = new Pay_SMS_Main ();

			if (duljina != 0 || Fill_ListView_With_Data.update_inbox_messages==true) {
				List<string> data = new List<string> ();
				String line;

				StreamReader reader = new StreamReader (message_Data);
				while ((line=reader.ReadLine ()) != null) {
					data.Add (line);
				}
				reader.Close ();
				Log.Debug ("Puni","puni listuuu");
				try{
					Fill_ListView_With_Data.FillListWithData (data,this,listView);
				}
				catch(NullReferenceException e){
					Log.Debug ("FillListWithData:Na pocetku Pay_SMS_Main",	e.ToString ());
				}
			}else{
				Fill_ListView_With_Data.DeleteHistory ();
			}

			//if user enabled Inbox messages
			if(Fill_ListView_With_Data.Enable_message_update==true){
				Fill_ListView_With_Data.Fill_With_Inbox_Data (this,listView);
			}
		
			#endregion

			listView.ItemClick += OnListItemClick;

			UpdateTime();
			UpdateDisplay ();

			TimePickerDialog time_dialog = new TimePickerDialog (this, TimePickerCallback, hour, minute, true);


			//Prikazuje trenutno vrijeme i nudi odabir promjene ili prekid.
			btn_change_time.Click += delegate {
				UpdateTime();
				time_dialog = new TimePickerDialog (this, TimePickerCallback, hour, minute, true);
				time_dialog.Show ();
				};
	
			/*
			 * UPALI ALARM I SPREMI VRIJEME PLACANJA U HISTORY
			 * */
			btn_save_time.Click+=delegate {
				//TODO klikom na save se pokrece alarm ako je upisano vrijeme i odabrana zona
				if(valid_check_automat){
					var activity_pay_main=new Intent (this,typeof(Pay_Main));
					StartActivity (activity_pay_main);
				}else 
					Toast.MakeText (this,"Odaberite zonu!",ToastLength.Short).Show ();
				};
				
	}
		



		public List<string> FillSpinnerWithData(){
			Dictionary<string,string> spinnerDict=new Dictionary<string,string>();
			List<string> zoneList = new List<string> ();
			try{
				var tuple=ParseZoneNumbers.LoadZoneNumbersAssetsData (this);
				spinnerDict = tuple.Item2; //zone [ ZONA 1 ],[ ZONA 2 ]...
			}catch(Exception e){
				Log.Debug ("Greska prilikom ucitavanja!",e.ToString ());
			}
			return spinnerDict.Values.ToList ();
		}



		private void UpdateDisplay ()
		{	
			string time = string.Format ("{0:D2}:{1:D2}", hour, minute.ToString ().PadLeft (2, '0'));
			text_time_screen.Text = time;
		}

		private void TimePickerCallback (object sender, TimePickerDialog.TimeSetEventArgs e)
		{
			hour = e.HourOfDay;
			minute = e.Minute;
			UpdateDisplay ();
		}

		private void UpdateTime(){
			hour = DateTime.Now.Hour;
			minute = DateTime.Now.Minute;
		}



		/*
		 * Nakon klika trebao bi se otvorit fragment samo s popisom, history.
		 * */
		void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
		{
			//TODO onListItemClick must be defined
			/*
			var listview = sender as ListView;
			FragmentTransaction ftr = FragmentManager.BeginTransaction ();
			try{
				ShowDialog dialog = new ShowDialog (listview.Id );
				dialog.Show (ftr, "dinamooo");
			}catch(NullReferenceException e1){
				Log.Debug("ouuuuuuuuuuuuu",e1.ToString ());
			}

			//Android.Widget.Toast.MakeText (this,"ispisaoaooa",Android.Widget.ToastLength.Short).Show ();
			*/
		}
	


	}
}



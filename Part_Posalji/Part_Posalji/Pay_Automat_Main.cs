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
using System.Threading;
using Android.Util;

/*
 * Klasa u kojoj se sprema vrijeme plaćanja parking karte na automatu. 
 */
namespace Part_Posalji
{

	[Activity ( Label = "Automat",Icon = "@drawable/main_icon",NoHistory = true)]			
	public class Pay_Automat_Main : Activity
	{
		private Button btn_change_time;
		private EditText text_time_screen;
		private Button btn_save_time;

		private int hour;
		private int minute;

		ListViewAdapter ls = new ListViewAdapter ();
		ListView listview;
		string[] items;



		protected override void  OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.Pay_Automat_Main_Enter);

			text_time_screen = FindViewById<EditText> (Resource.Id.edit_time_automat);
			btn_save_time = FindViewById<Button> (Resource.Id.btn_save_time);
			btn_change_time = FindViewById<Button> (Resource.Id.btn_change_time);


			items = List_Data.List_Data_Fill ();

			listview = FindViewById<ListView> (Resource.Id.List_Automat_History);
			listview.Adapter = new ListViewAdapter (this,items);
			listview.ItemClick += OnListItemClick;


			UpdateTime();
			UpdateDisplay ();

			TimePickerDialog time_dialog = new TimePickerDialog (this, TimePickerCallback, hour, minute, true);


			//Prikazuje trenutno vrijeme i nudi odabir promjene ili prekid.
			btn_change_time.Click += delegate {
				UpdateTime();
				time_dialog = new TimePickerDialog (this, TimePickerCallback, hour, minute, true);
				time_dialog.Show ();
				};
	

			//UPALI ALARM I SPREMI VRIJEME PLACANJA U HISTORY
			btn_save_time.Click+=delegate {
				var activity_pay_main=new Intent (this,typeof(Pay_Main));
				StartActivity (activity_pay_main);
				};
				
		}



		private void UpdateDisplay ()
		{	
			string time = string.Format ("{0}:{1}", hour, minute.ToString ().PadLeft (2, '0'));
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
//			var listview = sender as ListView;
//			FragmentTransaction ftr = FragmentManager.BeginTransaction ();
//			try{
//				ShowDialog dialog = new ShowDialog (listview.Id );
//				dialog.Show (ftr, "dinamooo");
//			}catch(NullReferenceException e1){
//				Log.Debug("ouuuuuuuuuuuuu","null");
//			}

			//Android.Widget.Toast.MakeText (this,"ispisaoaooa",Android.Widget.ToastLength.Short).Show ();
		}
	

	}
}



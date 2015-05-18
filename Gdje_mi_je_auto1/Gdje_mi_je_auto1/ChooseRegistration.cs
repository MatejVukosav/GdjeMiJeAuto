
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
	[Activity (NoHistory = true,ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]			
	public class ChooseRegistration : Activity
	{
		List<string> regText = new List<string> ();
		ListView listview;
		static bool yes=false;

		ISharedPreferences prefs = Application.Context.GetSharedPreferences("MySharedPrefs", FileCreationMode.Private);
		ISharedPreferencesEditor prefsEditor;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.ChooseRegistration);
			listview = FindViewById<ListView> (Resource.Id.Choose_Registration_List);

			prefsEditor = prefs.Edit ();
			ICollection<string> regColl;
			try{
				regColl=prefs.GetStringSet("MyRegistrationTextPrefs", null);
				foreach (String reg in regColl) {
					//Log.Debug ("regtext",reg);
					regText.Add (reg);
				}
			}catch(Exception e){
				Log.Debug ("Uitavanje registracijske oznake neuspijelo.",e.ToString ());
			}
			
			try{
				listview.Adapter = new BaseAdapterKlasa (this, regText.ToArray ());
				listview.ItemClick += OnListItemClick;
				listview.ItemLongClick += OnLongListItemClick;

			}catch(Exception e){
				Log.Debug ("Nije upisana nijedna registracija.",e.ToString ());				
			}



		}

		void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
		{
			var t = regText [e.Position];
			String registration = t;

			prefsEditor.PutString ("MyRegistrationDefaultPrefs", registration);
			prefsEditor.Commit ();
			var activity_P = new Intent (this, typeof(Postavke));
			StartActivity (activity_P);

		}

		void OnLongListItemClick(object sender, AdapterView.ItemLongClickEventArgs e)
		{
			yes=false;
			CreateAddProjectDialog(sender,e);
			if (yes) {
				var t = regText [e.Position];
				regText.Remove (t);
				prefsEditor.Remove ("MyRegistrationTextPrefs").Commit ();
				prefsEditor.PutStringSet ("MyRegistrationTextPrefs", regText).Commit ();

				if (t.Equals (prefs.GetString ("MyRegistrationDefaultPrefs", ""))) {
					prefsEditor.Remove ("MyRegistrationDefaultPrefs").Commit ();
				}
			}
		}

		private void CreateAddProjectDialog(object sender,AdapterView.ItemLongClickEventArgs e) { 
			var alert = new AlertDialog.Builder (this);
			alert.SetTitle ("Želite li izbrisati spremljenu registraciju?");
			//alert.SetView (layoutProperties);
			alert.SetCancelable (false);
			alert.SetPositiveButton("Da", HandlePositiveButtonClick);
			alert.SetNegativeButton("Ne", HandelNegativeButtonClick);
			var dialog = alert.Create();
			dialog.Show();
		}

		private void HandlePositiveButtonClick (object sender, EventArgs e) {
			yes = true;
		}

		private void HandelNegativeButtonClick (object sender, EventArgs e) {
			yes = false;
		}

	}
}


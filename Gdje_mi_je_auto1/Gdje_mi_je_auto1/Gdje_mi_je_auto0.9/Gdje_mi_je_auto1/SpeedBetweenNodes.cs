
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Gdje_mi_je_auto1
{
	[Activity (Label = "SpeedBetweenNodes",NoHistory = true)]			
	public class SpeedBetweenNodes : Activity
	{
		private Spinner speedSpinner;
		private string settingsFile;
		private SettingsRecord settings;
		private Button btnApply;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.SpeedBetweenNodes);

			settingsFile = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
			settingsFile = Path.Combine(settingsFile, "settings.txt");
			settings = JsonConvert.DeserializeObject<SettingsRecord> (File.ReadLines (settingsFile).First());

			speedSpinner = FindViewById<Spinner> (Resource.Id.speedSpinner);
			speedSpinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs> (speedSpinner_ItemSelected);
			var adapter = ArrayAdapter.CreateFromResource (this, Resource.Array.speed_array, Android.Resource.Layout.SimpleSpinnerItem);
			adapter.SetDropDownViewResource (Android.Resource.Layout.SimpleSpinnerDropDownItem);
			speedSpinner.Adapter = adapter;
			speedSpinner_Initialize ();

			btnApply = FindViewById<Button>(Resource.Id.btnApply2);
			btnApply.Click += delegate {
				if (settingsChanged()){
					using (TextWriter tw = new StreamWriter(settingsFile,false)){
						tw.WriteLine (JsonConvert.SerializeObject (settings));
					}
					if(isLoggerRunning()){
						restartLogger();
					}
				}
				Finish();
			};
		}

		//na temelju odabira mijenjaj postavke
		private void speedSpinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e){
			Spinner spinner = (Spinner)sender;

			switch (spinner.GetItemAtPosition (e.Position).ToString()) {
			case "10 kmh":
				settings.speedLimit = 10 / 3.6;
				break;
			case "15 kmh":
				settings.speedLimit = 15 / 3.6;
				break;
			case "30 kmh":
				settings.speedLimit = 30 / 3.6;
				break;
			case "40 kmh":
				settings.speedLimit = 40 / 3.6;
				break;
			case "50 kmh":
				settings.speedLimit = 50 / 3.6;
				break;
			}
		}

		//postavi početnu vrijednost spinera na onu iz fajla
		private void speedSpinner_Initialize(){
			if (settings.speedLimit == 10 / 3.6) 
				speedSpinner.SetSelection (0);
			else if (settings.speedLimit == 15 / 3.6)
				speedSpinner.SetSelection (1);
			else if (settings.speedLimit == 30 / 3.6)
				speedSpinner.SetSelection (2);
			else if (settings.speedLimit == 40 / 3.6)
				speedSpinner.SetSelection (3);
			else if (settings.speedLimit == 50 / 3.6)
				speedSpinner.SetSelection (4);
		}

		private bool settingsChanged(){
			SettingsRecord oldSettings = JsonConvert.DeserializeObject<SettingsRecord> (File.ReadLines (settingsFile).First());

			if (settings.speedLimit != oldSettings.speedLimit)
				return true;
			else if (settings.timeBetweenUpdates != oldSettings.timeBetweenUpdates)
				return true;
			else if (settings.timeBetweenUpdatesDense != oldSettings.timeBetweenUpdatesDense)
				return true;
			else
				return false;
		}

		public void restartLogger(){
			StopService (new Intent (this, typeof(LocationLogger)));
			StartService (new Intent (this, typeof(LocationLogger)));
		}

		private bool isLoggerRunning(){
			var manager = (ActivityManager)GetSystemService (ActivityService);
			List<string> services = manager.GetRunningServices (int.MaxValue).Select (service => service.Service.ClassName.ToLower()).ToList ();
			foreach (string service in services) {
				//if (service == typeof(LocationLogger).ToString().ToLower())
				if (service.EndsWith(".locationlogger"))
					return true;
			}
			return false;
		}
	}


}


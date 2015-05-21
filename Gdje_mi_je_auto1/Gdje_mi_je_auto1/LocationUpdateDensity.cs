
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using Newtonsoft.Json;

namespace Gdje_mi_je_auto1
{
	[Activity (NoHistory = true)]			
	public class LocationUpdateDensity : Activity

	{
		private Spinner normalSpinner;
		private Spinner denseSpinner;
		private Button btnApply;
		private Button toggleButton;
		private SettingsRecord settings;
		private string settingsFile;

		private const int MINUTE = 1000 * 60;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.LocationUpdateDensity);

			settingsFile = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
			settingsFile = Path.Combine(settingsFile, "settings.txt");
			settings = JsonConvert.DeserializeObject<SettingsRecord> (File.ReadLines (settingsFile).First());

			//ZA DEBUG ISPIS LOCATIONS FILEA XAMARINU
			//string locationsFile;
			//locationsFile = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
			//locationsFile = Path.Combine(locationsFile, "locations.txt");
			//foreach (string location in File.ReadLines(locationsFile)) {
			//	Console.WriteLine (location);
			//}
			//ZAKOMENTIRATI

			toggleButton = FindViewById<Button>(Resource.Id.btnUseLogging);
			toggleButton.Text = isLoggerRunning ()?"Zapisivanje lokacija je uključeno":"Zapisivanje lokacija je isključeno";

			toggleButton.Click += delegate {
				if (toggleButton.Text == "Zapisivanje lokacija je isključeno"){
					//pokreni snimanje lokaija ako već nije i updateaj settings file
					if(!isLoggerRunning())
						StartService (new Intent (this, typeof(LocationLogger)));
					SettingsRecord currentSettings = JsonConvert.DeserializeObject<SettingsRecord> (File.ReadLines (settingsFile).First());
					currentSettings.useLocationLogging = true;
					using (TextWriter tw = new StreamWriter(settingsFile,false)){
						tw.WriteLine (JsonConvert.SerializeObject (currentSettings));
					}
					toggleButton.Text = "Zapisivanje lokacija je uključeno";
				} else {
					//zaustavi pozadinsko zapisivanje i updateaj settings file
					StopService (new Intent (this, typeof(LocationLogger)));
					SettingsRecord currentSettings = JsonConvert.DeserializeObject<SettingsRecord> (File.ReadLines (settingsFile).First());
					currentSettings.useLocationLogging = false;
					using (TextWriter tw = new StreamWriter(settingsFile,false)){
						tw.WriteLine (JsonConvert.SerializeObject (currentSettings));
					}
					toggleButton.Text = "Zapisivanje lokacija je isključeno";
				}
			};

			normalSpinner = FindViewById<Spinner> (Resource.Id.normalSpinner);
			normalSpinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs> (normalSpinner_ItemSelected);
			var adapter = ArrayAdapter.CreateFromResource (this, Resource.Array.normal_density_array, Android.Resource.Layout.SimpleSpinnerItem);
			adapter.SetDropDownViewResource (Android.Resource.Layout.SimpleSpinnerDropDownItem);
			normalSpinner.Adapter = adapter;
			normalSpinner_Initialize ();

			denseSpinner = FindViewById<Spinner> (Resource.Id.denseSpinner);
			denseSpinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs> (denseSpinner_ItemSelected);
			adapter = ArrayAdapter.CreateFromResource (this, Resource.Array.high_density_array, Android.Resource.Layout.SimpleSpinnerItem);
			adapter.SetDropDownViewResource (Android.Resource.Layout.SimpleSpinnerDropDownItem);
			denseSpinner.Adapter = adapter;
			denseSpinner_Initialize ();

			btnApply = FindViewById<Button>(Resource.Id.btnApply1);
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
		private void normalSpinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e){
			Spinner spinner = (Spinner)sender;

			switch (spinner.GetItemAtPosition (e.Position).ToString()) {
			case "Rijetko":
				settings.timeBetweenUpdates = MINUTE * 10;
				break;
			case "Srednje":
				settings.timeBetweenUpdates = MINUTE * 4;
				break;
			case "Gusto":
				settings.timeBetweenUpdates = MINUTE * 1;
				break;
			}

		}

		//na temelju odabira mijenjaj postavke
		private void denseSpinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e){
			Spinner spinner = (Spinner)sender;

			switch (spinner.GetItemAtPosition (e.Position).ToString()) {
			case "Rijetko":
				settings.timeBetweenUpdatesDense = MINUTE;
				break;
			case "Srednje":
				settings.timeBetweenUpdatesDense = MINUTE/2;
				break;
			case "Gusto":
				settings.timeBetweenUpdatesDense = MINUTE/4;
				break;
			}
		}

		//postavi početnu vrijednost spinera na onu iz fajla
		private void normalSpinner_Initialize(){
			switch (settings.timeBetweenUpdates) {
			case MINUTE * 10:
				normalSpinner.SetSelection (0);
				//Toast.MakeText (this, "inicijaliziram normal na low", ToastLength.Long).Show ();
				break;
			case MINUTE * 4:
				normalSpinner.SetSelection (1);
				//Toast.MakeText (this, "inicijaliziram normal na medium", ToastLength.Long).Show ();
				break;
			case MINUTE * 1:
				normalSpinner.SetSelection (2);
				//Toast.MakeText (this, "inicijaliziram normal na high", ToastLength.Long).Show ();
				break;
			}
		}

		//postavi početnu vrijednost spinera na onu iz fajla
		private void denseSpinner_Initialize(){
			switch (settings.timeBetweenUpdatesDense) {
			case MINUTE:
				denseSpinner.SetSelection (0);
				//Toast.MakeText (this, "inicijaliziram dense na low", ToastLength.Long).Show ();
				break;
			case MINUTE / 2:
				denseSpinner.SetSelection (1);
				//Toast.MakeText (this, "inicijaliziram dense na medium", ToastLength.Long).Show ();
				break;
			case MINUTE / 4:
				denseSpinner.SetSelection (2);
				//Toast.MakeText (this, "inicijaliziram dense na high", ToastLength.Long).Show ();
				break;
			}
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


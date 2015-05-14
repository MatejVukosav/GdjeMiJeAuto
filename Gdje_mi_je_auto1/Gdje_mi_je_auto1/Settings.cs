using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using Android.App;
using Android.Content;
using Android.Gms.Common;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using Android.Content.PM;
using System.Threading;
using System.Threading.Tasks;


namespace Gdje_mi_je_auto1
{
	[Activity (Label = "Map & Location", Icon = "@drawable/icon",ScreenOrientation = ScreenOrientation.Portrait)]
//	public class Main : Activity, ILocationListener
	public class Settings : Activity
	{
//		private string locationsFile;
		private string settingsFile;

//		private LocationManager locMgr;
//		private Button btnMap;
		private Button btnLocations;
		private Button btnSettings;
		private Button btnApply;
//		private Button btnStartUpdates;
//		private Button btnStopUpdates;
//		private Button btnGetZone;
//		private TextView textStatus;
		private ToggleButton togglebutton;
		private SettingsRecord settings;
//		private TextView textTime;
//		private TextView textProvider;
//		private TextView textLatitude;
//		private TextView textLongitude;
//		private TextView textSpeed;
//		private TextView textZone;
		private Spinner normalSpinner;
		private Spinner denseSpinner;
		private Spinner speedSpinner;
//		private string chosenProvider = LocationManager.GpsProvider;

		private const int MINUTE = 1000 * 60;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.MainLayout);

//			locMgr = GetSystemService (Context.LocationService) as LocationManager;
			//GeoFencer.Initialize(this);

//			locationsFile = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
//			locationsFile = Path.Combine(locationsFile, "locations.txt");

			settingsFile = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
			settingsFile = Path.Combine(settingsFile, "settings.txt");
			settings = JsonConvert.DeserializeObject<SettingsRecord> (File.ReadLines (settingsFile).First());

//			textTime = FindViewById<TextView> (Resource.Id.mainTextTime);
//			textProvider = FindViewById<TextView> (Resource.Id.mainTextProvider);
//			textLatitude = FindViewById<TextView> (Resource.Id.mainTextLatitude);
//			textLongitude = FindViewById<TextView> (Resource.Id.mainTextLongitude);
//			textSpeed = FindViewById<TextView> (Resource.Id.mainTextSpeed);
//			textZone = FindViewById<TextView> (Resource.Id.mainTextZone);

			//kod pokretanja provjeri dali je servis uključen ili ne
//			textStatus = FindViewById<TextView> (Resource.Id.mainTextStatus);
//			textStatus.Text = isLoggerRunning () ? "Location updates are on" : "Location updates are off";

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

			speedSpinner = FindViewById<Spinner> (Resource.Id.speedSpinner);
			speedSpinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs> (speedSpinner_ItemSelected);
			adapter = ArrayAdapter.CreateFromResource (this, Resource.Array.speed_array, Android.Resource.Layout.SimpleSpinnerItem);
			adapter.SetDropDownViewResource (Android.Resource.Layout.SimpleSpinnerDropDownItem);
			speedSpinner.Adapter = adapter;
			speedSpinner_Initialize ();

//			btnMap = FindViewById<Button>(Resource.Id.mainMap);
//			btnMap.Click += delegate {
//				StartActivity(typeof(MapActivity));
//			};

			btnLocations = FindViewById<Button>(Resource.Id.mainLocation);
			btnLocations.Click += delegate {
				StartActivity(typeof(LocationsActivity));
			};

			btnSettings = FindViewById<Button>(Resource.Id.mainSettings);
			btnSettings.Click += delegate {
				StartActivity(typeof(SeeSettings));
			};

			btnApply = FindViewById<Button>(Resource.Id.applyButton);
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

			//Kad stisne gumb pokreni pozadinsko zapisivanje ako već nije pokrenuto
//			btnStartUpdates = FindViewById<Button>(Resource.Id.mainStartLocationUpdates);
//			btnStartUpdates.Click += delegate {
//				if (locMgr.AllProviders.Contains (chosenProvider) && locMgr.IsProviderEnabled (chosenProvider)) {
//					locMgr.RequestLocationUpdates (chosenProvider, UPDATE_INTERVAL , MIN_METERS, this);
//					textStatus.Text = "Location updates on";
//					textProvider.Text = "Provider: "+chosenProvider;

//				} else {
//					textStatus.Text = "Can't start location updates";
//				}
//				if(!isLoggerRunning())
//					StartService (new Intent (this, typeof(LocationLogger)));
//				textStatus.Text = isLoggerRunning () ? "Location updates on" : "Location updates off";
//				//textProvider.Text = "Provider: "+chosenProvider;
//			};

//			btnGetZone = FindViewById<Button>(Resource.Id.mainGetZone);
//			btnGetZone.Click += delegate {
//				textZone.Text = GeoFencer.inZone();
//			};

			//Kad stisne gumb zaustavi pozadinsko zapisivanje
//			btnStopUpdates = FindViewById<Button>(Resource.Id.mainStopLocationUpdates);
//			btnStopUpdates.Click += delegate{
//				locMgr.RemoveUpdates (this);
				//textProvider.Text = "Provider: ";
//				textStatus.Text = "Location updates off";
//				StopService (new Intent (this, typeof(LocationLogger)));
//			};

			togglebutton = FindViewById<ToggleButton>(Resource.Id.toggleLogging);
			togglebutton.Checked = isLoggerRunning ();

			togglebutton.Click += delegate {
				if (togglebutton.Checked){
					//pokreni snimanje lokaija ako već nije i updateaj settings file
					if(!isLoggerRunning())
						StartService (new Intent (this, typeof(LocationLogger)));
					SettingsRecord currentSettings = JsonConvert.DeserializeObject<SettingsRecord> (File.ReadLines (settingsFile).First());
					currentSettings.useLocationLogging = true;
					using (TextWriter tw = new StreamWriter(settingsFile,false)){
						tw.WriteLine (JsonConvert.SerializeObject (currentSettings));
					}
				} else {
					//zaustavi pozadinsko zapisivanje i updateaj settings file
					StopService (new Intent (this, typeof(LocationLogger)));
					SettingsRecord currentSettings = JsonConvert.DeserializeObject<SettingsRecord> (File.ReadLines (settingsFile).First());
					currentSettings.useLocationLogging = false;
					using (TextWriter tw = new StreamWriter(settingsFile,false)){
						tw.WriteLine (JsonConvert.SerializeObject (currentSettings));
					}
				}
			};
				
		}

		//Provjeri dali radi pozadinsko zapisivanje lokacije
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
			
		//na temelju odabira mijenjaj postavke
		private void normalSpinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e){
			Spinner spinner = (Spinner)sender;

			switch (spinner.GetItemAtPosition (e.Position).ToString()) {
			case "Low":
				settings.timeBetweenUpdates = MINUTE * 10;
				break;
			case "Medium":
				settings.timeBetweenUpdates = MINUTE * 4;
				break;
			case "High":
				settings.timeBetweenUpdates = MINUTE * 1;
				break;
			}

		}

		//na temelju odabira mijenjaj postavke
		private void denseSpinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e){
			Spinner spinner = (Spinner)sender;

			switch (spinner.GetItemAtPosition (e.Position).ToString()) {
			case "Low":
				settings.timeBetweenUpdatesDense = MINUTE;
				break;
			case "Medium":
				settings.timeBetweenUpdatesDense = MINUTE/2;
				break;
			case "High":
				settings.timeBetweenUpdatesDense = MINUTE/4;
				break;
			}
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

		//rovjerava dali su se promjenile postavke od sadašnjih
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
	}
		
}


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
	public class GPS_Main : Activity
	{
		private string locationsFile;

//		private LocationManager locMgr;
		private Button btnMap;
		private Button btnLocations;
		private Button btnStartUpdates;
		private Button btnStopUpdates;
		private TextView textStatus;
		private TextView textTime;
		private TextView textProvider;
		private TextView textLatitude;
		private TextView textLongitude;
		private TextView textSpeed;
		private TextView textZone;
		private Spinner spinner;
		private string chosenProvider = LocationManager.GpsProvider;

		private const int MINUTE = 1000 * 60;
		private const int MIN_METERS = 5;
		private const int UPDATE_INTERVAL = 1* MINUTE;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.MainLayout);

//			locMgr = GetSystemService (Context.LocationService) as LocationManager;

			locationsFile = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
			locationsFile = Path.Combine(locationsFile, "locations.txt");

			textTime = FindViewById<TextView> (Resource.Id.mainTextTime);
			textProvider = FindViewById<TextView> (Resource.Id.mainTextProvider);
			textLatitude = FindViewById<TextView> (Resource.Id.mainTextLatitude);
			textLongitude = FindViewById<TextView> (Resource.Id.mainTextLongitude);
			textSpeed = FindViewById<TextView> (Resource.Id.mainTextSpeed);
			textZone = FindViewById<TextView> (Resource.Id.mainTextZone);

			//kod pokretanja provjeri dali je servis uključen ili ne
			textStatus = FindViewById<TextView> (Resource.Id.mainTextStatus);
			textStatus.Text = isLoggerRunning () ? "Location updates on" : "Location updates off";

			spinner = FindViewById<Spinner> (Resource.Id.mainSpinner);
			spinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs> (spinner_ItemSelected);
			var adapter = ArrayAdapter.CreateFromResource (this, Resource.Array.providers_array, Android.Resource.Layout.SimpleSpinnerItem);
			adapter.SetDropDownViewResource (Android.Resource.Layout.SimpleSpinnerDropDownItem);
			spinner.Adapter = adapter;

			btnMap = FindViewById<Button>(Resource.Id.mainMap);
			btnMap.Click += delegate {
				StartActivity(typeof(MapActivity));
			};

			btnLocations = FindViewById<Button>(Resource.Id.mainLocation);
			btnLocations.Click += delegate {
				StartActivity(typeof(LocationsActivity));
			};

			//Kad stisne gumb pokreni pozadinsko zapisivanje ako već nije pokrenuto
			btnStartUpdates = FindViewById<Button>(Resource.Id.mainStartLocationUpdates);
			btnStartUpdates.Click += delegate {
//				if (locMgr.AllProviders.Contains (chosenProvider) && locMgr.IsProviderEnabled (chosenProvider)) {
//					locMgr.RequestLocationUpdates (chosenProvider, UPDATE_INTERVAL , MIN_METERS, this);
//					textStatus.Text = "Location updates on";
//					textProvider.Text = "Provider: "+chosenProvider;

//				} else {
//					textStatus.Text = "Can't start location updates";
//				}
				if(!isLoggerRunning())
					StartService (new Intent (this, typeof(LocationLogger)));
				textStatus.Text = isLoggerRunning () ? "Location updates on" : "Location updates off";
				textProvider.Text = "Provider: "+chosenProvider;
			};

			//Kad stisne gumb zaustavi pozadinsko zapisivanje
			btnStopUpdates = FindViewById<Button>(Resource.Id.mainStopLocationUpdates);
			btnStopUpdates.Click += delegate{
//				locMgr.RemoveUpdates (this);
				textProvider.Text = "Provider: ";
				textStatus.Text = isLoggerRunning () ? "Location updates on" : "Location updates off";
				StopService (new Intent (this, typeof(LocationLogger)));
			};
				
		}
			
//		public void OnLocationChanged (Location location)
//		{
//			textLatitude.Text = "Latitude: " + location.Latitude.ToString();
//			textLongitude.Text = "Longitude: " + location.Longitude.ToString();
//			textProvider.Text = "Provider: " + location.Provider.ToString();
//			if (location.HasSpeed) {
//				textSpeed.Text = "Speed: " + location.Speed;
//			} else {
//				textSpeed.Text = "Speed not available";
//			}
//			textTime.Text = "Last update: " + DateTime.Now.ToLocalTime();
//
//			int polygon =  GeoFencer.inZone (location.Latitude, location.Longitude);
//
//			textZone.Text = "Polygon " + polygon + "\nZone " + GeoFencer.zoneName(polygon);
//
//			StoreLocation (location);
//		}
//
//		void StoreLocation (Location location)
//		{
//			using (StreamWriter sw = File.AppendText (locationsFile)) {
//				LocationRecord record = new LocationRecord {
//					time = DateTime.Now.ToLocalTime(),
//					latitude = location.Latitude,
//					longitude = location.Longitude,
//					provider = location.Provider,
//					recordedSpeed = location.HasSpeed ? location.Speed : double.NaN,
//					calculatedSpeed = 0,
//					unixTime = location.Time
//				};
//				sw.WriteLine (JsonConvert.SerializeObject (record));
//			}
//
//		}
//
//		public void OnProviderDisabled (string provider)
//		{
//		}
//		public void OnProviderEnabled (string provider)
//		{
//		}
//		public void OnStatusChanged (string provider, Availability status, Bundle extras)
//		{
//		}

		//prati koji provider je odabran
		private void spinner_ItemSelected (object sender, AdapterView.ItemSelectedEventArgs e)
		{
			Spinner ProviderSpinner = (Spinner)sender;
			string Provider = (string) ProviderSpinner.GetItemAtPosition (e.Position);

			switch (Provider) {
			case "GPS":
				chosenProvider = LocationManager.GpsProvider;
				break;
			case "Network":
				chosenProvider = LocationManager.NetworkProvider;
				break;
			default:
				chosenProvider = LocationManager.GpsProvider;
				break;
			}
		}

		//Provjeri dali radi pozadinsko zapisivanje lokacije
		private bool isLoggerRunning(){
			var manager = (ActivityManager)GetSystemService (ActivityService);
			List<string> services = manager.GetRunningServices (int.MaxValue).Select (service => service.Service.ClassName.ToLower()).ToList ();
			foreach (string service in services) {
				if (service == typeof(LocationLogger).ToString().ToLower())
					return true;
			}
			return false;
		}
	}
		
}


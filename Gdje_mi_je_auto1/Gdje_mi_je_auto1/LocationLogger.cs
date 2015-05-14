using System;
using System.IO;
using System.Collections;
using Android.App;
using Android.Content;
using Android.Locations;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Gdje_mi_je_auto1
{
	[Service]
	public class LocationLogger : Service , ILocationListener
	{
		private LocationManager locMgr;
		private string locationsFile;
		private string settingsFile;
		private LocationRecord lastRecord = null;
		//private const int timeBetweenUpdates = 5*1000*60;
		private int timeBetweenUpdates = 4*1000*60; //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
		private int timeBetweenUpdatesDense = 500*60;
		private double speedLimit = 15 / 3.6;
		private bool denseUpdates = false;
		private int slowMesurement = 0;


		public override void OnStart (Android.Content.Intent intent, int startId)
		{
			base.OnStart (intent, startId);
			//base.OnStartCommand(intent,StartCommandFlags.Redelivery,startId);

			settingsFile = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
			settingsFile = Path.Combine(settingsFile, "settings.txt");

			if (File.Exists (settingsFile)) {
				SettingsRecord settings =  JsonConvert.DeserializeObject<SettingsRecord>( File.ReadAllText(settingsFile));
				timeBetweenUpdates = settings.timeBetweenUpdates;
				timeBetweenUpdatesDense = settings.timeBetweenUpdatesDense;
				speedLimit = settings.speedLimit;
			}

			locMgr = GetSystemService (Context.LocationService) as LocationManager;
			locMgr.RequestLocationUpdates (LocationManager.NetworkProvider, timeBetweenUpdates , 0, this);

			locationsFile = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
			locationsFile = Path.Combine(locationsFile, "locations.txt");

			if (File.Exists (locationsFile)) {
				List<string> lines = new List<string> (File.ReadLines (locationsFile));
				if (lines.Count > 0) {
					lastRecord = JsonConvert.DeserializeObject<LocationRecord> (lines [lines.Count - 1]);
				}
			}
		}

		public override void OnDestroy ()
		{
			locMgr.RemoveUpdates (this);
			base.OnDestroy ();    
		}
			
		public override Android.OS.IBinder OnBind (Android.Content.Intent intent)
		{
			throw new Exception();
		}

		public void OnLocationChanged (Location location)
		{
			StreamWriter sw;
			if (File.Exists (locationsFile)) {
				sw = File.AppendText (locationsFile);
			} else {
				sw = new StreamWriter(File.Create (locationsFile));
			}
			
			//using (StreamWriter sw = File.AppendText (locationsFile)) {
				LocationRecord record = new LocationRecord {
					time = DateTime.Now.ToLocalTime(),
					latitude = location.Latitude,
					longitude = location.Longitude,
					provider = location.Provider,
					recordedSpeed = location.HasSpeed ? location.Speed : double.NaN,
					calculatedSpeed = lastRecord != null? calculateSpeed(location.Time,location.Latitude,location.Longitude) : 0,
					unixTime = location.Time
				};

				lastRecord = record;
				sw.WriteLine (JsonConvert.SerializeObject (record));

				if (record.calculatedSpeed > speedLimit && !denseUpdates) {
					locMgr.RemoveUpdates (this);
					denseUpdates = true;
					locMgr.RequestLocationUpdates (LocationManager.GpsProvider, timeBetweenUpdatesDense , 0, this);
				}

				if (record.calculatedSpeed < speedLimit && record.recordedSpeed < speedLimit && denseUpdates) {
					slowMesurement += 1;
					if(slowMesurement > 9){
						locMgr.RemoveUpdates (this);
						denseUpdates = false;
						locMgr.RequestLocationUpdates (LocationManager.NetworkProvider, timeBetweenUpdates , 0, this);
					}
				}

			sw.Close ();
			
			//}
		}

		double calculateSpeed (long unixTime, double latitude, double longitude)
		{
			float distance;
			Location loc1 = new Location (LocationManager.NetworkProvider);
			Location loc2 = new Location (LocationManager.NetworkProvider);

			loc1.Latitude = latitude;
			loc1.Longitude = longitude;

			loc2.Latitude =lastRecord.latitude;
			loc2.Longitude =lastRecord.longitude;

			distance = loc1.DistanceTo (loc2);

			return (distance * 1000 / (unixTime - lastRecord.unixTime));
		}

		public void OnProviderDisabled (string provider)
		{
		}

		public void OnProviderEnabled (string provider)
		{
		}

		public void OnStatusChanged (string provider, Availability status, Android.OS.Bundle extras)
		{
		}
	}
}


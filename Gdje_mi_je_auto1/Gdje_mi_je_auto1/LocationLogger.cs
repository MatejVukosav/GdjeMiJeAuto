﻿using System;
using System.IO;
using System.Collections;
using Android.App;
using Android.Content;
using Android.Locations;
using Newtonsoft.Json;
using System.Collections.Generic;
using Android.Util;

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
		private bool sameProviders = true;
		private int maxLineCount = 30000;

		private long timeDifference;

		[Obsolete("Method is deprecated.", false)]
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

//				if (lines.Count >= maxLineCount) {
//					int i;
//					StreamWriter sw = new StreamWriter (locationsFile, false);
//					for (i = 2/3 * maxLineCount; i < lines.Count; i++) {
//						sw.WriteLine (lines [i]);
//					}
//					sw.Close ();
//				}
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
			try{
			StreamWriter sw;
			List<string> lines = new List<string> (File.ReadLines (locationsFile));

			if (File.Exists (locationsFile) && lines.Count < maxLineCount) {
				sw = File.AppendText (locationsFile);
			} else if (File.Exists (locationsFile) && !(lines.Count < maxLineCount)){
				sw = new StreamWriter (locationsFile, false);
				int i;
				for (i = lines.Count* 1/3; i < lines.Count; i++) {
						sw.WriteLine (lines [i]);
				}
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

			if (lastRecord != null) {		
				timeDifference = record.unixTime - lastRecord.unixTime;
				sameProviders = record.provider == lastRecord.provider;
			}

			lastRecord = record;
			sw.WriteLine (JsonConvert.SerializeObject (record));

			//if (record.calculatedSpeed > speedLimit && !denseUpdates) {
			if (record.calculatedSpeed > speedLimit && !denseUpdates && timeDifference > timeBetweenUpdates * 0.5 && sameProviders) {

				locMgr.RemoveUpdates (this);
				denseUpdates = true;
				if(locMgr.IsProviderEnabled(LocationManager.GpsProvider)){
					locMgr.RequestLocationUpdates (LocationManager.GpsProvider, timeBetweenUpdatesDense , 0, this);
				} else {
					locMgr.RequestLocationUpdates (LocationManager.NetworkProvider, timeBetweenUpdatesDense , 0, this);
				}
			}


			if (record.calculatedSpeed < speedLimit && record.recordedSpeed < speedLimit && denseUpdates) {
				slowMesurement += 1;
				if(slowMesurement > 5){
					locMgr.RemoveUpdates (this);
					denseUpdates = false;
					slowMesurement = 0;
					locMgr.RequestLocationUpdates (LocationManager.NetworkProvider, timeBetweenUpdates , 0, this);
				}
			}

			if ((record.calculatedSpeed > speedLimit || record.recordedSpeed > speedLimit) && denseUpdates) {
				if (slowMesurement > 0)
					slowMesurement--;
			}

			sw.Close ();
			}catch(Exception e){
					//Log.Debug ("File created","first time");
					
				}

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


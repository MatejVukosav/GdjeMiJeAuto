using System;
using System.IO;
using Android.App;
using Android.Content;
using Android.Locations;
using Newtonsoft.Json;
 

namespace Gdje_mi_je_auto1
{
	[Service]
	public class LocationLogger : Service , ILocationListener
	{
		private LocationManager locMgr;
		private string locationsFile;

		public override void OnStart (Android.Content.Intent intent, int startId)
		{
			base.OnStart (intent, startId);
			locMgr = GetSystemService (Context.LocationService) as LocationManager;
			locMgr.RequestLocationUpdates (LocationManager.NetworkProvider, 1000*60 , 0, this);

			locationsFile = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
			locationsFile = Path.Combine(locationsFile, "locations.txt");

		}

		public override void OnDestroy ()
		{
			locMgr.RemoveUpdates (this);
			base.OnDestroy ();    
		}
			
		public override Android.OS.IBinder OnBind (Android.Content.Intent intent)
		{
			
			throw new NotImplementedException ();
		}

		public void OnLocationChanged (Location location)
		{
			using (StreamWriter sw = File.AppendText (locationsFile)) {
				LocationRecord record = new LocationRecord {
					time = DateTime.Now.ToLocalTime(),
					latitude = location.Latitude,
					longitude = location.Longitude,
					provider = location.Provider,
					recordedSpeed = location.HasSpeed ? location.Speed : double.NaN,
					calculatedSpeed = 0,
					unixTime = location.Time
				};
			sw.WriteLine (JsonConvert.SerializeObject (record));
			}
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


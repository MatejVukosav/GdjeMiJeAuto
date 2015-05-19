using System;
using Android.Locations;


namespace Gdje_mi_je_auto1
{
	public class LocationRecord
	{
		public DateTime time { get; set;}
		public double latitude { get; set;}
		public double longitude { get; set;}
		public string provider {get; set;}
		public double recordedSpeed { get; set;}
		public double calculatedSpeed { get; set;}
		public long unixTime { get; set;}
	}
}


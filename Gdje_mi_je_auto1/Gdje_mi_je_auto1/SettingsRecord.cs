using System;
using Android.Locations;


namespace Gdje_mi_je_auto1
{
	public class SettingsRecord
	{
		public bool useLocationLogging { get; set;}
		public int timeBetweenUpdates { get; set;}
		public int timeBetweenUpdatesDense { get; set;}
		public double speedLimit { get; set;}
	}
}


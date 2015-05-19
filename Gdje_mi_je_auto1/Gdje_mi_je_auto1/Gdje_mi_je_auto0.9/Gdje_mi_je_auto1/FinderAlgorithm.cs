using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Newtonsoft.Json;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;

namespace Gdje_mi_je_auto1
{
	class FinderAlgorithm
	{
		private static string locationsFile;
		private static int millisBefore = 3*60*1000;
		private static int millisAfter = 5*60*1000;
		private static double speedLimit = 30/3.6;
		private static int ignoreSlows = 8;


		public static List<MarkerOptions>  FindCar()
		{
			List<MarkerOptions> markers = new List<MarkerOptions>();

			locationsFile = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
			locationsFile = Path.Combine(locationsFile, "locations.txt");

			IEnumerable<string> lines;

			if (File.Exists (locationsFile)) {
				lines = File.ReadLines (locationsFile);
			} else {
				return new List<MarkerOptions> ();
			}

			List<LocationRecord> records = new List<LocationRecord>();

			foreach (string line in lines ){
				LocationRecord record = JsonConvert.DeserializeObject<LocationRecord> (line);
				records.Add(record);
			}
				

			IEnumerable<string> speed = from record in records
				select record.calculatedSpeed > speedLimit ? "fast": "slow";
			string[] speeds = speed.ToArray();

			int i = 0;

			for(i=0; i<speeds.Count(); i++) {
				if (i > 0)
				{
					if (speeds[i] != speeds[i - 1] && speeds[i] == "slow")
					{
						int fasts = CountFasts(speeds, i);
						int slows = CountSlows(speeds, i);

						long fastTime = records[i].unixTime - records[i-fasts].unixTime;
						long slowTime = -( records[i].unixTime - records[i+slows].unixTime );


						if (fastTime > millisBefore && slowTime > millisAfter)
						{
							MarkerOptions options = new MarkerOptions ();
							options.SetPosition (new LatLng (records[i].latitude, records[i].longitude));
							options.SetTitle (records[i].time.ToShortTimeString ());
							options.SetSnippet (!double.IsNaN(records[i].recordedSpeed)?"Speed: " + String.Format ("{0:0.00}", records[i].recordedSpeed*3.6) + " km/h":"Speed: " + String.Format ("{0:0.00}", records[i].calculatedSpeed*3.6) + " km/h");
							markers.Add(options);
						}
					}
				}
			}

			return markers;
		}

		private static int CountFasts(string[] speeds, int index)
		{ 
			int returnValue = 0;

			index--;
			while (speeds[index] == "fast" && index > 0)
			{
				returnValue++;
				index--;
			}

			int slows = CountSlows(speeds, index, false);
			if (slows < ignoreSlows && index > ignoreSlows)
			{
				return returnValue + CountFasts(speeds, index - slows + 1);
			}

			return returnValue;
		}

		private static int CountSlows(string[] speeds, int index, bool forward = true)
		{
			int returnValue = 0;
			while (speeds[index] == "slow" && index < speeds.Length - 1 && index > 0)
			{
				returnValue++;
				if (forward)
					index++;
				else
					index--;
			}

			return returnValue;
		}
	}
}


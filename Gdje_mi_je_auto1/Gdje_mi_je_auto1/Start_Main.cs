
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;

namespace Gdje_mi_je_auto1
{
	[Activity (Label = "Gdje mi je auto?!",MainLauncher = true,ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]			
	public class Start_Main : Activity
	{
		string settingsFile;
		private const int MINUTE = 1000 * 60;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.Start);

			//Inicijaliziraj da bi mogao dobivati zone
			GeoFencer.Initialize(this);

			//put do filea za settingse
			settingsFile = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
			settingsFile = Path.Combine(settingsFile, "settings.txt");

			//ako ne postoji settings file stavi standardne vrijednosti i pokreni zapisivanje (uključeno je po defaultu)
			if (!File.Exists (settingsFile)) {
				CreateSettingsFile ();
				StartService (new Intent (this, typeof(LocationLogger)));
			} else {
				//ako u fajlu piše da se zapisivanje koristi, a nije pokrenuto onda pokreni zapisivanje
				if (!isLoggerRunning ()) {
					SettingsRecord settings = JsonConvert.DeserializeObject<SettingsRecord> (File.ReadLines (settingsFile).First ());
					if (settings.useLocationLogging) {
						StartService (new Intent (this, typeof(LocationLogger)));
					}
				}
			}


			//Pokaži korisniku poruku da je bedast ako ne ukljući GPS
			if (!GeoFencer.IsGPSProviderEnabled ()) {
				AlertDialog.Builder alert = new AlertDialog.Builder (this);
				alert.SetTitle ("Nemate uključen GPS");
				alert.SetMessage ("Kako bi što bolje mogli locirati Vaš auto preporučamo Vam da omogućite korištenje GPS-a u postavkama telefona.");
				alert.SetPositiveButton ("OK", (senderAlert, args) => {
					return;
				});
				alert.Show ();
			}


			Button startPAY = FindViewById<Button> (Resource.Id.startPay);
			Button startMAP = FindViewById<Button> (Resource.Id.startMap);
			Button startSETTINGS = FindViewById<Button> (Resource.Id.startSettings);
			Button startALARM = FindViewById<Button> (Resource.Id.startAlarm);

			startMAP.Click += delegate {
				var activity_start_sms_main = new Intent (this, typeof(MapActivity));
				StartActivity (activity_start_sms_main);
			};

			//Pokreni SMS
			startPAY.Click += delegate {
				var activity_start_sms_main = new Intent (this, typeof(Pay_Main));
				StartActivity (activity_start_sms_main);
			};

			startALARM.Click += delegate {
				var activity_alarmMain = new Intent (this, typeof(AlarmMain));
				StartActivity (activity_alarmMain);
			};

			startSETTINGS.Click += delegate {
				var activity_start_sms_main = new Intent (this, typeof(Postavke));
				StartActivity (activity_start_sms_main);
			};


		}



		private void CreateSettingsFile(){
			SettingsRecord settings = new SettingsRecord {
				useLocationLogging = true,
				timeBetweenUpdates = MINUTE * 4,
				timeBetweenUpdatesDense = MINUTE /2,
				speedLimit = 15 / 3.6
			};

			using (TextWriter tw = File.CreateText(settingsFile)){
				tw.WriteLine (JsonConvert.SerializeObject (settings));
			}
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


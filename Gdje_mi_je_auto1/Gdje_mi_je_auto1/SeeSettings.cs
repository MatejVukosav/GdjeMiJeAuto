using System.IO;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Gdje_mi_je_auto1
{
	[Activity (ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]			
	public class SeeSettings : Activity
	{
		private TextView textLocations;
		private Button btnClear;
		private string settingsFile;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.Locations);

			textLocations = FindViewById<TextView> (Resource.Id.textLocations);

			btnClear = FindViewById<Button> (Resource.Id.locationsClear);
		

			settingsFile = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
			settingsFile = Path.Combine(settingsFile, "settings.txt");

			btnClear.Click += delegate {
				File.WriteAllText(settingsFile,string.Empty);
				textLocations.Text = "";
			};

			if (!File.Exists(settingsFile))
				File.Create(settingsFile);


			textLocations.Text = File.ReadAllText(settingsFile);

		}
	}
}


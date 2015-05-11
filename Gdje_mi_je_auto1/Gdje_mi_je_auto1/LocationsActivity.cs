using System.IO;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Gdje_mi_je_auto1
{
	[Activity (Label = "Locations")]			
	public class LocationsActivity : Activity
	{
		private TextView textLocations;
		private Button btnClear;
		private string locationsFile;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.Locations);

			textLocations = FindViewById<TextView> (Resource.Id.textLocations);

			btnClear = FindViewById<Button> (Resource.Id.locationsClear);

			btnClear.Click += delegate {
				File.WriteAllText(locationsFile,string.Empty);
				textLocations.Text = "";
			};

			locationsFile = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
			locationsFile = Path.Combine(locationsFile, "locations.txt");
			textLocations.Text = File.ReadAllText(locationsFile);

		}
	}
}


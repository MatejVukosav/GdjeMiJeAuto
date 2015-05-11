using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Content.PM;

namespace Gdje_mi_je_auto1
{
	[Activity (Label = "MapTest",  Icon = "@drawable/icon", ScreenOrientation = ScreenOrientation.Portrait)]
	public class MapActivity : Activity , IOnMapReadyCallback
	{
		
		private GoogleMap mMap;
		private Zoner zoner;
		private bool zonesVisible = false;
		private bool locationsVisible = false;
		private Button btnZones;
		private Button btnLocations;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.MapLayout);

			btnZones = FindViewById<Button>(Resource.Id.btnZones);
			btnZones.Click += btnZones_Click;
			btnLocations = FindViewById<Button>(Resource.Id.btnLocations);
			btnLocations.Click += btnLocations_Click;

			Spinner spinner = FindViewById<Spinner> (Resource.Id.spinner1);
			spinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs> (spinner_ItemSelected);

			var adapter = ArrayAdapter.CreateFromResource (this, Resource.Array.maps_array, Android.Resource.Layout.SimpleSpinnerItem);

			adapter.SetDropDownViewResource (Android.Resource.Layout.SimpleSpinnerDropDownItem);
			spinner.Adapter = adapter;

			SetUpMap();
		}

		private void SetUpMap()
		{
			if (mMap == null)
			{
				FragmentManager.FindFragmentById<MapFragment> (Resource.Id.map).GetMapAsync (this);
			}
		}
			
		public void OnMapReady (Android.Gms.Maps.GoogleMap googleMap)
		{
			mMap = googleMap;
			mMap.MyLocationEnabled = true;
			setToZg (mMap);

			zoner = new Zoner (mMap);
		}
			
		void btnZones_Click (object sender, EventArgs e )
		{
			if (zonesVisible) {
				zoner.HideZones ();
				btnZones.Text = "Zones (off)";
				zonesVisible = false;
			} else {
				zoner.ShowZones ();
				btnZones.Text = "Zones (on)";
				zonesVisible = true;
			}
		}

		void btnLocations_Click (object sender, EventArgs e)
		{
			if (locationsVisible) {
				zoner.RemoveMarkers();
				btnLocations.Text = "Locations (off)";
				locationsVisible = false;
			} else {
				zoner.SetMarkers (5);
				btnLocations.Text = "Locations (on)";
				locationsVisible = true;
			}
		}

		private void spinner_ItemSelected (object sender, AdapterView.ItemSelectedEventArgs e)
		{
			Spinner spinner = (Spinner)sender;
			string MapType = (string) spinner.GetItemAtPosition (e.Position);
	
			switch (MapType) {
			case "Normal":
				mMap.MapType = GoogleMap.MapTypeNormal;
				break;
			case "Satellite":
				mMap.MapType = GoogleMap.MapTypeSatellite;
				break;
			case "Hybrid":
				mMap.MapType = GoogleMap.MapTypeHybrid;
				break;
			default:
				mMap.MapType = GoogleMap.MapTypeNormal;
				break;
			}
		}

		private void setToZg(GoogleMap map)
		{
			LatLng zg = new LatLng (45.81444, 15.97798); 
			CameraUpdate camera = CameraUpdateFactory.NewLatLngZoom (zg, 12);

			map.MoveCamera (camera);			
		}
			
	}
}



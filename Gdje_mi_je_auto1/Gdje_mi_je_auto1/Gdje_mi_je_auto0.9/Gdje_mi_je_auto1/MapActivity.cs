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
using System.Globalization;

namespace Gdje_mi_je_auto1
{
	[Activity (Label = "MapTest", MainLauncher = false, Icon = "@drawable/icon", ScreenOrientation = ScreenOrientation.Portrait)]
	public class MapActivity : Activity , IOnMapReadyCallback
	{
		
		private GoogleMap mMap;
		private Zoner zoner;
		private bool zonesVisible = true;
		private bool locationsVisible = false;
		private Button btnZones;
		private Button btnLocations;

		NumberFormatInfo nfi;

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

			nfi = new NumberFormatInfo();
			nfi.NumberDecimalSeparator = ".";

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
			mMap.MarkerClick += MapOnMarkerClick;
			setToZg (mMap);
// OVO TREBA MAKNUT!!!
//			mMap.AddMarker (new MarkerOptions().SetPosition(new LatLng (45.801827, 15.969146)).InvokeIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.carMarker)));
//			mMap.AddMarker (new MarkerOptions().SetPosition(new LatLng (45.82, 15.98)));
			zoner = new Zoner (mMap);
			zoner.ShowZones ();
		}
			
		void btnZones_Click (object sender, EventArgs e )
		{
			if (zonesVisible) {
				zoner.HideZones ();
				btnZones.Text = "Zone (off)";
				zonesVisible = false;
			} else {
				zoner.ShowZones ();
				btnZones.Text = "Zone (on)";
				zonesVisible = true;
			}
		}

		void btnLocations_Click (object sender, EventArgs e)
		{
			if (!GeoFencer.IsNetworkProviderEnabled () && !GeoFencer.IsGPSProviderEnabled ()) {
				AlertDialog.Builder alert = new AlertDialog.Builder (this);
				alert.SetTitle ("Lokacijske usluge isključene");
				alert.SetMessage ("Ne možete koristiti uslugu pronalaska automobila bez uključenih lokacijskih usluga!");
				alert.SetPositiveButton ("OK", (senderAlert, args) => {
					return;
				});
				alert.Show ();
			}
					
			if (locationsVisible) {
				zoner.RemoveMarkers();
				//btnLocations.Text = "Locations (off)";
				locationsVisible = false;
			} else {
				if (zoner.SetMarkers ()) {
					//btnLocations.Text = "Locations (on)";
					locationsVisible = true;
				} else {
					Toast.MakeText (this, "Nemoguće pronaći auto", ToastLength.Long).Show();
				}
			}
		}

//		void btnLocations_Click (object sender, EventArgs e){
//			LatLng origin = GeoFencer.getLocation ();
//			//zoner.DrawRoute (origin.Latitude.ToString()+","+origin.Longitude.ToString());
//		}

		private void MapOnMarkerClick(object sender, GoogleMap.MarkerClickEventArgs markerClickEventArgs)
		{
			markerClickEventArgs.Handled = true;
			Marker marker = markerClickEventArgs.Marker;
			LatLng origin = GeoFencer.getLocation ();
			LatLng destination = marker.Position;

			zoner.DrawRoute (origin.Latitude.ToString(nfi)+","+origin.Longitude.ToString(nfi), destination.Latitude.ToString(nfi)+","+destination.Longitude.ToString(nfi));
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



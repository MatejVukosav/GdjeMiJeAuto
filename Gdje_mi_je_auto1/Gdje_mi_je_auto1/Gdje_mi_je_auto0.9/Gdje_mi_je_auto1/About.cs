
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Gdje_mi_je_auto1
{
	[Activity (Label="O autorima",NoHistory = true,ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]			
	public class About : Activity
	{

		List<string> authors = new List<string> ();
		ListView listview;
	
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.About);
			listview = FindViewById<ListView> (Resource.Id.AboutAuthorsList);
			//TODO nadopuniti mailove
			authors.Add ("Filip Sakač , mail: (@fer.hr)");
			authors.Add ("Matej Vukosav , mail: (mv473765@fer.hr)");
			authors.Add ("Antonio Brezjan , mail: (@fer.hr)");
			authors.Add ("Emanuel Vukelić , mail: (@fer.hr)");

			listview.Adapter = new AuthorsAdapterKlasa (this, authors.ToArray ());

		}
	}
}


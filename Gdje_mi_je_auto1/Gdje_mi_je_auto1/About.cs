
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

			authors.Add ("Filip Sakač"+"\n"+"mail: (filip.sakac@fer.hr)");
			authors.Add ("Matej Vukosav"+"\n"+"mail: (matej.vukosav@fer.hr)");
			authors.Add ("Antonio Brezjan"+"\n"+"mail: (antonio.brezjan@fer.hr)");
			authors.Add ("Emanuel Vukelić"+"\n"+"mail: (emanuel.vukelic@fer.hr)");

			listview.Adapter = new AuthorsAdapterKlasa (this, authors.ToArray ());

		}
	}
}


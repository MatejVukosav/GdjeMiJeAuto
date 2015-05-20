
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
	/*
	 * Klasa u kojoj se omogucava korisniku biranje slike registracije.
	 * */
	[Activity (NoHistory = true,ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]			
	public class ChangeRegistrationPicture : Activity
	{

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.ChangeRegistrationPicture);
		
			ImageButton regBtn1=FindViewById<ImageButton> (Resource.Id.registrationPicture1);	
			ImageButton regBtn2=FindViewById<ImageButton> (Resource.Id.registrationPicture2);	
			ImageButton regBtn3=FindViewById<ImageButton> (Resource.Id.registrationPicture3);	

			//registrationPicture.Click +=new EventHandler (PictureOnClick);
			var prefs = Application.Context.GetSharedPreferences("MySharedPrefs", FileCreationMode.Private);
			var prefsEditor = prefs.Edit ();


			regBtn1.Click += delegate  {
				 int regaPic=Resource.Drawable.ociscena_rega;
				prefsEditor.PutInt("MyRegistrationPrefs", regaPic);
				prefsEditor.Commit();
				var activity_P = new Intent (this, typeof(Postavke));
				StartActivity (activity_P);
			};
			regBtn2.Click += delegate  {
				int regaPic=Resource.Drawable.registracija2;
				prefsEditor.PutInt("MyRegistrationPrefs", regaPic);
				prefsEditor.Commit();
				var activity_P = new Intent (this, typeof(Postavke));
				StartActivity (activity_P);
			};
			regBtn3.Click += delegate  {
				int regaPic=Resource.Drawable.registracija3;
				prefsEditor.PutInt("MyRegistrationPrefs", regaPic);
				prefsEditor.Commit();
				var activity_P = new Intent (this, typeof(Postavke));
				StartActivity (activity_P);
			};
		
		}
			
	}
}


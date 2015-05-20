﻿
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
using System.Threading;

/**
 * SPLASH SCREEN
 * Ekran koji se prikazuje dok se učitavaju podaci. Disabled
 * */
namespace Gdje_mi_je_auto1
{
	[Activity (Theme="@style/Theme.splash",Icon = "@drawable/main_icon" ,NoHistory = true,ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
	public class Splash_screen:Activity
	{
          
		protected override void OnCreate (Bundle bundle)
		{	
			Thread.Sleep (1500);
			base.OnCreate (bundle);

			StartActivity (typeof(Pay_Main));

		}
	}
}



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
	[Activity (Label = "ChooseReminderTime",NoHistory = true)]			
	public class ChooseReminderTime : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.ChooseReminderTime);
			//reminder vrijeme prije obavijesti
			//Spinner spinnerPayA = FindViewById<Spinner> (Resource.Id.ala);


		}
	}
}


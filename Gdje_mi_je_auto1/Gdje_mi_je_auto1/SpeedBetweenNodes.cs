
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
	[Activity (Label = "SpeedBetweenNodes",NoHistory = true)]			
	public class SpeedBetweenNodes : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.SpeedBetweenNodes);
		}
	}
}


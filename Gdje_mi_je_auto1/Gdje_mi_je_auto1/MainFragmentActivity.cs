using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Support.V4.App;
using Android.OS;
using Android.App;
using Android.Widget;

namespace Gdje_mi_je_auto1
{
	[Activity ()]            
	public class TestCustomDialogActivity : FragmentActivity
	{
		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			// Create your application here
			SetContentView(Resource.Layout.MainFragment);

			var button = FindViewById<Button>(Resource.Id.Button_Launch);
			button.Click += delegate 
			{
				var dialog = new CustomDialogFragment();
				dialog.Show(SupportFragmentManager, "dialog");
			};
		}   
	}
}
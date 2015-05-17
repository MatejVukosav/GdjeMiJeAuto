
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
	[Activity (Label = "EditExistingAlarm")]			
	public class EditExistingAlarm : Activity
	{
		private Button btnUpali;
		private Button btnUgasi;


		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.EditExistingAlarm);


			btnUpali = FindViewById<Button> (Resource.Id.btnUpali);
			btnUgasi = FindViewById<Button> (Resource.Id.btnUgasi);


			btnUgasi.Click+=delegate {
				//TODO ugasi alarm, (ali ne obrisat)
			};
			
			btnUpali.Click+=delegate {
				//TODO upali alarm
			};


		}

	}
}



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
	[Activity ( Label = "alert")]			
	public class alert : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.dialog_fragment_layout);
			var btn1 = FindViewById<Button> (Resource.Id.dialog_btn);
			btn1.Click += simpleDialogClick;

//			var btn2 = FindViewById<Button> (Resource.Id.button1);
//			btn2.Click += ComplexDialogClick;

		}

		void simpleDialogClick(object sender,EventArgs e)
		{
			Android.App.AlertDialog.Builder builder = new AlertDialog.Builder (this);
			AlertDialog alertDialog = builder.Create ();
			alertDialog.SetTitle ("Alert title");
			alertDialog.SetMessage ("This is a sample message");
			alertDialog.SetButton ("OK",(s,ev)=>{});
			alertDialog.Show ();
		}

	}
}



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
	[Activity (Label = "MessageFromInboxSettings",NoHistory = true)]			
	public class MessageFromInboxSettings : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.MessagesFromInboxSettings);


			ToggleButton tb1 = FindViewById<ToggleButton> (Resource.Id.toggleButton1);
			ToggleButton tb2 = FindViewById<ToggleButton> (Resource.Id.toggleButton2);


			tb1.CheckedChange += delegate {
				if(tb1.Checked){
					Fill_ListView_With_Data.Enable_message_update=true;
					tb2.Checked=false;
				}

			};
			tb2.CheckedChange += delegate {
				if(tb2.Checked){
					Fill_ListView_With_Data.DeleteHistory ();
					tb1.Checked=false;
				}

			};

		}
	}
}


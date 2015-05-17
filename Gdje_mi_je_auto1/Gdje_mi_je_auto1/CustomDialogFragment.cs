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
using Android.Graphics.Drawables;
using Android.Graphics;

namespace Gdje_mi_je_auto1 
{
	[Activity ()]
	public class CustomDialogFragment  
	{

//		protected override void OnCreate(Bundle bundle){
//			base.OnCreate (bundle);
//			SetContentView (Resource.Layout.AlertDialog);
//
//			Button bt = FindViewById<Button> (Resource.Id.button1);
//
//			bt.Click += delegate {
//				CreateAddProjectDialog ();
//			};
//
//			}

		public static void CreateListDialog(Context context,List<string> lista) { 

			Dialog dialog = new Dialog (context);
			dialog.SetContentView (Resource.Layout.DialogList);
			ListView lv = (ListView)dialog.FindViewById (Resource.Id.dialogList);
			//List<string> lista = new List<string> ();
			lv.Adapter = new ArrayAdapter<String> (context, Android.Resource.Layout.TestListItem, lista);
			dialog.Window.SetBackgroundDrawableResource (Android.Resource.Color.BackgroundDark);
			//dialog.Window.SetLayout(LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent);
			//dialog.Window.RequestFeature(WindowFeatures.NoTitle);

			dialog.SetCancelable(true);
			dialog.SetTitle ("Povijest poruka.");
			dialog.Show();
		}
	}
}


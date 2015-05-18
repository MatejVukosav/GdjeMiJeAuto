
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
	[Activity (ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]			
	public class AddNewAlarm : Activity
	{
		private Button btnVrijeme;
		private EditText etVrijeme;

		private int hour;
		private int minute;


		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.AddNewAlarm);

			 btnVrijeme = FindViewById<Button> (Resource.Id.btnVrijemeAlarma);
			etVrijeme = FindViewById<EditText> (Resource.Id.etVrijemeAlarma);


			btnVrijeme.Click += delegate {

				CreateAddProjectDialog();

			};

			UpdateTime ();
			UpdateDisplay ();

			TimePickerDialog time_dialog = new TimePickerDialog (this, TimePickerCallback, hour, minute, true);
		}

		private void CreateAddProjectDialog() { 
			var alert = new AlertDialog.Builder (this);
			alert.SetTitle ("Želite li upaliti novi alarm?");
			//alert.SetView (layoutProperties);
			alert.SetCancelable (false);
			alert.SetPositiveButton("Da", HandlePositiveButtonClick);
			alert.SetNegativeButton("Ne", HandelNegativeButtonClick);
			var dialog = alert.Create();
			dialog.Show();
		}

		private void HandlePositiveButtonClick (object sender, EventArgs e) {
			//TODO dodaje novi alarm i sprema ga u listuAlarma i gdje god treba i pali alarm samo s tim vremenom 
			var activity_AM = new Intent (this, typeof(AlarmMain));
			StartActivity (activity_AM);
		}

		private void HandelNegativeButtonClick (object sender, EventArgs e) {
		}


		private void UpdateDisplay ()
		{	
			string time = string.Format ("{0:D2}:{1:D2}", hour, minute.ToString ().PadLeft (2, '0'));
			etVrijeme.Text = time;
		}

		private void TimePickerCallback (object sender, TimePickerDialog.TimeSetEventArgs e)
		{
			hour = e.HourOfDay;
			minute = e.Minute;
			UpdateDisplay ();
		}

		private void UpdateTime(){
			hour = DateTime.Now.Hour;
			minute = DateTime.Now.Minute;
		}

	}
}


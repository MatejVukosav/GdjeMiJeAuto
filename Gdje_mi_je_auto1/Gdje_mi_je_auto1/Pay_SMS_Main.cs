using System;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Text;
using Android.Graphics;
using System.Xml;
using System.IO;
using Android.Util;
using Android.Telephony;
using Android.Database;
using System.Collections.Generic;
using System.Windows;
using System.Runtime.InteropServices;

/*
 * Klasa u kojoj se izvršava slanje SMS poruke. 
 * 
 * */
namespace Gdje_mi_je_auto1
{
	[Activity (Label = "SMS" , Icon = "@drawable/main_icon", NoHistory = true,
		ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
	//ConfigurationChanges=Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize			
	public class Pay_SMS_Main : Activity
	{
		Button sendSMS_btn;
		EditText messageEditText;
		EditText numberEditText;
		bool valid_check=false;
		private readonly int registrationLength=9;

		static Dictionary<string,string> zoneDictionary=new Dictionary<string,string>();
		static List<string> zone = new List<string> ();

		Android.Telephony.SmsManager smsManager = Android.Telephony.SmsManager.Default;
		ListViewAdapter ls = new ListViewAdapter ();
		ListView listView;


		static String message_Path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
		String message_Data = System.IO.Path.Combine(message_Path, "Message_data.txt");

		protected override void OnCreate (Bundle bundle)
		{
			//RequestWindowFeature(WindowFeatures.NoTitle);
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.Pay_SMS_Main);
			sendSMS_btn = FindViewById<Button> (Resource.Id.sendSMS);
			numberEditText = FindViewById<EditText> (Resource.Id.editText_number);
			listView = FindViewById<ListView> (Resource.Id.List_SMS_Main_History);
			messageEditText = FindViewById<EditText> (Resource.Id.editText_message);

			//prikazuje sva slova kao velika.(upper) i ogranicava velicinu registracije na registrationLength
			messageEditText.SetFilters (new IInputFilter[] { new InputFilterAllCaps (),new InputFilterLengthFilter (registrationLength) });


			#region Ucitava podatke iz datoteke svaki put kad se otvori layout Pay_SMS_Main
			//Log.Debug ("ON CREATE","DULJINA"+new FileInfo(message_Data).Length+" -- "+Enable_message_update);
			long duljina=0;
			try{
				duljina = new FileInfo (message_Data).Length;
			}catch(Exception e){

				Log.Debug ("Pay_SMS_Main","FILE INFO krivo učitava "+e.ToString ());
			}
				
			if (duljina != 0 || Fill_ListView_With_Data.update_inbox_messages==true) {
				List<string> data = new List<string> ();
				String line;

				StreamReader reader = new StreamReader (message_Data);
				while ((line=reader.ReadLine ()) != null) {
					data.Add (line);
				}

				reader.Close ();
				try{
					Fill_ListView_With_Data.FillListWithData (data,this,listView);
				}
				catch(NullReferenceException e){
					Log.Debug ("FillListWithData:Na pocetku Pay_SMS_Main",	e.ToString ());
				}
			}else{
				Fill_ListView_With_Data.DeleteHistory ();
			}

			#endregion


			//if user enabled Inbox messages
			if(Fill_ListView_With_Data.Enable_message_update==true){
				Fill_ListView_With_Data.Fill_With_Inbox_Data (this,listView);
			}


				

			try{
				var tuple=ParseZoneNumbers.LoadZoneNumbersAssetsData (this);
				zone = tuple.Item1; //zone
				zoneDictionary = tuple.Item2; // zone i pripadni stringovi
			}catch(Exception e){
				Log.Debug ("Greska prilikom ucitavanja zone i zoneDictionary u Pay_SMS_Main",e.ToString ());
			}
			Log.Debug ("U ON CREATE", "U ON CREATE PAY_SMS_MAIN");

			//dodavanje metode EventHandler delegatu iz OnReceiveSMS
			//OnReceiveSMS.ReceiveSMSmessage += new OnReceiveSMS.ReceiveSMSdelegate (EventHandler);




			#region Button inicijalizacija zona i Click metoda

			/*
			 * Inicijalizacija dugmova zona i postavljanje njihovog rada.
			 */

			Button zona1_btn = FindViewById<Button> (Resource.Id.zona1_btn);
			Button zona2_btn = FindViewById<Button> (Resource.Id.zona2_btn);
			Button zona3_btn = FindViewById<Button> (Resource.Id.zona3_btn);
			Button zona4_1_btn = FindViewById<Button> (Resource.Id.zona4_1_btn);
			Button zona4_2_btn = FindViewById<Button> (Resource.Id.zona4_2_btn);
			Button vuki_btn = FindViewById<Button> (Resource.Id.vuki_btn);
			string zone_number = "";


			zona1_btn.Click += delegate {
				zone_number=zone[0];
				//zone_number = Resources.GetString (Resource.String.zona1_number);
				numberEditText.Text = (zone_number + " [ ZONA 1 ] ");
			};

			zona2_btn.Click += delegate {
				zone_number=zone[1];
				//zone_number = Resources.GetString (Resource.String.zona2_number);
				numberEditText.Text = (zone_number + " [ ZONA 2 ] ");
			};
			zona3_btn.Click += delegate {
				zone_number=zone[2];
				//zone_number = Resources.GetString (Resource.String.zona3_number);
				numberEditText.Text = (zone_number + " [ ZONA 3 ] ");
			};
			zona4_1_btn.Click += delegate {
				zone_number=zone[3];
				//zone_number = Resources.GetString (Resource.String.zona4_1_number);
				numberEditText.Text = (zone_number + " [ ZONA 4.1 ] ");
			};
			zona4_2_btn.Click += delegate {
				zone_number=zone[4];
				//zone_number = Resources.GetString (Resource.String.zona4_2_number);
				numberEditText.Text = (zone_number + " [ ZONA 4.2 ] ");
			};

			vuki_btn.Click += delegate {
				zone_number=zone[5];
				//zone_number = Resources.GetString (Resource.String.vuki_number);
				numberEditText.Text = (zone_number + " [  ] ");
			};

			#endregion


			#region Slanje SMS poruke 
			/**
			 * Dio za slanje SMS poruke.
			 */
			sendSMS_btn.Click += delegate {
				var sms_message = messageEditText.Text;
				if(sms_message.Length>=1 && CheckSMSNumbers (zone_number) )
				{
					valid_check = true;
				}


				if (valid_check) {
					smsManager.SendTextMessage (zone_number, null, sms_message, null, null);
					Toast.MakeText (ApplicationContext, "SMS poruka je poslana.", ToastLength.Short).Show ();

					var activity_pay_main = new Intent (this, typeof(Pay_Main));

					StartActivity (activity_pay_main);

				} else {

					if (zone_number.Length == 0 && sms_message.Length == 0) {
						Toast.MakeText (ApplicationContext, "Odaberite zonu i upišite registracijsku oznaku.", ToastLength.Short).Show ();
					} else if (zone_number.Length == 0 && sms_message.Length != 0) {
						Toast.MakeText (ApplicationContext, "Odaberite zonu.", ToastLength.Short).Show ();
					} else {
						Toast.MakeText (ApplicationContext, "Upišite registracijsku oznaku.", ToastLength.Short).Show ();
					}
				}

			};
			#endregion

		}//end OnClick

		/*
		 * Metoda koja nadodaje dolaznu poruku u list view.
		 * */
		public static void AddIncomingMessageToView(string smsSender,string smsBody,string smsTime,string smsDate){

			Pay_SMS_Main psm=new Pay_SMS_Main();
			string poruka = psm.MessageDisplay (smsSender, smsBody,smsTime,smsDate);
			//string poruka=psm.DetermineZone(smsSender)+" [ " +smsBody +" ]" + " "+smsTime; 
			//zapisuje poruku u datoteku
			using (StreamWriter sw = File.AppendText (psm.message_Data)) {
				sw.Write (poruka+"\n");
			};

		}


	
		/*
		 * Metoda koju koristi delegat u obradi poruke.
		 * */
		//		protected void EventHandler(string smsSender,string smsBody,string smsTime){ }



		/* 
		 * Metoda koja provjerava za (value),nalazi li se u rasponu dopuštenih brojeva. 
		 * */
		public bool CheckSMSNumbers(string value){
			bool valid_number=false;

			foreach (string number in zone)
			{
				if(string.Compare(value,number)==0){
					valid_number=true;
					break;
				}
			}

			return valid_number;
		}


		/*
		 * Metoda koja uređuje ispis history-a
		 * */
		public String MessageDisplay(string smsSender,string smsBody,string smsTime,string smsDate){
			//return DetermineZone(smsSender)+" < " +"Vrijeme Od : Do" +" >"+" Datum " ;
			string[] satOD=smsTime.Split (':');
			//string format = "{ 0, 10 }"; // 
			string editedDate =smsDate.PadLeft (10,'_'); //string.Format (format, smsDate);

			string smsTimeDO=satOD[0];
			int smsTimeDOInt=Int32.Parse (satOD[0]);

			if (smsTimeDO.Equals ("23")) {
				smsTimeDO = "00";
			} else {
				smsTimeDO=string.Format("{0:D2}",(smsTimeDOInt+1));
			}
			 smsTimeDO = smsTimeDO +":"+ satOD [1];

			return editedDate+"  "+DetermineZone(smsSender)+ " < "+smsTime+" - "+smsTimeDO+" > "+ " [ " +smsBody +" ]"; 
			//return editedDate+"  "+DetermineZone(smsSender)+ " OD "+smsTime+" DO "+smsTimeDO+" "+ " [ " +smsBody +" ]"; 
		}


		/*
		 * Metoda koja na temelju ulaznog broja određuje o kojoj se zoni radi.
		 * zoneDictionary (key,value),key is a number of zone, value is a string representing that zone
		 * */
		private string DetermineZone(String value){

			foreach (KeyValuePair<string,string>pair in zoneDictionary) {
				if (value.Equals (pair.Key))
					return pair.Value;
			}
			return "Number not found.";
		}



		public string ConvertTimeFromMillseconds(long value){
			TimeSpan t = TimeSpan.FromMilliseconds( value );

			string normalTime = string.Format("{0:D2}:{1:D2}", t.Hours, t.Minutes);
			//return t.Hours.ToString ("HH")+":"+t.Minutes.ToString ("mm");
			return normalTime;
		}

		public string ConvertDateFromMillseconds(long value){
			var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			return epoch.AddMilliseconds(value).ToString ("d.M.yyyy");


		}

	}
}


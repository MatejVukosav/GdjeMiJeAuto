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
		bool update_inbox_messages=false;

		static Dictionary<string,string> zoneDictionary=new Dictionary<string,string>();
		static List<string> zone = new List<string> ();

		Android.Telephony.SmsManager smsManager = Android.Telephony.SmsManager.Default;
		ListViewAdapter ls = new ListViewAdapter ();
		ListView listView;


		static String message_Path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
		String message_Data = System.IO.Path.Combine(message_Path, "Message_data.txt");
		public static bool Enable_message_update=false;


		protected override void OnCreate (Bundle bundle)
		{
			//RequestWindowFeature(WindowFeatures.NoTitle);
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.Pay_SMS_Main);

			sendSMS_btn = FindViewById<Button> (Resource.Id.sendSMS);
			numberEditText = FindViewById<EditText> (Resource.Id.editText_number);
			listView = FindViewById<ListView> (Resource.Id.List_SMS_Main_History);
			messageEditText = FindViewById<EditText> (Resource.Id.editText_message);

			//prikazuje sva slova kao velika.(upper)
			messageEditText.SetFilters (new IInputFilter[] { new InputFilterAllCaps () });

			var tuple=LoadZoneNumbersAssetsData ();
			try{
				zone = tuple.Item1; //zone
				zoneDictionary = tuple.Item2; // zone i pripadni stringovi
			}catch(Exception e){
				Log.Debug ("Greska prilikom ucitavanja zone i zoneDictionary u Pay_SMS_Main",e.ToString ());
			}
			Log.Debug ("U ON CREATE", "U ON CREATE");

			//dodavanje metode EventHandler delegatu iz OnReceiveSMS
			//OnReceiveSMS.ReceiveSMSmessage += new OnReceiveSMS.ReceiveSMSdelegate (EventHandler);


			#region Ucitava podatke iz datoteke svaki put kad se otvori layout Pay_SMS_Main
			if (new FileInfo(message_Data).Length != 0 && update_inbox_messages==false) {
				List<string> data = new List<string> ();
				String line;
				StreamReader reader = new StreamReader (message_Data);
					while ((line=reader.ReadLine ()) != null) {
						data.Add (line);
					}
				reader.Close ();

				FillListWithData (data);
			}else{
				DeleteHistory ();
			}

			#endregion
				

			#region ucitavanje inboxa
			if(Enable_message_update){
				
				update_inbox_messages = true;
				List<String> items=new List<String>();

				String[] projection = new String[]{ "address", "body","date" };
				CursorLoader loader=new CursorLoader(this);
				try{
				//slaže podatke obrnutim redom,tj od najstarijeg do najmlađeg tako da kad dodajem novi podatak na kraj da bude poredano
				 loader = new CursorLoader( this,Android.Net.Uri.Parse ("content://sms/inbox"), projection, null, null, null);
				}catch(Exception e){
					Log.Debug ("Problem kod loadera",e.ToString ());
				}
				ICursor  cursor =  (ICursor)loader.LoadInBackground ();
				

				if (cursor.MoveToFirst ()) { // must check the result to prevent exception
					do {
						try {
							//Ako je broj iz datoteke Zone_Numbers_Assets, odnosno ako je broj iz raspona dozvoljenih
							if (CheckSMSNumbers(cursor.GetString (cursor.GetColumnIndexOrThrow (projection [0])))) 
							{
								String smsSender = cursor.GetString (cursor.GetColumnIndexOrThrow (projection [0]));
								String smsBody = cursor.GetString (cursor.GetColumnIndexOrThrow (projection [1]));
								String smsTime = cursor.GetString (cursor.GetColumnIndexOrThrow (projection [2]));
								long smsTimeLong=long.Parse (smsTime);
								String smsDate=ConvertDateFromMillseconds (smsTimeLong);
								smsTime=ConvertTimeFromMillseconds (smsTimeLong);
								String msgData = MessageDisplay (smsSender, smsBody,smsTime,smsDate);
								//punim Listu podacima iz poruke
								items.Add (msgData);
							}
						} catch (Exception e) {
							Log.Debug ("UCITAVANJE SMS-a u Pay_SMS_Main", e.ToString ());  
						}
					} while (cursor.MoveToNext ());
				} else { // empty box, no SMS
					cursor.Close ();
				}
				//okrecem podatke u descending nacin,tako da je najnoviji podatak na pocetku
				items.Reverse ();
				DeleteHistory ();
				using(StreamWriter writer = new StreamWriter(message_Data)){
					
					foreach(String data in items ){
						writer.Write (data+"\n");
					};
				}

				FillListWithData(itemsOpt:items);
				Enable_message_update=false;
			}
			#endregion




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
	

//		private void AddIncomingMessageToView(String poruka){
//		}

		/*
		 * Metoda koju koristi delegat u obradi poruke.
		 * */
//		protected void EventHandler(string smsSender,string smsBody,string smsTime){ }

		/*
		 * Puni list view s podacima .Koristi se ako je omogućeno ucitavanje poruka iz inboxa
		 * */
		private void FillListWithData([Optional]List<string> itemsOpt,[Optional]string[] poljeOpt){
			if(itemsOpt!=null){
				itemsOpt.Reverse ();  
				string[] data = itemsOpt.ToArray () ;
				listView.Adapter = new ListViewAdapter (this, data.ToArray ());

			}
			else if(poljeOpt.Length !=0){
				poljeOpt.Reverse ();
				listView.Adapter = new ListViewAdapter (this, poljeOpt);
			}
			else{
				Log.Debug ("FillListWithData Method","Both arguments are null.");
			}

		}

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
		private String MessageDisplay(string smsSender,string smsBody,string smsTime,string smsDate){
			//return DetermineZone(smsSender)+" < " +"Vrijeme Od : Do" +" >" + " Datum ";
			string[] satOD=smsTime.Split (':');
			int satDO = int.Parse (satOD[0]) + 1;
			String smsTimeDO = satDO +":"+ satOD [1];

			return smsDate+"  "+DetermineZone(smsSender)+ " OD "+smsTime+" DO "+smsTimeDO+" "+ " [ " +smsBody +" ]"; 
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




		/*
		 * Brise sve podatke iz datoteke za spremanje poruka.
		 * */
		public void DeleteHistory(){
			try{
				File.WriteAllText(message_Data, String.Empty);
			}
			catch(FileNotFoundException e) {
				Log.Debug ("DeleteHistory",e.ToString ());
			}
			update_inbox_messages = false;

		}


		/*
		 * Metoda koja ucitava brojeve iz Zone_Numbers_Assets u listu
		 * Return multiple value
		 * */
		private Tuple<List<string>, Dictionary<string,string>>   LoadZoneNumbersAssetsData(){
				string content;
				string[] value;
				Dictionary<string,string> zoneDict=new Dictionary<string,string>();
				List<string> numbers = new List<string> ();

				//otvara datoteku s brojevima
				using (StreamReader sr = new StreamReader (Assets.Open ("Zone_Numbers_Assets.xml")))
						{
							content = sr.ReadToEnd ();
						}

				XmlDocument doc = new XmlDocument();
				doc.LoadXml (content);
				//izvlaci linije u kojima su brojevi
				XmlNodeList x = doc.SelectNodes ("//string");	

				foreach (XmlNode node in x) {
						numbers.Add (node.InnerText);
						value=node.OuterXml.Split ('"');
						zoneDict.Add (node.InnerText,value[1]);
						}

//			foreach (KeyValuePair<string,string>pair in zoneDictionary)
//				Log.Debug ("dictionary", pair.Key + pair.Value);

			return new Tuple<List<string>, Dictionary<string,string>>(numbers,zoneDict);
		}

		public string ConvertTimeFromMillseconds(long value){
			TimeSpan t = TimeSpan.FromMilliseconds( value );

			string normalTime = string.Format("{0}:{1}", t.Hours, t.Minutes);
			//return t.Hours.ToString ("HH")+":"+t.Minutes.ToString ("mm");
			return normalTime;
		}

		public string ConvertDateFromMillseconds(long value){
			var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			return epoch.AddMilliseconds(value).ToString ("d.M.yyyy");


		}

	}
}


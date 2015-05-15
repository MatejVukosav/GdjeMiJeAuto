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
using System.Threading;
using Android.Util;
using System.IO;
using Android.Telephony;
using Android.Database;
using System.Windows;
using System.Runtime.InteropServices;

namespace Gdje_mi_je_auto1
{
	public class Fill_ListView_With_Data:Activity
	{
		static String message_Path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
		static String message_Data = System.IO.Path.Combine(message_Path, "Message_data.txt");

		public static bool Enable_message_update=false;
		public static bool update_inbox_messages=false;

		/*
		 * Puni listView s podacima .
		 * */
		public static void FillListWithData(List<string> itemsOpt,Activity context,ListView listView){
			update_inbox_messages = false;
			if (itemsOpt.Count != 0) {
				itemsOpt.Reverse ();  
				//Log.Debug ("duljina itemsopt",itemsOpt.Count.ToString ());

				string[] data = itemsOpt.ToArray ();

				if (context == null) {
					Log.Debug ("this je null", "this");
				}

				try {

					listView.Adapter = new ListViewAdapter (context, data);
				} catch (Exception e) {
					Log.Debug ("listaaa", e.ToString ());
				}

			} else {
				Log.Debug ("FillListWithData Method", "Both arguments are null.");
			}
		}


		/*
		 * Brise sve podatke iz datoteke za spremanje poruka.
		 * */
		public static void DeleteHistory(){
			
			try{
				File.WriteAllText(message_Data, String.Empty);
			}
			catch(FileNotFoundException e) {
				Log.Debug ("DeleteHistory",e.ToString ());
			}
			update_inbox_messages = false;
		}

		/*
		 * Fill list with message data from user Inbox.
		 * */
		public static void Fill_With_Inbox_Data(Activity context,ListView listView){
			#region ucitavanje inboxa
			Pay_SMS_Main psm=new Pay_SMS_Main();

				update_inbox_messages = true;
				List<String> items=new List<String>();
				bool valid_body_check=false;
				String smsFilteredBody="";

				String[] projection = new String[]{ "address", "body","date" };
				CursorLoader loader=new CursorLoader(context);
				try{
					//slaže podatke obrnutim redom,tj od najstarijeg do najmlađeg tako da kad dodajem novi podatak na kraj da bude poredano
					loader = new CursorLoader( context,Android.Net.Uri.Parse ("content://sms/inbox"), projection, null, null, null);
				}catch(Exception e){
					Log.Debug ("Problem kod loadera",e.ToString ());
				}
				ICursor  cursor =  (ICursor)loader.LoadInBackground ();


				if (cursor.MoveToFirst ()) { // must check the result to prevent exception
					do {
						try {
							//Ako je broj iz datoteke Zone_Numbers_Assets, odnosno ako je broj iz raspona dozvoljenih
							if (psm.CheckSMSNumbers(cursor.GetString (cursor.GetColumnIndexOrThrow (projection [0])))) 
							{
								String smsSender = cursor.GetString (cursor.GetColumnIndexOrThrow (projection [0]));
								String smsBody = cursor.GetString (cursor.GetColumnIndexOrThrow (projection [1]));
								String smsTime = cursor.GetString (cursor.GetColumnIndexOrThrow (projection [2]));
								long smsTimeLong=long.Parse (smsTime);
								String smsDate=psm.ConvertDateFromMillseconds (smsTimeLong);
								smsTime=psm.ConvertTimeFromMillseconds (smsTimeLong);

							try{
								var tuple=ParseSMS.ParseSMSbody (smsBody);
								valid_body_check = tuple.Item1; //bool true or false
								smsFilteredBody = tuple.Item2; // car registration
							}catch(Exception e){
								Log.Debug ("Greska prilikom filtriranja sms poruke.",e.ToString ());
							}
							//check if sms body pass validation
							if(valid_body_check){
								String msgData = psm.MessageDisplay (smsSender, smsFilteredBody,smsTime,smsDate);
								items.Add (msgData);
							}

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


				try{
				FillListWithData(items,context,listView);
				}catch(NullReferenceException e){
					Log.Debug ("FillListWithData:U ucitavanju inboxa",	e.ToString ());
				}


				Enable_message_update=false;
			}
			#endregion





	}
}


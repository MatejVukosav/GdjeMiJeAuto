using System;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Telephony;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Net;
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
using Java;


/*
 * Klasa koja obrađuje dolaznu poruku.
 * 
 * */
namespace Gdje_mi_je_auto1
{

	[BroadcastReceiver(Enabled = true, Label = "SMS Receiver")]
	[IntentFilter(new[] { "android.provider.Telephony.SMS_RECEIVED" },Priority = (int)IntentFilterPriority.HighPriority)] 
	[Activity ( Label = "ReadFromInbox")]			
	
	public class OnReceiveSMS: BroadcastReceiver
	{
//		public  delegate void ReceiveSMSdelegate(string smsSender,string smsBody,String smsTime);
//		public static event ReceiveSMSdelegate ReceiveSMSmessage;
		public static readonly string INTENT_ACTION = "android.provider.Telephony.SMS_RECEIVED"; 

		/*
		 * Metoda koje reagira na dolaznu poruku.
		 * Sprema sadrzaj u poruka i preko delegata ReceiveSMSdelegate poruka se koristi dalje.
		 * */
		public override void OnReceive (Context context, Intent intent)
		{

			Log.Info ("SmsReceiver", "SMS Received");
			//InvokeAbortBroadcast();

			if (intent.Action == INTENT_ACTION) {
				StringBuilder buffer = new StringBuilder ();
				Bundle bundle = intent.Extras;
				String smsSender = "";
				String smsBody = "";
				String smsTime = "";
				String smsDate = "";
				bool valid_body_check = false;
				String smsFilteredBody = "";

				if (bundle != null) {
					Java.Lang.Object[] pdus = (Java.Lang.Object[])bundle.Get ("pdus");


					Pay_SMS_Main psm = new Pay_SMS_Main ();
					SmsMessage[] msgs;

					msgs = new SmsMessage[pdus.Length];
					try {
						for (int i = 0; i < msgs.Length; i++) {	//obradujem poruku
							msgs [i] = SmsMessage.CreateFromPdu ((byte[])pdus [i]);
							smsSender = msgs [i].OriginatingAddress;
							smsBody = msgs [i].MessageBody;

							Log.Debug ("tuu", "buu");

							Intent broadcastIntent = new Intent ();
							broadcastIntent.SetAction ("SMS_READED");
							broadcastIntent.PutExtra ("sms", smsBody);
							context.SendBroadcast (broadcastIntent);

							Intent broadcastIntent1 = new Intent ();
							broadcastIntent1.SetAction ("SMS_READ");
							broadcastIntent1.PutExtra ("sms", smsBody);
							context.SendBroadcast (broadcastIntent1);


//							ContentValues values = new ContentValues();
//							values.Put("READ",1);
////							getContentResolver().update(Android.Net.Uri.Parse("content://sms/inbox"),values,
////								"_id=", null);
//							Log.Debug ("tuu2","buu");

							try {
								var tuple = ParseSMS.ParseSMSbody (smsBody);
								valid_body_check = tuple.Item1; //bool true or false
								smsFilteredBody = tuple.Item2; // car registration
								smsTime = tuple.Item3;
							} catch (Exception e) {
								Log.Debug ("Greska prilikom filtriranja sms poruke.", e.ToString ());
							}
						
							//smsTime= DateTime.Now.ToString("HH:mm");
							smsDate = DateTime.Now.ToString ("d.M.yyyy");

//						Log.Debug ("sender",smsSender);
//						Log.Debug ("evaluation",(smsSender != null).ToString ());
//						Log.Debug ("number",psm.CheckSMSNumbers (smsSender).ToString ());
//						Log.Debug ("check",valid_body_check.ToString ());

							if ((smsSender != null) && psm.CheckSMSNumbers (smsSender) && valid_body_check) {
								//send sms data to further reproduction




//								String[] projection = new String[]{ "_id" };
//								CursorLoader loader = new CursorLoader (context);
//								try {
//									loader = new CursorLoader (context, Android.Net.Uri.Parse ("content://sms/inbox"), projection, null, null, null);
//
//								} catch (Exception e) {
//									Log.Debug ("Problem kod loadera", e.ToString ());
//								}
//
//								ICursor cursor = (ICursor)loader.LoadInBackground ();
//								if (cursor.MoveToFirst ()) { 
//
//									String smsID = cursor.GetString (cursor.GetColumnIndexOrThrow (projection [0]));
//									ContentValues values1=new ContentValues();
//
//									values1.Put (smsID,1);
////									pdus.SetValue ("read",Convert.ToInt32 (smsID));
//									Log.Debug ("id", smsID);
//								}
//

								//	ICursor queryData = ContentResolver.Query (CallLog.Calls.ContentUri, null, queryFilter, null, querySorter)



								Pay_SMS_Main.AddIncomingMessageToView (smsSender, smsFilteredBody, smsTime, smsDate);
							} else {
								Log.Debug ("SMS was not from parking number!", "Pass message.");
								//ClearAbortBroadcast ();
							}
						}
					} catch (Exception e) {
						Log.Debug ("Exception caught while receiving message: !!!", e.Message);
					}
				}


				
			}
		}

	}
}


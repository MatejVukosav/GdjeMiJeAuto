using System;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Telephony;
using Android.Util;
using Android.Views;
using Android.Widget;


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
		public override void OnReceive(Context context, Intent intent)
		{

			Log.Info("SmsReceiver", "SMS Received");
			//InvokeAbortBroadcast();

			if (intent.Action == INTENT_ACTION)
			{
				StringBuilder buffer = new StringBuilder();
				Bundle bundle = intent.Extras;
				String smsSender = "";
				String smsBody = "";
				String smsTime = "";
				String smsDate = "";
				bool valid_body_check = false;
				String smsFilteredBody = "";

				if (bundle != null) {
					Java.Lang.Object[] pdus = (Java.Lang.Object[])bundle.Get ("pdus");

					Pay_SMS_Main psm=new Pay_SMS_Main();
					SmsMessage[] msgs;
					msgs = new SmsMessage[pdus.Length];
					try {
						for (int i = 0; i < msgs.Length; i++) {	//obradujem poruku
							msgs [i] = SmsMessage.CreateFromPdu ((byte[])pdus [i]);
							smsSender = msgs [i].OriginatingAddress;
							smsBody = msgs [i].MessageBody;
							//TODO parse body to take registration and defined validade of sms, now is always reg#30
							try{
								var tuple=ParseSMS.ParseSMSbody (smsBody);
								valid_body_check = tuple.Item1; //bool true or false
								smsFilteredBody = tuple.Item2; // car registration
							}catch(Exception e){
								Log.Debug ("Greska prilikom filtriranja sms poruke.",e.ToString ());
							}

							smsTime= DateTime.Now.ToString("HH:mm");
							smsDate=DateTime.Now.ToString ("d.M.yyyy");

						Log.Debug ("sender",smsSender);
						Log.Debug ("evaluation",(smsSender != null).ToString ());
						Log.Debug ("number",psm.CheckSMSNumbers (smsSender).ToString ());
						Log.Debug ("check",valid_body_check.ToString ());

						if ((smsSender != null) && psm.CheckSMSNumbers (smsSender) && valid_body_check) {
							//send sms data to further reproduction
							Pay_SMS_Main.AddIncomingMessageToView (smsSender, smsFilteredBody, smsTime,smsDate);
							Log.Debug ("poziva je","tu je");

						} else {
							
							Log.Debug ("SMS was not from parking number!","Pass message.");

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


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

				if (bundle != null) {
					Java.Lang.Object[] pdus = (Java.Lang.Object[])bundle.Get ("pdus");

					SmsMessage[] msgs;
					msgs = new SmsMessage[pdus.Length];
					try {
						for (int i = 0; i < msgs.Length; i++) {	//obradujem poruku
							msgs [i] = SmsMessage.CreateFromPdu ((byte[])pdus [i]);
							smsSender = msgs [i].OriginatingAddress;
							smsBody = msgs [i].MessageBody;
							smsTime= DateTime.Now.ToString("HH:mm");
							smsDate=DateTime.Now.ToString ("d.M.yyyy");

						}
						Pay_SMS_Main psm=new Pay_SMS_Main();
							
						if (smsSender != null && psm.CheckSMSNumbers (smsSender)) {
							//saljem podatke poruke 
							Pay_SMS_Main.AddIncomingMessageToView (smsSender, smsBody, smsTime,smsDate);

//							if (ReceiveSMSmessage != null) {
//								Log.Debug ("kefir","if receive");
//								ReceiveSMSmessage (smsSender, smsBody, smsTime);
//							}

						} else {
							Log.Debug ("kefir","elseeeee");

							//ClearAbortBroadcast ();
						}

					} catch (Exception e) {
						Log.Debug ("Exception caught while receiving message: !!!", e.Message);
					}
				}
			} 
		}



	}
}


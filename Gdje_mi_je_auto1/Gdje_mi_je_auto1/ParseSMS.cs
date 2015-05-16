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
	
	public class ParseSMS
	{
		

		public static Tuple<bool, String,String> ParseSMSbody (String smsBody)
		{
			String car_registration = "";

			//test smsBody="Kupili ste parkirnu kartu za vozilo ZG4994AG,ZAGREB2,(3 KN) do 19:52. .."
			bool valid_check = false;

			char[] delimiterChars = { ' ' , ',' ,  '.' };
			string[] words = smsBody.Split(delimiterChars);
			String car_time = "";
//			int i = 0;
//			Log.Debug ("SMS BODY",smsBody);
//			foreach (String str in words) {
//				Log.Debug ("SMS word "+i,str);
//				i++;
//			}
			//Log.Debug ("word",words[0].ToString ());
				
			if (words[0].Equals ("Kupili")) {
				car_registration = words [6]; //registration
				valid_check=true;
				car_time=words[13]; // hour:minutes
				//Log.Debug ("Kupili",car_registration);
				//Log.Debug ("Kupili",car_time);
				Log.Debug ("Equal Kupili","Kupili");
				//TODO disable zvuk dolazne poruke
				//TODO procitat poruku
			} 
			return new Tuple<bool, String,String>(valid_check,car_registration,car_time);

		}

	

	}
}


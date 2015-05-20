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

			char[] delimiterChars = {' ' , ','};
			string[] words = smsBody.Split(delimiterChars);
			foreach (String r in words)
				Log.Debug ("poruka", r);

			String car_time = "";

			bool Do=false;
			bool DoProsao = false;
			bool datumProsao = false;
			//int i = 0;

			foreach (String str in words) {
				Log.Debug ("rijec",str);

				if (str.Equals ("do")) {
					Do = true;
				}

				if(datumProsao) {
					car_time = str; //nema datuma
					break;
				}

				if (DoProsao && (str.Count () > 7)) {
					datumProsao = true;
					Log.Debug ("Do prosao i str.count >7",Do.ToString ());
				}

				if (DoProsao && (str.Count () < 7)) {
					car_time = str;// hour:minutes
					break;
				} 
					


				if (Do) {
					DoProsao = true;
				}

//				Log.Debug ("SMS word "+i,str);
//				Log.Debug ("count", str.Count ().ToString ());
//				i++;

			}
			 Do=false;
			 DoProsao = false;
			 datumProsao = false;
			//Log.Debug ("word",words[0].ToString ());
				
			if (words[0].Equals ("Kupili")) {
				car_registration = words [6]; //registration
				valid_check=true;
			 
				Log.Debug ("Kupili",car_registration);
				Log.Debug ("Kupili",car_time);
				Log.Debug ("Poruka je ispravna.","SMS is valid");
				//TODO disable zvuk dolazne poruke-tesko izvedivo/ne moguce
				//TODO procitat poruku-tesko izvedivo/ne moguce
			} 
			return new Tuple<bool, String,String>(valid_check,car_registration,car_time);

		}

	

	}
}


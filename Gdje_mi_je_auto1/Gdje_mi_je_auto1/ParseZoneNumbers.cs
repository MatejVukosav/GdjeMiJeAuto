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

namespace Gdje_mi_je_auto1
{
	/*
	 * Klasa koja ucitava brojeve iz Zone_Numbers_Assets u listu
	 * Return multiple value 
	 * */
	public class ParseZoneNumbers
	{
		/*using example:
		 *  static Dictionary<string,string> zoneDictionary=new Dictionary<string,string>();
		 *	static List<string> zone = new List<string> ();
		 * try{
				var tuple=ParseZoneNumbers.LoadZoneNumbersAssetsData (this);
				zone = tuple.Item1; //zone 700101,700102...
				zoneDictionary = tuple.Item2; // zone i pripadni stringovi -> key 700101 value[ ZONA 1 ]
			}catch(Exception e){
				Log.Debug ("Greska prilikom ucitavanja zone i zoneDictionary u Pay_SMS_Main",e.ToString ());
			}
		 * 
		 * */
		public static Tuple<List<string>, Dictionary<string,string>> LoadZoneNumbersAssetsData(Activity context){
			string content="";
			string[] value;
			Dictionary<string,string> zoneDict=new Dictionary<string,string>();
			List<string> numbers = new List<string> ();


			String fileName = "Zone_Numbers_Assets.xml";
			using(var input=context.Assets.Open (fileName))
			try{
				//otvara datoteku s brojevima;

				using (StreamReader sr = new StreamReader (input))
			{
				content = sr.ReadToEnd ();
			}
			}catch(Exception e){

				Log.Debug ("PROBLEM"+content,e.ToString ());
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

					//	foreach (KeyValuePair<string,string>pair in zoneDict)
					//		Log.Debug ("dictionary", pair.Key + pair.Value);

			return new Tuple<List<string>, Dictionary<string,string>>(numbers,zoneDict);
		}
			



		}


}


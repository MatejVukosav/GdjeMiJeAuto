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
using Android.Text;
using Android.Graphics;
using System.Xml;
using System.IO;
using Android.Util;
using System.Reflection;
using System.Xml.XPath;

/*
 * Klasa u kojoj se izvršava slanje SMS poruke. 
 * 
 * */
namespace Part_Posalji
{
	[Activity (Label = "SMS" , Icon = "@drawable/main_icon", NoHistory = true)]			
	public class Pay_SMS_Main : Activity
	{
		Button sendSMS_btn;
		EditText messageEditText;
		EditText numberEditText;
		bool valid_check=false;
		Android.Telephony.SmsManager smsManager = Android.Telephony.SmsManager.Default;

		ListViewAdapter ls = new ListViewAdapter ();
		ListView listview;
		string[] items;

		protected override void OnCreate (Bundle bundle)
		{
			//RequestWindowFeature(WindowFeatures.NoTitle);
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.Pay_SMS_Main);

			sendSMS_btn = FindViewById<Button> (Resource.Id.sendSMS);
			numberEditText = FindViewById<EditText> (Resource.Id.editText_number);
			messageEditText = FindViewById<EditText> (Resource.Id.editText_message);
			messageEditText.SetFilters(new IInputFilter[] {new InputFilterAllCaps()});

			#region Ucitavanje liste za prikaz Historya
			items = List_Data.List_Data_Fill ();
			listview = FindViewById<ListView> (Resource.Id.List_SMS_Main_History);
			listview.Adapter = new ListViewAdapter (this,items);
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
			Button vuki_btn=FindViewById<Button> (Resource.Id.vuki_btn);
			string zone_number="";

			zona1_btn.Click+=delegate {
				zone_number=Resources.GetString (Resource.String.zona1_number);
				numberEditText.Text=(zone_number+ " [ ZONA 1 ] ");

			};

			zona2_btn.Click+=delegate {
				
				zone_number=Resources.GetString (Resource.String.zona2_number);
				numberEditText.Text=(zone_number+ " [ ZONA 2 ] ");
			};
			zona3_btn.Click+=delegate {
				zone_number=Resources.GetString (Resource.String.zona3_number);
				numberEditText.Text=(zone_number+ " [ ZONA 3 ] ");
			};
			zona4_1_btn.Click+=delegate {
				zone_number=Resources.GetString (Resource.String.zona4_1_number);
				numberEditText.Text=(zone_number+ " [ ZONA 4.1 ] ");
			};
			zona4_2_btn.Click+=delegate {
				zone_number=Resources.GetString (Resource.String.zona4_2_number);
				numberEditText.Text=(zone_number+ " [ ZONA 4.2 ] ");
			};

			vuki_btn.Click+=delegate {
				zone_number=Resources.GetString (Resource.String.vuki_number);
				numberEditText.Text=(zone_number+ " [  ] ");
			};

			#endregion


			#region Slanje SMS poruke 
			/**
			 * Dio za slanje SMS poruke.
			 */
			sendSMS_btn.Click+= delegate {
				var sms_message=messageEditText.Text;

				valid_check=Check_SMS_Fields(zone_number);

				if(valid_check){
						smsManager.SendTextMessage (zone_number,null,sms_message,null,null);
					Toast.MakeText(ApplicationContext,"SMS poruka je poslana.",ToastLength.Short).Show ();

						var activity_pay_main=new Intent (this,typeof(Pay_Main));
						StartActivity (activity_pay_main);
					}
				else
				{
					if(zone_number.Length==0 && sms_message.Length==0)
					{
						Toast.MakeText (ApplicationContext,"Odaberite zonu i upišite registracijsku oznaku.",ToastLength.Short).Show ();
						}
					else if(zone_number.Length==0 && sms_message.Length!=0)
					{
						Toast.MakeText (ApplicationContext,"Odaberite zonu.",ToastLength.Short).Show ();
						}
					else 
					{
						Toast.MakeText (ApplicationContext,"Upišite registracijsku oznaku.",ToastLength.Short).Show ();
						}
				}

			};
			#endregion

			#region History poruka - Building
			//stvaranje povijesti poruka
			//StartActivity(new Intent(Intent.ActionView, Android.Net.Uri.Parse("sms:"+ phone_number)));

			/*
				 var values = new ContentValues();
				values.Put("address", num);
				values.Put("body", txt);
				values.Put("date", DateTime.Now.Ticks);
				try
				{
				ContentResolver.Insert(Android.Net.Uri.Parse("content://sms/sent"), values);
				}
				catch (Exception ex)
				{
				Console.WriteLine(ex.Message);
				Log.Error(PushHandlerBroadcastReceiver.TAG, ex.Message);
				}
				*/	

			/*private void SaveToInbox(ShortMessages shortMessage)
			{
				var now = shortMessage.MessageDate.HasValue ? shortMessage.MessageDate.Value : DateTime.Now;
				var d = new Date((now.Year - 1900), now.Month - 1, now.Day, now.Hour, now.Minute, now.Second);
				var context = Application.Context.ApplicationContext;
				var values = new ContentValues();
				values.Put("address", shortMessage.From);
				values.Put("body", shortMessage.Message);
				values.Put("read", false);
				values.Put("date", d.Time);
				context.ContentResolver.Insert(Android.Net.Uri.Parse("content://sms/inbox"), values);
			}
			*/
			#endregion
		}
		


		     #region Check_SMS_Fields values
		/**Metoda koja provjerava polja za unos broja i registracije.
		 * Broj mora odgovarati bar jednom broju iz Zet_zone_numbers.
		 * */
		private bool Check_SMS_Fields(string value){
			bool valid_number=false;
			string content;

		using (StreamReader sr = new StreamReader (Assets.Open ("Zone_Numbers_Assets.xml")))
		{
			content = sr.ReadToEnd ();
		}
				
		XmlDocument doc = new XmlDocument();
		doc.LoadXml (content);
		XmlNodeList x = doc.SelectNodes ("//string");

		List<string> zone = new List<string> ();
		foreach (XmlNode node in x) {
			zone.Add (node.InnerText);
		}
				
			foreach (string number in zone)
			{
				if(string.Compare(value,number)==0){
					valid_number=true;
					break;
				}
			}


			if(valid_number && messageEditText.Length ()>=1){
				valid_check = true;
			}
			else{
				valid_check = false;
			}
			return valid_check;
		}
		#endregion
	



	}
}


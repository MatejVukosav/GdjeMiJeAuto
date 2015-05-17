using System;
using Java.Lang;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using Android.Util;

namespace Gdje_mi_je_auto1
{
	
[Activity (Label = "Alarm",  Icon = "@drawable/icon")]
public class Alarms : Activity
{
	public static string mindif="15"; //TODO ova inicijalizacija neka bude u klasi settingsa
	public static bool prebrisi;
	private static int idNovi;
	private static int reqcode;
	private static int idPostoji;
	private static ISharedPreferences prefsDict = Application.Context.GetSharedPreferences("MySharedPrefs", FileCreationMode.Private);
	private static ISharedPreferences prefsidNovi = Application.Context.GetSharedPreferences("MySharedPrefs", FileCreationMode.Private);
	private static ISharedPreferences prefsidPostoji = Application.Context.GetSharedPreferences("MySharedPrefs", FileCreationMode.Private);
	private static ISharedPreferences prefsreqCode = Application.Context.GetSharedPreferences("MySharedPrefs", FileCreationMode.Private);

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			//SetContentView (Resource.Layout.Alarm_Main);

		}

	public static Dictionary<int, string> aktivniAlarmi  {
		get; //sta vraca get ??
		private set;
	}

	// ovo radi novi kalendar na temelju proslijeđenih vrijednosti iz smsa,
	//koji se dalje koristi za stvaranje alarma i remindera
	private static Java.Util.Calendar setCalendars(string hour, string minutes)
	{
		Java.Util.Calendar cal = Java.Util.Calendar.Instance;
		Java.Util.Calendar calNow = Java.Util.Calendar.Instance;
		cal.Set (Java.Util.CalendarField.HourOfDay, Convert.ToInt32 (hour));
		cal.Set (Java.Util.CalendarField.Minute, Convert.ToInt32 (minutes));
		cal.Set (Java.Util.CalendarField.Second, 0);
		cal.Set (Java.Util.CalendarField.Millisecond, 0);

		if (cal.CompareTo (calNow) <= 0) {
			cal.Add (Java.Util.CalendarField.Date, 1);
		}
		return cal;
	}

	//provjera da li se radi o istoj zoni i registraciji - ako da,
	//ključ se ne mijenja te će se alarm (i reminder) prebrisati s novim vremenom
	private static bool provjera(string hour, string minutes, string zona, string rega)
	{
		prebrisi = false;

		foreach (KeyValuePair<int, string> entry in aktivniAlarmi)		// prolazi kroz (vremenski) aktivne alarme i gleda postoji li zapis sa istom zonom i regom
		{
			string[] split = entry.Value.Split ('&');
			if ((split [2] == zona) && (split [3] == rega)) {		// ako postoji:
				prebrisi = true;
				idPostoji = entry.Key;			// idPostoji dohvaća ključ zapisa, koji je ujedno identifikator tog alarma i intenta koji šaljemo
			}
		}

		if (!prebrisi)													// ako ne postoji:
		{
			var sb = new System.Text.StringBuilder ();					// napravi string builder (odnosno string) oblika sat&minute&zona&rega
			sb.Append (hour);
			sb.Append ("&");
			sb.Append (minutes);
			sb.Append ("&");
			sb.Append (zona);
			sb.Append ("&");
			sb.Append (rega);

			aktivniAlarmi.Add (idNovi, sb.ToString());					// dodaj alarm u zapis (vremenski) aktivnih alarma, ključ za zapis i intent je idNovi
			// idNovi je inicijalno nula, povećava se u createAlarm metodi (najspretnija opcija, ne dirati to)

			var prefsDictEditor = prefsDict.Edit ();
			string dictAlarmi = string.Join (";", aktivniAlarmi.Select (x => x.Key.ToString() + ":" + x.Value).ToArray ());
			prefsDictEditor.PutString ("MyAktivniAlarmiPrefs", dictAlarmi);
			prefsDictEditor.Commit ();

		}
		//TODO spremiti idNovi u saved preferences... da li treba i reqcode i idPostoji?

		return prebrisi;
	}


	// automatski stvara i reminder koji se pojavljuje mindif minuta prije alarma
	public static void createAlarm(string hour, string minutes, string zona, string rega)
	{
			Log.Debug ("alar","suuu");
			try{
		string dictString = prefsDict.GetString ("MyAktivniAlarmiPrefs", "-1");
		aktivniAlarmi = dictString.Split(';').Select(x => x.Trim().Split(':')).ToDictionary(x => Convert.ToInt32(x[0]), x => x[1]);
			
				Log.Debug ("alar0","suuu");
		idNovi = prefsidNovi.GetInt ("MyIdNoviPrefs", idNovi);
		idPostoji = prefsidPostoji.GetInt ("MyIdPostojiPrefs", idPostoji);
		reqcode = prefsreqCode.GetInt ("MyReqCodePrefs", reqcode);
			Log.Debug ("alar1","suuu");
		bool rezultat = provjera (hour, minutes, zona, rega);
		if (!rezultat) {												// ako je novi alarm
			reqcode = idNovi;
			idNovi = idNovi + 2;

			//TODO ovdje dodati alarm u list view, id mu treba biti reqcode (ne idNovi!)
		} else {
			reqcode = idPostoji;										// ako alarm već postoji ali ga treba prebrisati s novim vremenom
		}
			Log.Debug ("alar2","suuu");
		var prefsidPostojiEditor = prefsidPostoji.Edit ();
		prefsidPostojiEditor.PutInt ("MyIdPostojiPrefs", idPostoji);
		prefsidPostojiEditor.Commit ();

		var prefsidNoviEditor = prefsidNovi.Edit ();
		prefsidNoviEditor.PutInt ("MyIdNoviPrefs", idNovi);
		prefsidNoviEditor.Commit ();

		var prefsreqCodeEditor = prefsreqCode.Edit ();
		prefsreqCodeEditor.PutInt ("MyReqCodePrefs", reqcode);
		prefsreqCodeEditor.Commit ();
			Log.Debug ("alar3","suuu");
		Java.Util.Calendar cal = setCalendars (hour, minutes);
		Intent intentAlarm = new Intent (Application.Context, typeof(NotifAlarmBroadcast));
		Intent intentReminder = new Intent (Application.Context, typeof(NotifReminderBroadcast));

		PendingIntent alarmPendingIntent = PendingIntent.GetBroadcast (Application.Context, reqcode, intentAlarm, PendingIntentFlags.UpdateCurrent);
		// reminder vezan za neki alarm uvijek ima identifikator za +1 veći od tog alarma, time najlakše pratimo par alarm-reminder
		PendingIntent reminderPendingIntent = PendingIntent.GetBroadcast (Application.Context, reqcode + 1, intentReminder, PendingIntentFlags.UpdateCurrent);
			Log.Debug ("alar4","suuu");
		AlarmManager am = (AlarmManager)Application.Context.ApplicationContext.GetSystemService (Context.AlarmService);
		am.SetExact (AlarmType.RtcWakeup, cal.TimeInMillis, alarmPendingIntent);
		am.SetExact (AlarmType.RtcWakeup, (cal.TimeInMillis)-((Convert.ToInt64(mindif))*60*1000), reminderPendingIntent);	// vrijeme remindera je mindif minuta prije alarma
			Log.Debug ("alar5","suuu");
		
			Log.Debug ("alar6","suuu");
			}catch(System.Exception e){
				Log.Debug ("sranjeee","suuu");
			}
			//TODO ,dodati vrijeme u Toast, Podsjetnk postavljen mindif minuta prije isteka roka <SAT_ZVONA_ALARMA>
			Toast.MakeText (Application.Context, "Podsjetnik postavljen " + mindif + " minuta prije isteka roka.", ToastLength.Long).Show ();
		}

	// TODO metodu deleteAlarm treba pozvati u klasi za broadcast
	// briše pojedinačni alarm
	public static void deleteAlarm(int idAlarma)
	{
		string dictString = prefsDict.GetString ("MyAktivniAlarmiPrefs", null);
		aktivniAlarmi = dictString.Split(';').Select(x => x.Trim().Split(':')).ToDictionary(x => Convert.ToInt32(x[0]), x => x[1]);
		aktivniAlarmi.Remove (idAlarma);
			//TODO nece radit jer nije osvjezeno u preferences da je alarm obrisan
	}


	// otkazuje (ne briše!) pojedinačni alarm i njegov reminder
	public static void cancelAlarm(int idAlarma)
	{
		Intent intentAlarm = new Intent (Application.Context, typeof(NotifAlarmBroadcast));
		Intent intentReminder = new Intent (Application.Context, typeof(NotifReminderBroadcast));

		PendingIntent senderAlarm = PendingIntent.GetBroadcast (Application.Context, idAlarma, intentAlarm, PendingIntentFlags.UpdateCurrent);
		PendingIntent senderReminder = PendingIntent.GetBroadcast (Application.Context, idAlarma+1, intentReminder, PendingIntentFlags.UpdateCurrent);

		AlarmManager alarmManager = (AlarmManager)Application.Context.ApplicationContext.GetSystemService (Context.AlarmService);
		alarmManager.Cancel (senderAlarm);
		alarmManager.Cancel (senderReminder);
	}


	// nastavlja (ne stvara!) pojedinačni alarm i njegov reminder
	public static void resumeAlarm(int idAlarma)
	{
		string dictString = prefsDict.GetString ("MyAktivniAlarmiPrefs", null);
		aktivniAlarmi = dictString.Split(';').Select(x => x.Trim().Split(':')).ToDictionary(x => Convert.ToInt32(x[0]), x => x[1]);

		string[] split = aktivniAlarmi [idAlarma].Split ('&');			
			// u popisu (vremenski) aktivnih alarma nalazi preko idAlarma odgovarajući alarm,
		// splita mu vrijednost i uzima prva 2 dijela - hours i minutes 
		Java.Util.Calendar cal = setCalendars (split [0], split [1]);
		Intent intentAlarm = new Intent (Application.Context, typeof(NotifAlarmBroadcast));
		Intent intentReminder = new Intent (Application.Context, typeof(NotifReminderBroadcast));

		PendingIntent alarmPendingIntent = PendingIntent.GetBroadcast (Application.Context, idAlarma, intentAlarm, PendingIntentFlags.UpdateCurrent);
		PendingIntent reminderPendingIntent = PendingIntent.GetBroadcast (Application.Context, idAlarma+1, intentReminder, PendingIntentFlags.UpdateCurrent);

		AlarmManager am = (AlarmManager)Application.Context.ApplicationContext.GetSystemService (Context.AlarmService);
		am.SetExact (AlarmType.RtcWakeup, cal.TimeInMillis, alarmPendingIntent);
		am.SetExact (AlarmType.RtcWakeup, (cal.TimeInMillis)-((Convert.ToInt64(mindif))*60*1000), reminderPendingIntent);
	}

	}
}

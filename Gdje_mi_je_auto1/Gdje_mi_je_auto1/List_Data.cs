using System;

namespace Gdje_mi_je_auto1
{
	/**
	 * Klasa koja puni listu sa podacima. Podatke povlaci iz sms inbox-a klasom History
	 * */
	public class List_Data
	{
		string[] items;

		public static string[] List_Data_Fill ()
		{
			List_Data data = new List_Data ();
			return data.items = new string[] { "21.04.2015 22:14-23:15 ", "21.04.2015 [22:14 - 23:15]", "11.04.2015 [12:14 - 13:15]", "17.03.2015 [16:10 - 19:10]", "15.03.2015 [08:10 - 09:10]", "Buildings",
				"Vegetables", "Fruits", "Flower", "Meats", "Baals", "Buildings","Vegetables", "Fruits", "Flower", "Meats", "Baals", "Buildings",
				"Vegetables", "Fruits", "Flower", "Meats", "Baals", "Buildings" };
		}
	}
}


using System;
using Android.Util;
namespace Gdje_mi_je_auto1
{
	/*
	 * Primjer moje iznimke.
	 * */
	public class LayoutNotConnected:Exception
	{
		/*
		 * Testna implementacija vlastitog exceptiona.
		 * */
		public LayoutNotConnected ():base()
		{
			Log.Debug ("Layout nije spojen s pritiskom na listu.","Not connected");
		}
	}
}



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

namespace Gdje_mi_je_auto1
{
	public class ListSection
	{
		
		String _columnHeader1;
		 BaseAdapter _adapter;
		//ImageView image;

		public ListSection( String columnHeader1, BaseAdapter adapter)
		{
			
			_columnHeader1 = columnHeader1;
			//this.image = image;
			_adapter = adapter;
		}
		//public String Caption { get { return _caption; } set { _caption = value; } }
		public String ColumnHeader1 { 
			get { return _columnHeader1; } 
			set { _columnHeader1 = value; } }

//		public ImageView Image { 
//			get { return image; } 
//			set { image = value; } }
		
		public BaseAdapter Adapter { 
			get { return _adapter; } 
			set { _adapter = value; } }
	}
}
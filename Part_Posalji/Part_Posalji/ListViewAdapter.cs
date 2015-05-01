using Android.App;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;
using Android.Util;

/*
 * Klasa u kojoj definiram adaper za list view. 
 * Skroz je neovisna i definiraju je ulazni parametri. 
 * Predaju se samo podaci koji ce se ispisat.
 * */
namespace Part_Posalji
{
	public class ListViewAdapter:BaseAdapter<string>
	{
		string[] items;
		Activity context;
		int viewId;

		 
		public ListViewAdapter ():base()
		{
			
		}

		public ListViewAdapter (Activity context, string[] items):base()
		{
			this.context = context;
			this.items=items;
		}

		public override long GetItemId(int position){
			return position;
		}

		//return data associated with a particular row number
		public override string this[int position]
		{
			get{ return items [position]; }
		}

		//to tell the control how many rows are in the data
		public override int Count
		{	
			get {return items.Length; }
		}

		/**
		 * metoda u kojoj se stvara view 
		 * */
		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			View view = convertView;
			if (view == null)//iskoristava se postojeci view
				view = context.LayoutInflater.Inflate (Android.Resource.Layout.TestListItem, null);
			view.FindViewById<TextView> (Android.Resource.Id.Text1).Text = items[position];

			return view;
		}

		public static int GetViewId
		{
			
			get{ 
				ListViewAdapter ada = new ListViewAdapter();
				return ada.viewId;
			}
		}

	}
}


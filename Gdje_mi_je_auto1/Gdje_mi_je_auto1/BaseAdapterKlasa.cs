using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Threading;
using System.Globalization;
using System.Diagnostics;
using System.IO;
using System.Xml;
using Android.Util;
using System.Text;
using System.Collections.Generic;
using System.Resources;
using System.Windows.Input;
using System.Windows;
using Android.Appwidget;
using System.Linq;
using System.Net;

namespace Gdje_mi_je_auto1
{
	[Activity (Label = "BaseAdapterKlasa")]			
	public class BaseAdapterKlasa : BaseAdapter<string>
	{

				string[] items;
				Activity context;


				public BaseAdapterKlasa(Activity context, string[] items):base(){
					this.context = context;
					this.items = items;
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

				//to return a view for each row,populated with data.
				public override View GetView(int position, View convertView, ViewGroup parent)
				{
					View view = convertView; //re-use an existing view, if one is available
					if(view==null)//otherwise create another one
				view = context.LayoutInflater.Inflate (Android.Resource.Layout.SimpleExpandableListItem2, null);
					view.FindViewById<TextView> (Android.Resource.Id.Text1).Text = items[position];//glavni natpis
					//view.FindViewById<TextView> (Android.Resource.Id.Text2).Text = items[position];

					return view;

				}

		void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
		{
//			var listview = sender as ListView;
//			var t = tableItems [e.Position];
//			Android.Widget.Toast.MakeText (this,t.Heading,Android.Widget.ToastLength.Short).Show ();

		}


			}
		}


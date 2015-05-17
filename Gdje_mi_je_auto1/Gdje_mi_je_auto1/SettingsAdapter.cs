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
using Android.Util;


namespace Gdje_mi_je_auto1
{

	public class SettingsAdapter : BaseAdapter<ListSection>
		{
			private const int TYPE_SECTION_HEADER = 0;
			private Context context;
			private List<ListSection> sections;
			private LayoutInflater inflater;

			public SettingsAdapter(Context context)
			{
				this.context = context;
				this.inflater =LayoutInflater.From(context);
				this.sections = new List<ListSection>();
			}

			public List<ListSection> Sections { 
				get { return sections; } 
				set { sections = value; } 
			}

			//index podatka
			public override ListSection this[int index] { 
				get { return sections[index]; } 
			}

			//broji podatke za prikaz
			public override int Count
			{
				get
				{
					int count = 0;
					foreach(ListSection s in sections) 
					count += s.Adapter.Count + 1;
					return count;
				}
			}
			
			public override int ViewTypeCount
			{
				get
				{
					int viewTypeCount = 1;
					foreach(ListSection s in sections) 
					viewTypeCount += s.Adapter.ViewTypeCount;
					return viewTypeCount;
				}
			}
			
			

			//public override bool  AreAllItemsEnable() { return false; }

			public override int GetItemViewType(int position)
			{
				int typeOffset = TYPE_SECTION_HEADER + 1;
				foreach(ListSection s in sections)
				{
					if(position == 0) return TYPE_SECTION_HEADER;
					int size = s.Adapter.Count + 1;
				if (position < size) {
					return (typeOffset + s.Adapter.GetItemViewType (position - 1));
				}
					position -= size;
					typeOffset += s.Adapter.ViewTypeCount;
				}
				return -1;
			}

			
			
			//dodaje podatak
		public void AddSection( String columnHeader1 ,BaseAdapter adapter)
			{
			sections.Add(new ListSection( columnHeader1, adapter));
			}
			
			//prikazuje podatak
			public override View GetView(int position, View convertView, ViewGroup parent)
			{
				View view = convertView;
				foreach(ListSection s in sections)
				{
					if(position == 0){
					if (view == null || !(view is LinearLayout)) {
						view = inflater.Inflate (Resource.Layout.SettingsSeparator, parent, false);
					}
				
						TextView columnHeader1 = view.FindViewById<TextView>(Resource.Id.columnHeader1);
						columnHeader1.Text = s.ColumnHeader1;
						return view;
					}
					int size = s.Adapter.Count + 1;
					if(position < size) return s.Adapter.GetView(position - 1, convertView, parent);
					position -= size;
				}
				return null;
			}

			public override Java.Lang.Object GetItem(int position)
			{
				foreach(ListSection s in sections)
				{
					if(position == 0) return null;
					int size = s.Adapter.Count + 1;
					if(position < size) return s.Adapter.GetItem(position);
					position -= size;
				}
				return null;
			}

			public override long GetItemId(int position) { 
				return position; 
			}

		}
	}


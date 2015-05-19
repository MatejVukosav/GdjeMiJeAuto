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
using Android.Graphics.Drawables;
using Android.Graphics;

namespace Gdje_mi_je_auto1
{
	/**
	 * Open dialog based on layoutId
	 * 
	 * FragmentTransaction ftr = FragmentManager.BeginTransaction ();
			ShowDialog ch=new ShowDialog(Resource.Layout.dialog_fragment_layout);
			ch.Show (ftr,"dinamooo");
	 * */
	public class ShowDialog:DialogFragment
	{
		int layoutId;

		public ShowDialog(int layoutId){
			this.layoutId = layoutId;
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			Dialog.Window.RequestFeature(WindowFeatures.NoTitle);
			var view = inflater.Inflate(layoutId, container, false);
			//Dialog.Window.SetLayout(300,300 ); //LinearLayout.LayoutParams.WrapContent
			//Dialog.Window.SetBackgroundDrawable(new ColorDrawable(Color.Transparent)); prozirno

		return view;
		}

		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

		}
			

			
	}
		
}




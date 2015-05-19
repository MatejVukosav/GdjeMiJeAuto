package md5cd61a8e1f9f2291d7d02a5b72739b7e4;


public class Start_Main
	extends android.app.Activity
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("Gdje_mi_je_auto1.Start_Main, Gdje_mi_je_auto1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", Start_Main.class, __md_methods);
	}


	public Start_Main () throws java.lang.Throwable
	{
		super ();
		if (getClass () == Start_Main.class)
			mono.android.TypeManager.Activate ("Gdje_mi_je_auto1.Start_Main, Gdje_mi_je_auto1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);

	java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}

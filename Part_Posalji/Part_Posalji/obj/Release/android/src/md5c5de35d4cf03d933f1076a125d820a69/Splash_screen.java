package md5c5de35d4cf03d933f1076a125d820a69;


public class Splash_screen
	extends android.app.Activity
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("Part_Posalji.Splash_screen, Part_Posalji, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", Splash_screen.class, __md_methods);
	}


	public Splash_screen () throws java.lang.Throwable
	{
		super ();
		if (getClass () == Splash_screen.class)
			mono.android.TypeManager.Activate ("Part_Posalji.Splash_screen, Part_Posalji, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
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

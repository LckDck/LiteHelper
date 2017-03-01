using System;
using Foundation;

namespace LiteHelper.iOS.InterfaceImplementation
{
	public class InternalStorage : IInternalStorage
	{
		public InternalStorage ()
		{
			var defaults = NSUserDefaults.StandardUserDefaults;

			NSDictionary defaultPrefs = NSDictionary.FromObjectsAndKeys (new [] { NSObject.FromObject (true), NSObject.FromObject (4) }, new [] { NSObject.FromObject (true), NSObject.FromObject (1) });

			defaults.RegisterDefaults (defaultPrefs);
		}

		public void Store (string key, object data)
		{
			var defaults = NSUserDefaults.StandardUserDefaults;

			string dataString = data as string;
			if (!string.IsNullOrEmpty (dataString)) {
				defaults.SetString (dataString, key);
			}

			if (data is bool) {
				defaults.SetBool ((bool)data, key);
			}

			if (data is int) {
				defaults.SetInt ((int)data, key);
			}

			defaults.Synchronize ();
		}

		public string RetrieveString (string key)
		{
			var defaults = NSUserDefaults.StandardUserDefaults;
			return defaults.StringForKey (key);
		}

	}
}

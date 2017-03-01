using System;
using Android.App;
using Android.Content;

namespace LiteHelper.Droid.InterfaceImplementation
{
	public class InternalStorage : IInternalStorage
	{
		
		private ISharedPreferences Preferences {
			get {
				return Application.Context.GetSharedPreferences (Application.Context.PackageName + ".InternalStorage", FileCreationMode.Private);
			}
		}

		public void Store (string key, object data)
		{
			using (var editor = Preferences.Edit ()) {
				string dataString = data as string;
				if (!string.IsNullOrEmpty (dataString)) {
					editor.PutString (key, dataString);
				}

				if (data is bool) {
					editor.PutBoolean (key, (bool)data);
				}

				if (data is int) {
					editor.PutInt (key, (int)data);
				}

				editor.Commit ();
			}
		}

		public string RetrieveString (string key)
		{
			return Preferences.GetString (key, string.Empty);
		}

	}
}

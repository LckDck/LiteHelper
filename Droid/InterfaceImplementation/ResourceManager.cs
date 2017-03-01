using System;
using System.Linq;
using Android.OS;
using LiteHelper.Interfaces;

namespace LiteHelper.Droid.InterfaceImplementation
{
	public class ResourceManager : IResourceManager
	{
		public ResourceManager ()
		{
		}

		public string BaseUrl {
			get {
				return "file:///android_asset/";
			}
		}

		public string OSVersion {
			get {
				return Android.OS.Build.VERSION.Release;
			}
		}

		public string DeviceName {
			get {
				string manufacturer = Build.Manufacturer;
				string model = Build.Model;
				if (model.StartsWith (manufacturer)) {
					return Utils.Capitalize (model);
				} else {
					return Utils.Capitalize (manufacturer) + " " + model;
				}
			}
		}

	}
}

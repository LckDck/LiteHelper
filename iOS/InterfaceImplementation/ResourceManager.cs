using System;
using Foundation;
using LiteHelper.Interfaces;
using UIKit;

namespace LiteHelper.iOS.InterfaceImplementation
{
	
	public class ResourceManager : IResourceManager
	{
		public string BaseUrl {
			get {
				return NSBundle.MainBundle.BundlePath;
			}
		}

		public string DeviceName {
			get {
				return UIDevice.CurrentDevice.SystemVersion;
			}
		}

		public string OSVersion {
			get {
				return UIDevice.CurrentDevice.SystemVersion;
			}
		}
	}
}

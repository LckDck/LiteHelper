using System;
using System.IO;
using System.Net;
using System.Net.Http;
using LiteHelper.Interfaces;
using Microsoft.Practices.ServiceLocation;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;

namespace LiteHelper
{
	public partial class MenuPage : ContentPage
	{
		IResourceManager _resoruceManager;

		public MenuPage ()
		{
			InitializeComponent ();
			_resoruceManager = ServiceLocator.Current.GetInstance<IResourceManager> ();
		}

		private void OnTapped (object sender, EventArgs args)
		{
			Device.OpenUri (new Uri ("mailto:" + Constants.SupportEmail + "?subject=" + Constants.AppDisplayName + "&body=" + GetBody ()));
		}

		protected override void OnAppearing ()
		{
			base.OnAppearing ();
			ContactDeveloper.Clicked += OnTapped;
		}

		protected override void OnDisappearing ()
		{
			base.OnDisappearing ();
			ContactDeveloper.Clicked -= OnTapped;
		}


		private string GetBody ()
		{
			var resourceManager = ServiceLocator.Current.GetInstance<IResourceManager> ();
			var result = String.Empty;

			result += "\n\n\n";
			result += "-----------\n";
			result += "OS Version: " + Device.RuntimePlatform + " " + resourceManager.OSVersion + "\n";
			result += "Model : " + _resoruceManager.DeviceName + "\n";


			return result;
		}



}
}

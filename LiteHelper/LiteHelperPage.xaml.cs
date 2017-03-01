using System;
using System.IO;
using System.Net;
using System.Net.Http;
using Xamarin.Forms;

namespace LiteHelper
{
	public partial class LiteHelperPage : ContentPage
	{
		public LiteHelperPage ()
		{
			InitializeComponent ();
		}

		protected override void OnAppearing ()
		{
			base.OnAppearing ();
			WebView.Navigating += OnNavigating;
		}


		protected override void OnDisappearing ()
		{
			base.OnDisappearing ();
			WebView.Navigating += OnNavigating;
		}

		void OnNavigating (object sender, WebNavigatingEventArgs e)
		{
			if (e.Url.StartsWith ("http")) {
				try {
					var uri = new Uri (e.Url);
					Device.OpenUri (uri);
				} catch (Exception) {
				}

				e.Cancel = true;
			}

			if (e.Url.StartsWith ("file")){
				e.Cancel = true;
			}
		}
}
}

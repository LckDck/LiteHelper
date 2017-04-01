using System;
using System.ComponentModel;
using Android.Views;
using LiteHelper.Droid.Renderers;
using LiteHelper.Managers;
using Microsoft.Practices.ServiceLocation;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer (typeof (WebView), typeof (ZoomableWebViewRenderer))]
namespace LiteHelper.Droid.Renderers
{
	
	public class ZoomableWebViewRenderer : WebViewRenderer, Android.Views.View.IOnScrollChangeListener
	{
		RefreshManager _refreshManager;


		public ZoomableWebViewRenderer () {
			_refreshManager = ServiceLocator.Current.GetInstance<RefreshManager> ();
		}

		public void OnScrollChange (Android.Views.View v, int scrollX, int scrollY, int oldScrollX, int oldScrollY)
		{
			_refreshManager.DispatchScrollChanged (Control.ScrollY);
		}

		protected override void OnElementPropertyChanged (object sender, PropertyChangedEventArgs e)
		{
			if (Control != null) {
				Control.Settings.BuiltInZoomControls = true;
				Control.Settings.DisplayZoomControls = false;
				Control.SetOnScrollChangeListener(this);
			}
			base.OnElementPropertyChanged (sender, e);
		}

	
	}
}

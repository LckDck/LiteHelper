using System;
using System.ComponentModel;
using LiteHelper.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer (typeof (WebView), typeof (ZoomableWebViewRenderer))]
namespace LiteHelper.Droid.Renderers
{
	
	public class ZoomableWebViewRenderer : WebViewRenderer
	{

		protected override void OnElementPropertyChanged (object sender, PropertyChangedEventArgs e)
		{
			if (Control != null) {
				Control.Settings.BuiltInZoomControls = true;
				Control.Settings.DisplayZoomControls = false;
			}
			base.OnElementPropertyChanged (sender, e);
		}

	}
}

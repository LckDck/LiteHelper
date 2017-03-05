
using System.ComponentModel;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Widget;
using LiteHelper.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Label), typeof(HtmlLabelRenderer))]

namespace LiteHelper.Droid.Renderers
{
	class HtmlLabelRenderer : LabelRenderer
	{
		protected override void OnElementChanged (ElementChangedEventArgs<Label> e)
		{
			base.OnElementChanged (e);
			var text = Element.Text ?? string.Empty;
			if (Control == null) return;

			Control.TextFormatted = Html.FromHtml (text);
		}

		//public static ISpanned FromHtml (string html)
		//{
		//	if (((int)Build.VERSION.SdkInt) >= 24) {
		//		return Html.FromHtml (html, FromHtmlOptions.ModeLegacy); // SDK >= Android N
		//	} else {
		//		return Html.FromHtml (html);
		//	}
		//}

		protected override void OnElementPropertyChanged (object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged (sender, e);

			if (e.PropertyName == Label.TextProperty.PropertyName) {
				Control?.SetText (Html.FromHtml (Element.Text), TextView.BufferType.Spannable);
			}
		}
	}
}

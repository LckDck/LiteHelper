using System;
using LiteHelper.Controls;
using LiteHelper.iOS;
using LiteHelper.iOS.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer (typeof (CustomEntry), typeof (CustomEnrtyRenderer))]
namespace LiteHelper.iOS
{
	public class CustomEnrtyRenderer : EntryRenderer
	{
		public CustomEnrtyRenderer ()
		{
		}

		protected override void OnElementChanged (ElementChangedEventArgs<Entry> e)
		{
			base.OnElementChanged (e);
			Control.TouchUpInside += OnTouchUpInside;
		}

		void OnTouchUpInside (object sender, EventArgs e)
		{
			var position = Control.GetPosition (Control.BeginningOfDocument, 0);
		}

		protected override void OnElementPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged (sender, e);
			if (e.PropertyName == Entry.TextProperty.PropertyName) {
				Control.ResignFirstResponder ();
			}
		}
	}
}

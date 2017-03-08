
using System;
using Android.Text;
using Android.Views;
using Android.Views.InputMethods;
using LiteHelper.Controls;
using LiteHelper.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Entry), typeof(CommonEntryRenderer))]
namespace LiteHelper.Droid.Renderers
{
	public class CommonEntryRenderer : EntryRenderer
										
	{

		protected override void OnElementChanged (ElementChangedEventArgs<Xamarin.Forms.Entry> e)
		{
			base.OnElementChanged (e);

			if (Control == null) {
				return;
			}

			this.Control.SetBackgroundColor (Color.Transparent.ToAndroid ());
		}

	

	}
}

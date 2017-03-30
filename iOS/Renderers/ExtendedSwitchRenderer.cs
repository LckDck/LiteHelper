using System;
using LiteHelper.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;


[assembly: ExportRenderer (typeof (Switch), typeof (ExtendedSwitchRenderer))]
namespace LiteHelper.iOS.Renderers
{
	public class ExtendedSwitchRenderer  : SwitchRenderer
	{
		public ExtendedSwitchRenderer ()
		{
		}

		protected override void OnElementChanged (ElementChangedEventArgs<Switch> e)
		{
			base.OnElementChanged (e);
			Control.OnTintColor = Colors.AccentColor;
		}
	}
}

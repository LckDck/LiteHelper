using System;
using Android.Views;
using LiteHelper.Controls;
using LiteHelper.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer (typeof (LongClickButton), typeof (LongClickButtonRenderer))]
namespace LiteHelper.Droid.Renderers
{
	public class LongClickButtonRenderer : FlatButtonRenderer
	{

		protected override void OnElementChanged (ElementChangedEventArgs<Button> e)
		{
			base.OnElementChanged (e);

			if (Control == null) return;

			if (e.OldElement == null) {
				
				Control.LongClick += OnLongClick;
			}

			if (e.NewElement == null) {
				Control.LongClick -= OnLongClick;
			}

		}

		void OnLongClick (object sender, LongClickEventArgs e)
		{
			var view = Element as LongClickButton;
			if (view != null) {
				if (view.LongClickCommand != null) {
					view.LongClickCommand.Execute (null);
				}
			}
		}
}
}
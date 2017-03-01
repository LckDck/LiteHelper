

using LiteHelper.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Button),typeof(FlatButtonRenderer))]
namespace LiteHelper.Droid.Renderers
{
	public class FlatButtonRenderer : ButtonRenderer
	{
		protected override void OnElementChanged (ElementChangedEventArgs<Button> e)
		{
			base.OnElementChanged (e);
			if (Control != null) {
				Control.SetPadding (0, 0, 0, 0);
			}
		}

		protected override void OnDraw (Android.Graphics.Canvas canvas)
		{
			base.OnDraw (canvas);
		}
	}
}
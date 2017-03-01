
using LiteHelper.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Picker), typeof(ExtendedPickerRenderer))]
namespace LiteHelper.Droid.Renderers
{
	public class ExtendedPickerRenderer : PickerRenderer
	{

		protected override void OnElementChanged (ElementChangedEventArgs<Xamarin.Forms.Picker> e)
		{
			base.OnElementChanged (e);

			if (Control == null) {
				return;
			}

			this.Control.SetBackgroundColor (Color.Transparent.ToAndroid ());
		}
	}
}


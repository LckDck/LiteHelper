
using LiteHelper.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Entry), typeof(ExtendedEntryRenderer))]
namespace LiteHelper.Droid.Renderers
{
	public class ExtendedEntryRenderer : EntryRenderer
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

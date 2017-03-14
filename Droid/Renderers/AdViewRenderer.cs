using Android.Widget;
using Android.Gms.Ads;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using LiteHelper.Droid.Renderers;
using Android.Provider;

[assembly: ExportRenderer(typeof(LiteHelper.Controls.AdView), typeof(AdViewRenderer))]
namespace LiteHelper.Droid.Renderers
{
	public class AdViewRenderer : ViewRenderer<LiteHelper.Controls.AdView, AdView>
	{
		string adUnitId = string.Empty;
		AdSize adSize = AdSize.Banner;
		AdView adView;
		protected override AdView CreateNativeControl ()
		{
			if (adView != null)
				return adView;

			adUnitId = Forms.Context.Resources.GetString (Resource.String.ad_unit_id);
			adView = new AdView (Forms.Context);
			adView.AdSize = adSize;
			adView.AdUnitId = adUnitId;

			var adParams = new LinearLayout.LayoutParams (LayoutParams.WrapContent, LayoutParams.WrapContent);

			adView.LayoutParameters = adParams;

			string android_id = Settings.Secure.GetString (Context.ContentResolver, Settings.Secure.AndroidId);
			
			adView.LoadAd (new AdRequest.Builder ().Build ());
			return adView;
		}

		protected override void OnElementChanged (ElementChangedEventArgs<LiteHelper.Controls.AdView> e)
		{
			base.OnElementChanged (e);
			if (Control == null) {
				CreateNativeControl ();
				SetNativeControl (adView);
			}
		}
	}
}

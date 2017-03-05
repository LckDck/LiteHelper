

using System;
using System.Threading.Tasks;
using Android.OS;
using Android.Views;
using LiteHelper.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using LiteHelper.Controls;
using System.Threading;

[assembly: ExportRenderer(typeof(Button),typeof(FlatButtonRenderer))]
namespace LiteHelper.Droid.Renderers
{
	public class FlatButtonRenderer : ButtonRenderer
	{
		public FlatButtonRenderer () { 
			
		}

		protected override void OnElementChanged (ElementChangedEventArgs<Button> e)
		{
			base.OnElementChanged (e);
			if (Control == null) {
				return;
			}

			if (Control != null) {
				Control.SetPadding (0, 0, 0, 0);
			}

			if (Element != null) {
				Element.AnchorX = 0.48;
				Element.AnchorY = 0.48;
			}

			if (e.NewElement != null) {
				Element.Clicked += OnTouch;
			} else {
				Element.Clicked -= OnTouch;
			}
		}

		//Task _task;		

		CancellationTokenSource cts;

		async void OnTouch (object sender, EventArgs e)
		{
			if (Element == null) return;
			if (cts != null) {
				cts.Cancel ();
				cts = null;
			}
			Element.CancelAllAnimations ();
			cts = new CancellationTokenSource ();

			if (Utils.DoVibro) {
				MainActivity.Current.Vibrator.Vibrate (30);
			}

			try {
				await Task.Run (async () => {
					await Element.ScaleTo (0.9, 50, Easing.Linear);
					await Element.ScaleTo (1, 50, Easing.Linear);
				}, cts.Token);
			} catch (System.OperationCanceledException exep) {
				Console.WriteLine ("Task canceled. " + exep.Message);
			} catch (Exception ex) { 
				 Console.WriteLine ("ex {0}", ex.Message);
			}
		}

		protected override void OnDraw (Android.Graphics.Canvas canvas)
		{
			base.OnDraw (canvas);
		}
	}
}

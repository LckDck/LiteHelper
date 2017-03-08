
using System;
using Android.Text;
using Android.Views;
using Android.Views.InputMethods;
using Java.Lang;
using LiteHelper.Controls;
using LiteHelper.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(ExtendedEntryRenderer))]
namespace LiteHelper.Droid.Renderers
{
	public class ExtendedEntryRenderer : CommonEntryRenderer, 
										Android.Views.View.IOnTouchListener, 
										Android.Views.View.IOnFocusChangeListener,
										ITextWatcher
	{

		protected override void OnElementChanged (ElementChangedEventArgs<Xamarin.Forms.Entry> e)
		{
			base.OnElementChanged (e);

			if (Control == null) {
				return;
			}
			if (e.NewElement != null) {
				Control.SetOnTouchListener (this);
				Control.OnFocusChangeListener = this;
			}
			this.Control.SetBackgroundColor (Color.Transparent.ToAndroid ());
		}

		public bool OnTouch (Android.Views.View v, MotionEvent e)
		{
			v.OnTouchEvent (e);
			var imm = (InputMethodManager)v.Context.GetSystemService(Android.Content.Context.InputMethodService);
            if (imm != null) {
				imm.HideSoftInputFromWindow (v.WindowToken, 0);
			}
			Utils.LastSelection = Control.SelectionStart;
			return true;
		}


		public void OnFocusChange (Android.Views.View v, bool hasFocus) {
			Control.Activated = true;
			Control.Pressed = true;
			Control.SetSelection (Utils.LastSelection);
			System.Diagnostics.Debug.WriteLine ("OnFocusChange");
		}

		public void OnTextChanged (ICharSequence s, int start, int before, int count){
			Control.Activated = true;
			Control.Pressed = true;
			Control.SetSelection (Utils.LastSelection);
			System.Diagnostics.Debug.WriteLine ("OnTextChanged " + s + " " + Utils.LastSelection);
		}


	}
}

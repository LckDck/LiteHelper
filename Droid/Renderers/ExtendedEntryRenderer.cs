
using System;
using Android.Graphics;
using Android.Text;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
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

			this.Control.SetBackgroundColor (Android.Graphics.Color.Transparent);
		}

		public bool OnTouch (Android.Views.View v, MotionEvent e)
		{
			v.OnTouchEvent (e);
			var imm = (InputMethodManager)v.Context.GetSystemService(Android.Content.Context.InputMethodService);
			         if (imm != null) {
				imm.HideSoftInputFromWindow (v.WindowToken, 0);
			}

			if (e.EventTime == StrangeEventTime) {
				Control.SetSelection (Utils.LastSelection);
				ClearFocus ();
			} else {
				Utils.LastSelection = Control.SelectionStart;
			}

			return true;
		}


		public void OnFocusChange (Android.Views.View v, bool hasFocus) {
			if (!hasFocus) {
				Control.Activated = true;
				Control.Pressed = true;
				Control.SetSelection (Utils.LastSelection);
				System.Diagnostics.Debug.WriteLine ("OnFocusChange");
			} else { 
				var imm = (InputMethodManager)v.Context.GetSystemService (Android.Content.Context.InputMethodService);
				if (imm != null) {
					imm.HideSoftInputFromWindow (v.WindowToken, 0);
				}
			}
		}


		public override bool DispatchTouchEvent (MotionEvent ev)
		{
			InputMethodManager manager = (InputMethodManager)Control.Context.GetSystemService (Android.Content.Context.InputMethodService);
			manager.HideSoftInputFromWindow (this.WindowToken, 0);
				
			return base.DispatchTouchEvent (ev);
		}

		long StrangeEventTime = 10000000000;

		public void AfterTextChanged (IEditable s){

			MotionEvent motionEvent = MotionEvent.Obtain (
				0,
				StrangeEventTime,
				0,
				0,
				0,
				MetaKeyStates.MetaOn
			);

			Control.DispatchTouchEvent (motionEvent);
		}


	}
}

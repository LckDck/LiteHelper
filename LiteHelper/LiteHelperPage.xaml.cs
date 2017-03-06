﻿using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Http;
using Xamarin.Forms;

namespace LiteHelper
{
	public partial class LiteHelperPage : ContentPage
	{
		public LiteHelperPage ()
		{
			InitializeComponent ();

			// Store the inital value so we know what to what height to restore to

		}

		protected override void OnAppearing ()
		{
			base.OnAppearing ();
			WebView.Navigating += OnNavigating;
			StatusLabel.PropertyChanged += OnStatusLabelPropertyChanged;
			_initialHeight = topRow.Height.Value;
		}


		protected override void OnDisappearing ()
		{
			base.OnDisappearing ();
			WebView.Navigating += OnNavigating;
			StatusLabel.PropertyChanged += OnStatusLabelPropertyChanged;
		}

		void OnStatusLabelPropertyChanged (object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == Label.TextProperty.PropertyName) {
				AnimateRow (StatusLabel.Text == String.Empty);
			}
		}

		void OnNavigating (object sender, WebNavigatingEventArgs e)
		{
			if (e.Url.StartsWith ("http")) {
				try {
					var uri = new Uri (e.Url);
					Device.OpenUri (uri);
				} catch (Exception) {
				}

				e.Cancel = true;
			}

			if (e.Url.StartsWith ("file")){
				e.Cancel = true;
			}
		}


		private Animation _animation;
		private double _initialHeight;
		private bool LastHidden { get; set;}

		private void AnimateRow (bool hide)
		{
			if (!hide) {
				if (!LastHidden) return;
				LastHidden = false;
				// Move back to original height
				_animation = new Animation (
					(d) => topRow.Height = new GridLength (Clamp (d, 0, double.MaxValue)),
					0, _initialHeight, Easing.Linear, () => _animation = null);
			} else {
				if (LastHidden) return;
				LastHidden = true;
				// Hide the row
				_animation = new Animation (
					(d) => topRow.Height = new GridLength (Clamp (d, 0, double.MaxValue)),
					_initialHeight, 0, Easing.Linear, () => _animation = null);
			}

			_animation.Commit (this, "the animation");
		}

		// Make sure we don't go below zero
		private double Clamp (double value, double minValue, double maxValue)
		{
			if (value < minValue) {
				return minValue;
			}

			if (value > maxValue) {
				return maxValue;
			}

			return value;
		}
	}
}

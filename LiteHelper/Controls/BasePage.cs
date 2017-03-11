using System;
using Xamarin.Forms;
using Foundation.MVVM.ViewModels;

namespace LiteHelper.Controls
{
	public class BasePage : ContentPage
	{


		protected override void OnAppearing ()
		{
			base.OnAppearing ();
			var model = BindingContext as IViewModel;
			if (model != null) {
				var vm = model;
				vm.OnNavigatingFrom ();
			}
		}

		protected override void OnDisappearing ()
		{
			base.OnDisappearing ();
			var model = BindingContext as IViewModel;
			if (model != null) {
				var vm = model;
				vm.OnNavigatingTo ();
			}
		}
	}
}


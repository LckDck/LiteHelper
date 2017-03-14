using System;
using Android.Views.InputMethods;
using LiteHelper.Droid.Renderers;
using LiteHelper.Managers;
using Microsoft.Practices.ServiceLocation;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android.AppCompat;

[assembly: ExportRenderer (typeof (MasterDetailPage), typeof (MyMasterDetailPageRenderer))]
namespace LiteHelper.Droid.Renderers
{
	public class MyMasterDetailPageRenderer : MasterDetailPageRenderer
	{
		CodeManager _codeManager;

		public MyMasterDetailPageRenderer ()
		{
			_codeManager = ServiceLocator.Current.GetInstance<CodeManager> ();

		}

		void HideKeyboard (object sender, EventArgs e)
		{
			var imm = (InputMethodManager)MainActivity.Current.GetSystemService (Android.Content.Context.InputMethodService);
			var result = imm.HideSoftInputFromWindow (WindowToken, 0);
		}

		protected override void OnElementChanged (VisualElement oldElement, VisualElement newElement)
		{
			base.OnElementChanged (oldElement, newElement);
			if (newElement == null) { 
				_codeManager.MenuIsClosing -= HideKeyboard;
			}

			if (newElement != null) { 
				_codeManager.MenuIsClosing += HideKeyboard;
			}
		}
	}
}

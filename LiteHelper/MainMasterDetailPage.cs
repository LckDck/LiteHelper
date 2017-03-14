using System;
using System.ComponentModel;
using LiteHelper.Managers;
using Microsoft.Practices.ServiceLocation;
using Xamarin.Forms;

namespace LiteHelper
{
	public class MainMasterDetailPage : MasterDetailPage
	{
		CodeManager _codeManager;

		public MainMasterDetailPage ()
		{
			Detail = new LiteHelperPage ();
			var bc = new MainScreenViewModel ();
			Detail.BindingContext = bc;


			Master = new MenuPage { Title = "Sandwich"};
			Master.BindingContext = Detail.BindingContext;
			_codeManager = ServiceLocator.Current.GetInstance<CodeManager> ();

			//no unsubcribing, because this view is lifetime view
			bc.ShowMenuEvent += ShowMenu;
			IsPresentedChanged += OnIsPresentedChanged;
		}

		void ShowMenu (object sender, EventArgs e)
		{
			IsPresented = !IsPresented;
		}

		void OnIsPresentedChanged (object sender, EventArgs e)
		{
			if (!IsPresented) {
				_codeManager.DispatchMenuClosing ();
			}
		}

		public void SaveEverything () {
			var bc = Detail.BindingContext as MainScreenViewModel;
			if (bc != null) {
				bc.SaveEverything ();
			}
		}
	}
}

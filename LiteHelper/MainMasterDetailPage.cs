using System;
using Xamarin.Forms;

namespace LiteHelper
{
	public class MainMasterDetailPage : MasterDetailPage
	{
		public MainMasterDetailPage ()
		{
			Detail = new LiteHelperPage ();
			var bc = new MainScreenViewModel ();
			Detail.BindingContext = bc;

			bc.ShowMenuEvent += ShowMenu;

			Master = new MenuPage { Title = "Sandwich"};
			Master.BindingContext = Detail.BindingContext;

		}

		void ShowMenu (object sender, EventArgs e)
		{
			IsPresented = !IsPresented;
		}

		public void SaveEverything () {
			var bc = Detail.BindingContext as MainScreenViewModel;
			if (bc != null) {
				bc.SaveEverything ();
			}
		}
	}
}

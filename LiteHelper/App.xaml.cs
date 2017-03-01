using Xamarin.Forms;

namespace LiteHelper
{
	public partial class App : Application
	{
		//Россия, Украина, Молдавия, Казахстан, Литва, Эстония

		public App ()
		{
			InitializeComponent ();

			MainPage = new MainMasterDetailPage ();


		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			var page = MainPage as MainMasterDetailPage;
			if (page != null) {
				page.SaveEverything ();
			}

		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}

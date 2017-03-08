using System.IO;
using System.Net;
using System.Net.Http;
using Xamarin.Forms;

namespace LiteHelper.History
{
	public partial class HistoryPage : ContentPage
	{
		public HistoryPage ()
		{
			InitializeComponent ();
			BindingContext = new HistoryViewModel ();
		}
	}
}

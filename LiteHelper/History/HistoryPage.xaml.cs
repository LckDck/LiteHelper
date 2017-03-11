using System.IO;
using System.Net;
using System.Net.Http;
using LiteHelper.Controls;
using Xamarin.Forms;

namespace LiteHelper.History
{
	public partial class HistoryPage : BasePage
	{
		public HistoryPage ()
		{
			InitializeComponent ();
			BindingContext = new HistoryViewModel ();
		}
	}
}

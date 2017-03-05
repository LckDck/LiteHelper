using System;
using System.Windows.Input;
using Foundation.Commands;
using Foundation.MVVM.ViewModels;
using Xamarin.Forms;

namespace LiteHelper.History
{
	public class HistoryItem : ViewModelBase
	{
		public HistoryItem ()
		{
		}


		public string Code { get; set;}
		public string Status { get; set;}

		public string StatusText { get {
				return Status;
			} 
		}



	}
}

using System;
using System.Windows.Input;
using Foundation.Commands;
using Foundation.MVVM.ViewModels;
using LiteHelper.Managers;
using Microsoft.Practices.ServiceLocation;
using Xamarin.Forms;

namespace LiteHelper.History
{
	public class HistoryItem : ViewModelBase
	{
		public HistoryItem ()
		{
		}

		public string Code { get; set;}

		string _status;
		public string Status {
			get {
				return _status;
			}
			set {
				_status = value;
				RaisePropertyChanged (() => Status);
				RaisePropertyChanged (() => StatusText);
				RaisePropertyChanged (() => ResendVisible);
			}
		}

		public string StatusText { get {
				return Status;
			} 
		}

		public bool ResendVisible { 
			get {
				return StatusText == Constants.CodeStatusWrong
					                          || StatusText == Constants.CodeStatusSending
					                          || StatusText == Constants.CodeStatusTimeOut;
			}
		}


		ICommand _resendCommand;
		public ICommand ResendCommand { 
			get {
				return _resendCommand ?? (_resendCommand = new DelegateCommand(Resend));
			}
		}

		public ICommand Command { get; internal set; }

		void Resend (object obj)
		{
			Command.Execute (Code);
			Command = null;
		}

	}
}

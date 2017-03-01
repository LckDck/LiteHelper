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
		public AnswerStatus Status { get; set;}

		public string StatusText { get {
				switch (Status) {
				case AnswerStatus.Accepted:
					return "Принят";
					case AnswerStatus.NoResponse:
					return "Сбой отправки";
					case AnswerStatus.NotAccepted:
					return "Не принят.";
					case AnswerStatus.FalseCode:
					return "Не принят. Ложный код.";
					case AnswerStatus.TimeIsOut:
					return "Не принят. Время истекло.";
					case AnswerStatus.Duplicate:
					return "Не принят. Уже введен.";
				}
				return string.Empty;
			} 
		}


		public bool ResendVisible { 
			get {
				return Status == AnswerStatus.NoResponse;
			}
		}

		ICommand _resendCommand;
		public ICommand ResendCommand {
			get {
				return _resendCommand ?? (_resendCommand = new DelegateCommand ((obj) => {
					
				}));
			}
		}
	}
}

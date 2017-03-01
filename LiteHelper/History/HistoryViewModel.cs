using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Foundation.Commands;
using Foundation.MVVM.ViewModels;
using Xamarin.Forms;

namespace LiteHelper.History
{
	public class HistoryViewModel : ViewModelBase
	{
		public HistoryViewModel ()
		{
			HistoryList = new List<HistoryItem> {
				new HistoryItem { Code = "123drl123", Status = AnswerStatus.Accepted},
				new HistoryItem { Code = "123dl2l3", Status = AnswerStatus.NoResponse },
				new HistoryItem { Code = "123d123rl", Status = AnswerStatus.NotAccepted },
				new HistoryItem { Code = "123drl234242", Status = AnswerStatus.Accepted},
				new HistoryItem { Code = "123dr2984383l", Status = AnswerStatus.Accepted},
				new HistoryItem { Code = "123drl1246", Status = AnswerStatus.Accepted},
				new HistoryItem { Code = "123drl123023", Status = AnswerStatus.Accepted},
			};
		}

		public List<HistoryItem> HistoryList { get; set;}


		ICommand _closeCommand;
		public ICommand CloseCommand {
			get {
				return _closeCommand ?? (_closeCommand = new DelegateCommand ((obj) => {
					Application.Current.MainPage.Navigation.PopModalAsync ();
				}));
			}
		}

		ICommand _deleteCommand;
		public ICommand DeleteCommand {
			get {
				return _deleteCommand ?? (_deleteCommand = new DelegateCommand ((obj) => {
					Application.Current.MainPage.Navigation.PopModalAsync ();
				}));
			}
		}
	}
}

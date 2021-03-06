﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Foundation.Commands;
using Foundation.MVVM.ViewModels;
using LiteHelper.Managers;
using Microsoft.Practices.ServiceLocation;
using Xamarin.Forms;

namespace LiteHelper.History
{
	public class HistoryViewModel : ViewModelBase
	{
		CodeStorageManager _codeStorageManager;
		public HistoryViewModel ()
		{
			_codeStorageManager = ServiceLocator.Current.GetInstance<CodeStorageManager> ();
			_historyList = new ObservableCollection<HistoryItem> ();
			foreach (var item in _codeStorageManager.Codes) {
				_historyList.Add (new HistoryItem { Code = item.Code, Status = item.Status, Command = CloseCommand });
			}
		}

		public override void OnNavigatingFrom ()
		{
			base.OnNavigatingFrom ();
			_codeStorageManager.StatusChanged += OnCodeStatusChanged;
		}

		void OnCodeStatusChanged (object sender, CodeInfoEventArgs e)
		{
			var info = HistoryList.Where (item => item.Code == e.Code).ToList();
			if (info.Any()) {
				info.First().Status = e.Status;
			}
		}

		ObservableCollection<HistoryItem> _historyList;
		public ObservableCollection<HistoryItem> HistoryList { 
			get { 
				return _historyList; 
			} 
			set { 
				_historyList = value; 
			} 
		}


		protected override void NotifyCommandsCanExecuteChanged ()
		{
			base.NotifyCommandsCanExecuteChanged ();
			CloseCommand.NotifyCanExecuteChanged ();
		}

		DelegateCommand _closeCommand;
		public DelegateCommand CloseCommand {
			get {
				return _closeCommand ?? (_closeCommand = new DelegateCommand ((obj) => {
					BlockCommands ();
					var code = (string)obj;
					if (!string.IsNullOrEmpty (code)) {
						var codeManager = ServiceLocator.Current.GetInstance<CodeManager> ();
						codeManager.UpdateCode (code);
					}
					Application.Current.MainPage.Navigation.PopModalAsync ();
				}, CanExecutePredicate));
			}
		}

		public override void OnNavigatingTo ()
		{
			_codeStorageManager.StatusChanged -= OnCodeStatusChanged;
			base.OnNavigatingTo ();
		}

		ICommand _deleteCommand;
		public ICommand DeleteCommand {
			get {
				return _deleteCommand ?? (_deleteCommand = new DelegateCommand (async (obj) => {

					var yes = await Application.Current.MainPage.DisplayAlert (String.Empty, "Хотите очистить историю введенных кодов?", "Да", "Отмена");
					if (yes) {
						_codeStorageManager.Clear ();
						Refresh ();
					}
				}));
			}
		}

		void Refresh ()
		{
			HistoryList.Clear ();
			foreach (var item in _codeStorageManager.Codes) {
				HistoryList.Add (new HistoryItem { Code = item.Code, Status = item.Status });
			}
		}

}
}

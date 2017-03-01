

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Windows.Input;
using Foundation.Commands;
using Foundation.MVVM.ViewModels;
using LiteHelper.History;
using Microsoft.Practices.ServiceLocation;
using Xamarin.Forms;

namespace LiteHelper
{
	public class MainScreenViewModel : ViewModelBase
	{
		IInternalStorage _storage;
		List<string> _cityNames;
		public MainScreenViewModel ()
		{
			_storage = ServiceLocator.Current.GetInstance<IInternalStorage> ();
			_cityNames = new List<string> (Constants.CityList.Keys);
			var savedPin = _storage.RetrieveString (Constants.Pin);
			PIN = (string.IsNullOrEmpty(savedPin)) ? "1111111" : savedPin;

			var savedCity = _storage.RetrieveString (Constants.City);

			SelectedCity = (string.IsNullOrEmpty(savedCity)) ? Constants.DefaultCity : savedCity;

			var savedKey1 = _storage.RetrieveString (Constants.Key1);
			Prefix1 =  (string.IsNullOrEmpty(savedKey1)) ? Keyboard._0_0_Text : savedKey1;

			var savedKey2 = _storage.RetrieveString (Constants.Key2);
			Prefix2 = (string.IsNullOrEmpty (savedKey2)) ? Keyboard._0_1_Text : savedKey2;
			var savedKey3 = _storage.RetrieveString (Constants.Key3);
			Prefix3 = (string.IsNullOrEmpty (savedKey3)) ? Keyboard._0_2_Text : savedKey3;
			var savedKey4 = _storage.RetrieveString (Constants.Key4);
			Prefix4 = (string.IsNullOrEmpty (savedKey4)) ? Keyboard._0_3_Text : savedKey4;
			var savedKey5 = _storage.RetrieveString (Constants.Key5);
			Prefix5 = (string.IsNullOrEmpty (savedKey5)) ? Keyboard._0_4_Text : savedKey5;

			Load ();
		}

		async void Load ()
		{
			var httpClient = new HttpClient ();
			var html = await httpClient.GetStringAsync ($"http://lite.dzzzr.ru/{CityCode}/go/?pin={PIN}");
			System.Diagnostics.Debug.WriteLine (html);
			Source = new HtmlWebViewSource { Html = html};
		}


		string _pin;
		public string PIN {
			get {
				return _pin;
			}

			set {
				_pin = value;
				RaisePropertyChanged (() => PIN);
			}
		}

		HtmlWebViewSource _source;
		public HtmlWebViewSource Source {
			get {
				return _source;
			}

			set {
				_source = value;
				RaisePropertyChanged (() => Source);
			}
		}


		ICommand _sendCommand;
		public ICommand SendCommand {
			get {
				return _sendCommand ?? (_sendCommand = new DelegateCommand ((obj) => {
					Code = string.Empty;
					Refresh ();
				}));
			}
		}


		ICommand _refreshCommand;
		public ICommand RefreshCommand {
			get {
				return _refreshCommand ?? (_refreshCommand = new DelegateCommand (Refresh));
			}
		}



		void Refresh (object obj = null)
		{
			Load ();
		}

		ICommand _showMenuCommand;
		public ICommand ShowMenuCommand {
			get {
				return _showMenuCommand ?? (_showMenuCommand = new DelegateCommand ((obj) => {
					if (ShowMenuEvent != null) {
						ShowMenuEvent.Invoke (this, new EventArgs ());
					}
				}));
			}
		}

		ICommand _clearCommand;
		public ICommand ClearCommand {
			get {
				return _clearCommand ?? (_clearCommand = new DelegateCommand ((obj) => {
					Code = String.Empty;
				}));
			}
		}

		ICommand _backspaceCommand;
		public ICommand BackspaceCommand {
			get {
				return _backspaceCommand ?? (_backspaceCommand = new DelegateCommand ((obj) => {
					if (Code.Length == 0) return;
					Code = Code.Substring (0, Code.Length - 1);
				}));
			}
		}




		string _code;
		public string Code {
			get {
				return _code;
			}

			set {
				_code = value;
				RaisePropertyChanged (() => Code);
				RaisePropertyChanged (() => CodePlaceholder);
			}
		}


		public string CodePlaceholder {
			get {
				return string.IsNullOrEmpty (Code) ? "Введите код" : string.Empty;
			}
		}



		#region Key bindings
		ICommand __0_0_Command;
		public ICommand _0_0_Command {
			get {
				return __0_0_Command ?? (__0_0_Command = new DelegateCommand ((obj) => {
					Code += Prefix1;
				}));
			}
		}


		ICommand __0_1_Command;
		public ICommand _0_1_Command {
			get {
				return __0_1_Command ?? (__0_1_Command = new DelegateCommand ((obj) => {
					Code += Prefix2;
				}));
			}
		}


		ICommand __0_2_Command;
		public ICommand _0_2_Command {
			get {
				return __0_2_Command ?? (__0_2_Command = new DelegateCommand ((obj) => {
					Code += Prefix3;
				}));
			}
		}

		ICommand __0_3_Command;
		public ICommand _0_3_Command {
			get {
				return __0_3_Command ?? (__0_3_Command = new DelegateCommand ((obj) => {
					Code += Prefix4;
				}));
			}
		}

		ICommand __0_4_Command;
		public ICommand _0_4_Command {
			get {
				return __0_4_Command ?? (__0_4_Command = new DelegateCommand ((obj) => {
					Code += Prefix5;
				}));
			}
		}



		ICommand __0_Command;
		public ICommand _0_Command {
			get {
				return __0_Command ?? (__0_Command = new DelegateCommand ((obj) => {
					Code += Keyboard._0_Text;
				}));
			}
		}


		ICommand __1_Command;
		public ICommand _1_Command {
			get {
				return __1_Command ?? (__1_Command = new DelegateCommand ((obj) => {
					Code += Keyboard._1_Text;
				}));
			}
		}

		ICommand __2_Command;
		public ICommand _2_Command {
			get {
				return __2_Command ?? (__2_Command = new DelegateCommand ((obj) => {
					Code += Keyboard._2_Text;
				}));
			}
		}

		ICommand __3_Command;
		public ICommand _3_Command {
			get {
				return __3_Command ?? (__3_Command = new DelegateCommand ((obj) => {
					Code += Keyboard._3_Text;
				}));
			}
		}

		ICommand __4_Command;
		public ICommand _4_Command {
			get {
				return __4_Command ?? (__4_Command = new DelegateCommand ((obj) => {
					Code += Keyboard._4_Text;
				}));
			}
		}

		ICommand __5_Command;
		public ICommand _5_Command {
			get {
				return __5_Command ?? (__5_Command = new DelegateCommand ((obj) => {
					Code += Keyboard._5_Text;
				}));
			}
		}

		ICommand __6_Command;
		public ICommand _6_Command {
			get {
				return __6_Command ?? (__6_Command = new DelegateCommand ((obj) => {
					Code += Keyboard._6_Text;
				}));
			}
		}


		ICommand __7_Command;
		public ICommand _7_Command {
			get {
				return __7_Command ?? (__7_Command = new DelegateCommand ((obj) => {
					Code += Keyboard._7_Text;
				}));
			}
		}


		ICommand __8_Command;
		public ICommand _8_Command {
			get {
				return __8_Command ?? (__8_Command = new DelegateCommand ((obj) => {
					Code += Keyboard._8_Text;
				}));
			}
		}

		ICommand __9_Command;
		public ICommand _9_Command {
			get {
				return __9_Command ?? (__9_Command = new DelegateCommand ((obj) => {
					Code += Keyboard._9_Text;
				}));
			}
		}

		string _prefix1;
		public string Prefix1 { 
			get {
				return _prefix1;
			}

			set {
				_prefix1 = value;
				RaisePropertyChanged (()=> Prefix1);
			}
		}

		string _prefix2;
		public string Prefix2 {
			get {
				return _prefix2;
			}

			set {
				_prefix2 = value;
				RaisePropertyChanged (() => Prefix2);
			}
		}

		string _prefix3;
		public string Prefix3 {
			get {
				return _prefix3;
			}

			set {
				_prefix3 = value;
				RaisePropertyChanged (() => Prefix3);
			}
		}

		string _prefix4;
		public string Prefix4 {
			get {
				return _prefix4;
			}

			set {
				_prefix4 = value;
				RaisePropertyChanged (() => Prefix4);
			}
		}

		string _prefix5;
		public string Prefix5 {
			get {
				return _prefix5;
			}

			set {
				_prefix5 = value;
				RaisePropertyChanged (() => Prefix5);
			}
		}
		#endregion

		public void SaveEverything () {
			
			_storage.Store (Constants.Key1, Prefix1);
			_storage.Store (Constants.Key2, Prefix2);
			_storage.Store (Constants.Key3, Prefix3);
			_storage.Store (Constants.Key4, Prefix4);
			_storage.Store (Constants.Key5, Prefix5);

			_storage.Store (Constants.City, SelectedCity);
			_storage.Store (Constants.Pin, PIN);
		}

		List<string> _cities;
		public List<string> Cities { 
			get {
				return _cities ?? (_cities = new List<string>(Constants.CityList.Keys));
			}
		}


		private string CityCode { 
			get {
				string code;
				Constants.CityList.TryGetValue (SelectedCity, out code);
				if (string.IsNullOrEmpty (code)) {
					code = Constants.DefaultCityCode;
				}
				return code;
			}
		
		}

		string _selectedCity;
		public string SelectedCity {
			get {
				return _selectedCity;
			}

			set {
				_selectedCity = value;
				RaisePropertyChanged (() => SelectedCity);
				Refresh ();
			}
		}

		public int SelectedCityIndex { 
			get {
				return GetIndex ();
			}
		}


		int GetIndex ()
		{
			return _cityNames.IndexOf (SelectedCity);
		}

		public string DonateText { 
			get {
				return "Привет всем дозорным от команды Pepperhouse из Калининграда! " +
					"Если вам нравится это приложение, вы можете поддержать разработчика:";
			}
		}

		public event EventHandler ShowMenuEvent;

		ICommand _buyCommand;
		public ICommand BuyCommand {
			get {
				return _buyCommand ?? (_buyCommand = new DelegateCommand ((obj) => {

				}));
			}
		}


		ICommand _showHistoryCommand;
		public ICommand ShowHistoryCommand {
			get {
				return _showHistoryCommand ?? (_showHistoryCommand = new DelegateCommand ((obj) => {
					Application.Current.MainPage.Navigation.PushModalAsync (new HistoryPage());
				}));
			}
		}



	}
}



using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Foundation.Commands;
using Foundation.MVVM.ViewModels;
using LiteHelper.History;
using LiteHelper.Managers;
using Microsoft.Practices.ServiceLocation;
using ModernHttpClient;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace LiteHelper
{
	public class MainScreenViewModel : ViewModelBase
	{
		IInternalStorage _storage;
		List<string> _cityNames;

		CodeStorageManager _codeStorageManager;

		public MainScreenViewModel ()
		{
			_storage = ServiceLocator.Current.GetInstance<IInternalStorage> ();
			_codeStorageManager = ServiceLocator.Current.GetInstance<CodeStorageManager> ();
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

		async void Load (string html = null)
		{
			IsLoading = true;
			Source = null;

			if (html == null) {
				using (var httpClient = new HttpClient ()) {
					try {
						html = await httpClient.GetStringAsync (Constants.GetHtmlUrl (CityCode, PIN));
					} catch (Exception e) {
						Debug.WriteLine (e.Message);
						html = "Нет сети";
					}
					StatusText = string.Empty;
				}
			}
			Source = Constants.GetHtmlUrl (CityCode ,PIN);
			UpdateCodesInfo (html);
			IsLoading = false;
		}


		bool _isLoading;
		public bool IsLoading { 
			get {
				return _isLoading;
			}
			set {
				_isLoading = value;
				RaisePropertyChanged (() => IsLoading);
			}
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

		string _source;
		public string Source {
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
				return _sendCommand ?? (_sendCommand = new DelegateCommand (async (obj) => {
					if (!String.IsNullOrEmpty (Code)) {
						_codeStorageManager.AddCode (Code, "Отправляется...");
						IsLoading = true;
						var result = await SendCode ();
						Code = string.Empty;
						Refresh (result);
					}

				}));
			}
		}


		async Task<string> SendCode ()
		{
			var client = GetClient ();
			var code = Code;
			using (var message = new HttpRequestMessage (
				HttpMethod.Post,
				Constants.GetSendCodeUrl (CityCode, PIN)) {
				Content = new StringContent ($"pin={PIN}&cod={code}&action=entcod", Encoding.UTF8, "application/x-www-form-urlencoded")

			})
			try {
				using (var response = await client.SendAsync (message)) {
					var body = await response.Content.ReadAsStringAsync ();
					var status = GetStatusFor (code, body);
					_codeStorageManager.AddCode (Code, status);
					UpdateCodesInfo (body);
					StatusText = status;
					Debug.WriteLine (body);
					return body;
				}
			} catch (Exception e) {
				Debug.WriteLine (e.Message);
				return "Нет сети";
			}
		}

		void UpdateCodesInfo (string html)
		{
			_codesInfo = GetCodesInfoHtml (html);
			RaisePropertyChanged (() => CodesInfo);
		}

		string GetCodesInfoHtml (string body)
		{
			var prefix = "<html>" +
				"<head></head>"
				+ "<body>"
				+ "<span style='font-size:13px;'>";
			var postfix = 
				"</span>" +
				"</body>" +
				"</html>";

			var regex = new Regex ("(?<=<div class='dcodes'><strong>Коды сложности</strong><br>)(.*?)(?=</p></div>)");

			var match = regex.Match (body);
			if (match.Success) {
				var substring = match.Value;
				return prefix + substring + postfix;
			}

			return prefix + "Нет информации о кодах" + postfix;
		}

		string GetStatusFor (string code, string html)
		{
			var errorBegin = "<!--errorText--><p><strong>";
			var errorEnd = "</strong></p><!--errorTextEnd-->";
			var indexBegin = html.IndexOf (errorBegin);
			var indexEnd = html.IndexOf (errorEnd);
			if (indexEnd > -1 && indexBegin > -1) {
				var statusBeginIndex = indexBegin + errorBegin.Length;
				var length = indexEnd - statusBeginIndex;
				if (length > 0) {
					var status = html.Substring (statusBeginIndex, length);
					return status;
				}
			}
			return "Код не принят. Движок неактивен.";
		}

		NativeMessageHandler _requestHandler;
		private HttpClient GetClient ()
		{
			_requestHandler = new NativeMessageHandler ();
			var client = new HttpClient (_requestHandler) {
				Timeout = new TimeSpan (0, 1, 0)
			};
			client.DefaultRequestHeaders.Accept.Clear ();
			client.DefaultRequestHeaders.Accept.Add (new MediaTypeWithQualityHeaderValue ("application/x-www-form-urlencoded"));

			return client;
		}


		ICommand _refreshCommand;
		public ICommand RefreshCommand {
			get {
				return _refreshCommand ?? (_refreshCommand = new DelegateCommand (Refresh));
			}
		}



		void Refresh (object page = null)
		{
			Load ((string)page);
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




		string _statusText;
		public string StatusText {
			get {
				return _statusText;
			}

			set {
				_statusText = value;
				RaisePropertyChanged (() => StatusText);
				RaisePropertyChanged (() => StatusVisible);
			}
		}

		public bool StatusVisible { 
			get {
				return !string.IsNullOrEmpty (StatusText);
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


		bool? _doVibro;
		public bool DoVibro { 
			get {
				if(_doVibro.HasValue) {
					return _doVibro.Value;
				}
				Utils.DoVibro = _storage.RetrieveBool (Constants.DoVibro);
				_doVibro = Utils.DoVibro;
				return _doVibro.Value;
			}

			set {
				_doVibro = value;
				Utils.DoVibro = value;
				_storage.Store (Constants.DoVibro, _doVibro);
				RaisePropertyChanged (() => DoVibro);
			}
		}


		ICommand _changeVibrateCommand;
		public ICommand ChangeVibrateCommand {
			get {
				return _changeVibrateCommand ?? (_changeVibrateCommand = new DelegateCommand ((obj) => {
					DoVibro = !DoVibro;
				}));
			}
		}


		string _codesInfo;
		public HtmlWebViewSource CodesInfo { 
			get {
				return new HtmlWebViewSource { Html = _codesInfo };
			}
		}


		private string DebugBody = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01 Transitional//EN\">\n\n<html>\n<head>\n\t<META http-equiv=\"Content-Type\" Content=\"text/html; charset=windows-1251\">\n\t\t<meta http-equiv=\"Cache-Control\" content=\"max-age=0\" />\n\t<script src=\"timer.js\" type=\"text/javascript\"></script>\n\t<title>DozoR.Lite / Тестовая игра</title>\n\n\n<style>\nbody {\n\n\n\n\n}\n\n\n\n\n\n</style>\n\n</head>\n\n<body>\n\n\n\n<!--errorText--><p><strong>Код не принят.</strong></p><!--errorTextEnd--><p>Обновлено в 23:31:35<br /></p><!--contentBegin-->\n\t\n<!--gameType Through-->\n\n\n<p><strong>Задание <!--levelNumberBegin-->1<!--levelNumberEnd-->.  <!--bMin-->0<!--bMinE--> мин. (получено <!--bTime-->20:45:00<!--bTimeEnd-->):</strong><!--levelTextBegin--><p dir=\"ltr\">Экстрасенсам дано видеть намного больше обычного человека, зачастую у них в сознании вырисовываются странные картины. Вот одна из них:</p>\n<p dir=\"ltr\">Справа замок, слева круглое озеро, от замка до озера и вокруг него идет дорожка.</p><div class=spoiler><strong>Примечания к заданию</strong>: <!--taskNotes--><p dir=\"ltr\">Введите ответ в поле спойлера для получения проверочного кода и дальнейших указаний. Формат ответа: СЛОВО. &nbsp;На локации 13 кодов, для закрытия уровня достаточно ввести любые 10, включая проверочный. На уровне - бонусный код (1 минута), доступен до ввода основных кодов, код стандартный. Время на уровне увеличено.</p>\n<p dir=\"ltr\">P.S.: Не забудьте сдать агентам выданные на брифинге анкеты игроков (заявки)!&nbsp;</p><!--taskNotesEnd--></div><br/><!--levelTextEnd--><div style='font-size:120%;'><div class=spoiler><div class=title style='padding-left:0'>Спойлер</div><p dir=\"ltr\">Проверочный код: 157D15RL227</p>\n<p>На локации вас встретят агенты - выполняйте указания:</p>\n<p><img src=\"../../uploaded/kaliningrad/Lite/157DRL/BY_eUJIJoOk.jpg\" alt=\"\" width=\"450\" height=\"400\" /></p></div></div><!--bonusCodeCount 1--><!--mainCodeCount 14--><!--difficultyCods null,2,2,1+,1+,1+,1+,1+,1+,1+,1+,1+,1+,1+,1+--><div class='dcodes'><strong>Коды сложности</strong><br>основные коды: <span style='color:red'>null</span>, 2, 2, 1+, 1+, 1+, 1+, 1+, <span style='color:red'>1+</span>, <span style='color:red'>1+</span>, 1+, <span style='color:red'>1+</span>, <span style='color:red'>1+</span>, <span style='color:red'>1+</span><br>бонусные коды: 1+<br> (Всего - 14 , для прохождения достаточно любых 10 , принято - 7) <br/><strong>Бонусные коды</strong>: код сложности 1+ (1  мин.)<br /></p></div><p>Найденные коды: основные коды: 157D15RL227, 157DR131L, 157D1R47L4, 157DRL123456789123456789, 157DR1842L51, 157D13R5L7, 157DRL18265</p><p><strong>Подсказка l:</strong><br/><!--LevelClue1Text--><p><img src=\"../../uploaded/kaliningrad/Lite/157DRL/vghnmhjmnhyt.png\" alt=\"\" width=\"460\" height=\"260\" /></p><!--LevelClue1TextEnd--></p><p><strong>Подсказка 2:</strong><br/><!--LevelClue2Text--><p>Пароль к спойлеру: КЛЮЧ</p><!--LevelClue2TextEnd--></p><p>Введите код</p>\t<form  data-ajax=\"false\" method=post>\n   \t\t<input type=hidden name=\"nomessage\" value=\"\">\n\t\t<input type=hidden name=action value=entcod>\n\t\t<input type=hidden name=pin value=485315>\n\t\t<input type=hidden name=lev value=0>\n\t\t<input type=text id=\"levelCodeInput\" placeholder=\"код к заданию\" name=cod size=25 value=\"\"><br>\n\t\t\t\t<input id=\"sendCode\" type=submit value=\"отправить код\">\n\t</form>\n\t\n<!--contentEnd-->Время на уровне: <span id=clock2></span><br>\n\tВремя до окончания задания: <span id=clock>&nbsp;</span> \n\t    <script>\n    window.setTimeout('countDown(4406,9995)',1000); \n    </script>\n\t<!--timeOnLevelBegin 9995 timeOnLevelEnd-->\n\t<!--timeToFinishBegin 4406 timeToFinishEnd-->\n\t<!--LastLog--><div class=title>Последние три события игры</div><table cellpadding=3 cellspacing=0 border=1><tr><th>время</th><th>событие</th><th>данные</th></tr><tr bgcolor=><td nowrap>23:31:32</td><td>передан неверный код</td><td>159DRL&nbsp;</td></tr><tr bgcolor=><td nowrap>23:31:03</td><td>принят код</td><td>157DRL18265&nbsp;</td></tr><tr bgcolor=><td nowrap>23:30:53</td><td>принят код</td><td>157D13R5L7&nbsp;</td></tr></table><!--LastLogEnd--><br>\n\t\t<form method=get data-ajax=\"false\">\n   \t\t<input type=hidden name=\"pin\" value=\"485315\">\n   \t\t<input type=hidden  name=\"q\" value=\"44272283OKOFGTAI\"/>\n   \t\t<input type=hidden  name=\"zone\" value=\"\"/>\n   \t\t<input type=hidden  name=\"nomessage\" value=\"\"/>\n\t\t<input type=submit value=\"обновить страницу\">\n\t\t</form>\n\t\t<form method=get name=refr data-ajax=\"false\">\n   \t\t<input type=hidden name=\"pin\" value=\"485315\">\n   \t\t<input type=hidden  name=\"q\" value=\"44272283OKOFGTAI\"/>\n   \t\t<input type=hidden  name=\"nomessage\" value=\"\"/>\n\t\t<input type=submit value=\"обновлять автоматически через\"> <input type=text name=refresh size=3 value=\"0\"> мин.\n\t\t</form>\n<p>Таймер браузера работает независимо от сервера! Используйте авто-обновление страницы для его синхронизации.\n<!--gameID248--><style>\n.teamstat td, .teamstat th  {\n\tfont-family : Arial;\n\tfont-size : 12px;\n}\n\n.teamstat table, .teamstat table td, .teamstat table th {\n  border: 1px solid gray;\n  border-collapse: collapse;\n}\n\n.teamstat table td, .teamstat table th {\n\tpadding : 2px 2px 2px 2px;\n}\n</style>\n<hr/><p><b>Общение с организатором</b><br/><form method=post>\n\t\t\t<input type=hidden name=action value=message>\n\t\t\t<input type=hidden name=gameData[id] value=248>\n\t\t\t<input type=hidden name=pin value=485315>\n\t\t\t<textarea name=content rows=5 cols=30></textarea><br>\n\t\t\t<input type=submit value='отправить сообщение'>\n\t\t</form><p>Команда: <strong>Pepperhouse</strong><!--<br>Игрок: <strong></strong>--><br>Игра: <strong>Тестовая игра. Начало в 2017-03-03 20:45:00. </strong><br>Организатор: <strong>Надежда Елисеева</strong>. ICQ: <strong>291-377-249</strong>. tel: <strong>+7(911)4820353</strong></p>\n\n\n\n</body>\n</html>";
		private string DebugBodyNoError = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01 Transitional//EN\">\n\n<html>\n<head>\n\t<META http-equiv=\"Content-Type\" Content=\"text/html; charset=windows-1251\">\n\t\t<meta http-equiv=\"Cache-Control\" content=\"max-age=0\" />\n\t<script src=\"timer.js\" type=\"text/javascript\"></script>\n\t<title>DozoR.Lite / Тестовая игра</title>\n\n\n<style>\nbody {\n\n\n\n\n}\n\n\n\n\n\n</style>\n\n</head>\n\n<body>\n\n\n\n<!--errorText--><p><!--errorTextEnd--><p>Обновлено в 23:31:35<br /></p><!--contentBegin-->\n\t\n<!--gameType Through-->\n\n\n<p><strong>Задание <!--levelNumberBegin-->1<!--levelNumberEnd-->.  <!--bMin-->0<!--bMinE--> мин. (получено <!--bTime-->20:45:00<!--bTimeEnd-->):</strong><!--levelTextBegin--><p dir=\"ltr\">Экстрасенсам дано видеть намного больше обычного человека, зачастую у них в сознании вырисовываются странные картины. Вот одна из них:</p>\n<p dir=\"ltr\">Справа замок, слева круглое озеро, от замка до озера и вокруг него идет дорожка.</p><div class=spoiler><strong>Примечания к заданию</strong>: <!--taskNotes--><p dir=\"ltr\">Введите ответ в поле спойлера для получения проверочного кода и дальнейших указаний. Формат ответа: СЛОВО. &nbsp;На локации 13 кодов, для закрытия уровня достаточно ввести любые 10, включая проверочный. На уровне - бонусный код (1 минута), доступен до ввода основных кодов, код стандартный. Время на уровне увеличено.</p>\n<p dir=\"ltr\">P.S.: Не забудьте сдать агентам выданные на брифинге анкеты игроков (заявки)!&nbsp;</p><!--taskNotesEnd--></div><br/><!--levelTextEnd--><div style='font-size:120%;'><div class=spoiler><div class=title style='padding-left:0'>Спойлер</div><p dir=\"ltr\">Проверочный код: 157D15RL227</p>\n<p>На локации вас встретят агенты - выполняйте указания:</p>\n<p><img src=\"../../uploaded/kaliningrad/Lite/157DRL/BY_eUJIJoOk.jpg\" alt=\"\" width=\"450\" height=\"400\" /></p></div></div><!--bonusCodeCount 1--><!--mainCodeCount 14--><!--difficultyCods null,2,2,1+,1+,1+,1+,1+,1+,1+,1+,1+,1+,1+,1+--><div class='dcodes'><strong>Коды сложности</strong><br>основные коды: <span style='color:red'>null</span>, 2, 2, 1+, 1+, 1+, 1+, 1+, <span style='color:red'>1+</span>, <span style='color:red'>1+</span>, 1+, <span style='color:red'>1+</span>, <span style='color:red'>1+</span>, <span style='color:red'>1+</span><br>бонусные коды: 1+<br> (Всего - 14 , для прохождения достаточно любых 10 , принято - 7) <br/><strong>Бонусные коды</strong>: код сложности 1+ (1  мин.)<br /></p></div><p>Найденные коды: основные коды: 157D15RL227, 157DR131L, 157D1R47L4, 157DRL123456789123456789, 157DR1842L51, 157D13R5L7, 157DRL18265</p><p><strong>Подсказка l:</strong><br/><!--LevelClue1Text--><p><img src=\"../../uploaded/kaliningrad/Lite/157DRL/vghnmhjmnhyt.png\" alt=\"\" width=\"460\" height=\"260\" /></p><!--LevelClue1TextEnd--></p><p><strong>Подсказка 2:</strong><br/><!--LevelClue2Text--><p>Пароль к спойлеру: КЛЮЧ</p><!--LevelClue2TextEnd--></p><p>Введите код</p>\t<form  data-ajax=\"false\" method=post>\n   \t\t<input type=hidden name=\"nomessage\" value=\"\">\n\t\t<input type=hidden name=action value=entcod>\n\t\t<input type=hidden name=pin value=485315>\n\t\t<input type=hidden name=lev value=0>\n\t\t<input type=text id=\"levelCodeInput\" placeholder=\"код к заданию\" name=cod size=25 value=\"\"><br>\n\t\t\t\t<input id=\"sendCode\" type=submit value=\"отправить код\">\n\t</form>\n\t\n<!--contentEnd-->Время на уровне: <span id=clock2></span><br>\n\tВремя до окончания задания: <span id=clock>&nbsp;</span> \n\t    <script>\n    window.setTimeout('countDown(4406,9995)',1000); \n    </script>\n\t<!--timeOnLevelBegin 9995 timeOnLevelEnd-->\n\t<!--timeToFinishBegin 4406 timeToFinishEnd-->\n\t<!--LastLog--><div class=title>Последние три события игры</div><table cellpadding=3 cellspacing=0 border=1><tr><th>время</th><th>событие</th><th>данные</th></tr><tr bgcolor=><td nowrap>23:31:32</td><td>передан неверный код</td><td>159DRL&nbsp;</td></tr><tr bgcolor=><td nowrap>23:31:03</td><td>принят код</td><td>157DRL18265&nbsp;</td></tr><tr bgcolor=><td nowrap>23:30:53</td><td>принят код</td><td>157D13R5L7&nbsp;</td></tr></table><!--LastLogEnd--><br>\n\t\t<form method=get data-ajax=\"false\">\n   \t\t<input type=hidden name=\"pin\" value=\"485315\">\n   \t\t<input type=hidden  name=\"q\" value=\"44272283OKOFGTAI\"/>\n   \t\t<input type=hidden  name=\"zone\" value=\"\"/>\n   \t\t<input type=hidden  name=\"nomessage\" value=\"\"/>\n\t\t<input type=submit value=\"обновить страницу\">\n\t\t</form>\n\t\t<form method=get name=refr data-ajax=\"false\">\n   \t\t<input type=hidden name=\"pin\" value=\"485315\">\n   \t\t<input type=hidden  name=\"q\" value=\"44272283OKOFGTAI\"/>\n   \t\t<input type=hidden  name=\"nomessage\" value=\"\"/>\n\t\t<input type=submit value=\"обновлять автоматически через\"> <input type=text name=refresh size=3 value=\"0\"> мин.\n\t\t</form>\n<p>Таймер браузера работает независимо от сервера! Используйте авто-обновление страницы для его синхронизации.\n<!--gameID248--><style>\n.teamstat td, .teamstat th  {\n\tfont-family : Arial;\n\tfont-size : 12px;\n}\n\n.teamstat table, .teamstat table td, .teamstat table th {\n  border: 1px solid gray;\n  border-collapse: collapse;\n}\n\n.teamstat table td, .teamstat table th {\n\tpadding : 2px 2px 2px 2px;\n}\n</style>\n<hr/><p><b>Общение с организатором</b><br/><form method=post>\n\t\t\t<input type=hidden name=action value=message>\n\t\t\t<input type=hidden name=gameData[id] value=248>\n\t\t\t<input type=hidden name=pin value=485315>\n\t\t\t<textarea name=content rows=5 cols=30></textarea><br>\n\t\t\t<input type=submit value='отправить сообщение'>\n\t\t</form><p>Команда: <strong>Pepperhouse</strong><!--<br>Игрок: <strong></strong>--><br>Игра: <strong>Тестовая игра. Начало в 2017-03-03 20:45:00. </strong><br>Организатор: <strong>Надежда Елисеева</strong>. ICQ: <strong>291-377-249</strong>. tel: <strong>+7(911)4820353</strong></p>\n\n\n\n</body>\n</html>";
	}
}

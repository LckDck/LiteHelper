using System;
using System.Collections.Generic;
using System.Linq;
using LiteHelper.History;
using Microsoft.Practices.ServiceLocation;
using Newtonsoft.Json;

namespace LiteHelper.Managers
{
	public class CodeStorageManager
	{
		IInternalStorage _storage;
		List<CodeInfo> _codes = new List<CodeInfo>();
		DateTime Beginning = new DateTime (1970, 1, 1);
		public int  CodesLimit = 10;

		public List<CodeInfo> Codes { 
			get {
				return _codes;
			}
		}

		public CodeStorageManager ()
		{
			_storage = ServiceLocator.Current.GetInstance<IInternalStorage> ();
		}

		public void Init () {
			var json = _storage.RetrieveString (Constants.Codes);
			var list = JsonConvert.DeserializeObject<List<CodeInfo>> (json);
			if (list != null) {
				_codes = list;
			}
		}

		public void AddCode (string key, string status) {
			var existed = _codes.Find (item => item.Code == key);
			if (existed != null) {
				existed.LastEditTime = GetTime ();
				existed.Status = status;
				if (StatusChanged != null) {
					StatusChanged.Invoke (null, new CodeInfoEventArgs { Code = existed.Code, Status = status});
				}
			} else {
				_codes.Add (new CodeInfo { Code = key, Status = status, LastEditTime = GetTime () });
			}
			var sorted = _codes.OrderByDescending (item => item.LastEditTime).ToList ();
			_codes = sorted;
			CheckTooMuchCodes ();
		}

		bool _isConnected;
		public bool IsConnected { 
			get {
				return _isConnected;
			} 

			internal set {
				var oldValue = _isConnected;
				_isConnected = value;
				if (oldValue != _isConnected) {
					if (ConnectedChanged != null) {
						ConnectedChanged.Invoke (null, new StatusEventArgs { Positive = _isConnected});
					}
				}
			} 
		}

		public event EventHandler<StatusEventArgs> ConnectedChanged;
		public event EventHandler<CodeInfoEventArgs> StatusChanged;

		internal void SetTimeOut (string code)
		{
			var info = _codes.Find (item => item.Code == code);
			if (info != null){
				info.TimeOut = true;
				info.Status = Constants.CodeStatusTimeOut;
			}
		}

		void CheckTooMuchCodes ()
		{
			if (_codes.Count > CodesLimit) {
				var sorted = _codes.OrderByDescending (item => item.LastEditTime).ToList ();
				sorted.RemoveRange (CodesLimit, _codes.Count - CodesLimit);
				foreach (var code in _codes) {
					if (sorted.Find (item => item.Code == code.Code) == null) {
						code.Deleted = true;
					}
				}

				_codes.RemoveAll (item => item.Deleted);
			}
		}

		double GetTime ()
		{
			return (long)(DateTime.Now - Beginning).TotalMilliseconds;
		}

		public void Clear () {
			_codes.Clear ();
			_storage.Delete (Constants.Codes);
		}

		public void SaveAll () {
			var serialized = JsonConvert.SerializeObject (_codes);
			_storage.Store (Constants.Codes, serialized);
		}
	}

	public class StatusEventArgs : EventArgs { 
		public bool Positive { get; set;}
	}

	public class CodeInfoEventArgs : EventArgs { 
		public string Code { get; set;}
		public string Status { get; set;}
	}
}

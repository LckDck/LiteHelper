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
				return;
			}

			_codes.Add (new CodeInfo { Code = key, Status = status, LastEditTime = GetTime () });
			var sorted = _codes.OrderByDescending (item => item.LastEditTime).ToList();
			_codes = sorted;
			CheckTooMuchCodes ();
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
}

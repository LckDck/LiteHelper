using System;
namespace LiteHelper
{
	public interface IInternalStorage
	{
		void Store (string key, object dataString);
		string RetrieveString (string key);
		bool RetrieveBool (string key);
		void Delete (string key);
	}
}

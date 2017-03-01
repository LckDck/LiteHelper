using System;
namespace LiteHelper.Interfaces
{
	public interface IResourceManager
	{
		string BaseUrl { get; }
		string OSVersion { get; }
		string DeviceName { get; }
	}
}

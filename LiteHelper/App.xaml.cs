﻿using LiteHelper.Managers;
using Microsoft.Practices.ServiceLocation;
using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace LiteHelper
{
	public partial class App : Application
	{
		CodeStorageManager _codeStorageManager;

		//Россия, Украина, Молдавия, Казахстан, Литва, Эстония

		public App ()
		{
			InitializeComponent ();

			MainPage = new MainMasterDetailPage ();
			_codeStorageManager = ServiceLocator.Current.GetInstance<CodeStorageManager> ();
			_codeStorageManager.Init ();
		}

		protected override void OnStart ()
		{
			CrossConnectivity.Current.ConnectivityChanged += OnConnectChanged;
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			CrossConnectivity.Current.ConnectivityChanged -= OnConnectChanged;
			var page = MainPage as MainMasterDetailPage;
			if (page != null) {
				page.SaveEverything ();
			}
			_codeStorageManager.SaveAll ();
		}


		protected override void OnResume ()
		{
			CrossConnectivity.Current.ConnectivityChanged += OnConnectChanged;
			// Handle when your app resumes
		}

		private void OnConnectChanged (object sender, ConnectivityChangedEventArgs e)
		{
			_codeStorageManager.IsConnected = e.IsConnected;
		}

	}
}

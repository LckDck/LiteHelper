using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using LiteHelper.Droid.InterfaceImplementation;
using Autofac;
using Microsoft.Practices.ServiceLocation;
using Autofac.Extras.CommonServiceLocator;
using LiteHelper.History;
using LiteHelper.Interfaces;
using LiteHelper.Managers;
using CrashlyticsKit;
using FabricSdk;
using Android.Gms.Ads;

namespace LiteHelper.Droid
{
	[Activity (Label = "LiteHelper", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		public static MainActivity Current;
		public Vibrator Vibrator;
		InAppPurchase _iInAppPurchase;
		protected override void OnCreate (Bundle bundle)
		{

			InitCrashlytics ();
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;
			Current = this;

			base.OnCreate (bundle);

			global::Xamarin.Forms.Forms.Init (this, bundle);
			var builder = new ContainerBuilder ();

			builder.RegisterInstance (new InternalStorage ())
			       .As<IInternalStorage> ();

			builder.RegisterType<CodeStorageManager> ().InstancePerLifetimeScope ();
			builder.RegisterType<CodeManager> ().InstancePerLifetimeScope ();
			builder.RegisterType<RefreshManager> ().InstancePerLifetimeScope ();

			builder.RegisterInstance (new ResourceManager ())
				   .As<IResourceManager> ();

			builder.RegisterInstance (new InAppPurchase ())
				   .As<IInAppPurchase> ();

			builder.RegisterInstance (new TimerInstance ())
			       .As<ITimerInstance> ();
			var container = builder.Build ();

			Vibrator = (Vibrator)GetSystemService (Context.VibratorService);

			ServiceLocator.SetLocatorProvider (() => new AutofacServiceLocator (container));

			MobileAds.Initialize (ApplicationContext, "ca-app-pub-9132934287753769~2766954331");

			_iInAppPurchase = ServiceLocator.Current.GetInstance<IInAppPurchase> () as InAppPurchase;
			LoadApplication (new App ());
		}

		void InitCrashlytics ()
		{
			

			Crashlytics.Instance.Initialize ();
#if DEBUG
			Fabric.Instance.Debug = true;
#endif
			Fabric.Instance.Initialize (this);

		}

		protected override void OnActivityResult (int requestCode, Result resultCode, Intent data)
		{
			base.OnActivityResult (requestCode, resultCode, data);
			if (_iInAppPurchase != null) {
				_iInAppPurchase.OnActivityResult (requestCode, resultCode, data);
			}
		}
	}
}

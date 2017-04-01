

using Autofac;
using Autofac.Extras.CommonServiceLocator;
using Foundation;
using LiteHelper.Interfaces;
using LiteHelper.iOS.InterfaceImplementation;
using LiteHelper.Managers;
using Microsoft.Practices.ServiceLocation;
using UIKit;

namespace LiteHelper.iOS
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.Init ();
			var builder = new ContainerBuilder ();

			builder.RegisterType<CodeStorageManager> ().InstancePerLifetimeScope ();

			builder.RegisterInstance (new InternalStorage ())
				   .As<IInternalStorage> ();

			builder.RegisterType<CodeManager> ().InstancePerLifetimeScope ();
			builder.RegisterType<RefreshManager> ().InstancePerLifetimeScope ();

			builder.RegisterInstance (new ResourceManager ())
				   .As<IResourceManager> ();


			builder.RegisterInstance (new TimerInstance ())
				   .As<ITimerInstance> ();
			var container = builder.Build ();

			ServiceLocator.SetLocatorProvider (() => new AutofacServiceLocator (container));
			LoadApplication (new App ());

			var result = base.FinishedLaunching (app, options);
			UIApplication.SharedApplication.KeyWindow.TintColor =  UIColor.Gray;
			return result;
		}
	}
}

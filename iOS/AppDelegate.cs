using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Autofac.Extras.CommonServiceLocator;
using Foundation;
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
			var container = builder.Build ();

			ServiceLocator.SetLocatorProvider (() => new AutofacServiceLocator (container));
			LoadApplication (new App ());

			return base.FinishedLaunching (app, options);
		}
	}
}

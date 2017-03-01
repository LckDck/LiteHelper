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

namespace LiteHelper.Droid
{
	[Activity (Label = "LiteHelper", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;

			base.OnCreate (bundle);

			global::Xamarin.Forms.Forms.Init (this, bundle);
			var builder = new ContainerBuilder ();

			builder.RegisterInstance (new InternalStorage ())
			       .As<IInternalStorage> ();

			builder.RegisterInstance (new ResourceManager ())
				   .As<IResourceManager> ();
			var container = builder.Build ();

			ServiceLocator.SetLocatorProvider (() => new AutofacServiceLocator (container));

			LoadApplication (new App ());
		}
	}
}

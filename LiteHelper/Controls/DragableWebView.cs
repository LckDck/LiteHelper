using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace LiteHelper.Controls
{
	public class DragableWebView : WebView
	{
		public static readonly BindableProperty DragCommandProperty = BindableProperty.Create ("DragCommand", typeof (ICommand), typeof (ExtendedWebView), null);


		public ICommand DragCommand {
			get {
				return (ICommand)GetValue (DragCommandProperty);
			}
			set {
				SetValue (DragCommandProperty, value);
			}
		}
	}
}

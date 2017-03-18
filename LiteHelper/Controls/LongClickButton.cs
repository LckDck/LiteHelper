using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace LiteHelper.Controls
{
	public class LongClickButton : Button
	{
		
		public static readonly BindableProperty LongClickCommandProperty = BindableProperty.Create ("LongClickCommand", typeof (ICommand), typeof (LongClickButton), null);


		public ICommand LongClickCommand {
			get {
				return (ICommand)GetValue (LongClickCommandProperty);
			}
			set {
				SetValue (LongClickCommandProperty, value);
			}
		}
	}
}

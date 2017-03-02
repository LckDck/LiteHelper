using System;
using System.Globalization;
using Xamarin.Forms;

namespace LiteHelper.Converters
{
	public class DefaultConverter<T> : IValueConverter
	{
		public object Convert (object value, Type targetType, object parameter, CultureInfo culture)
		{
			return Convert ((T)value, targetType, parameter, culture);
		}

		protected virtual object Convert (T value, Type targetType, object parameter, CultureInfo culture)
		{
			return null;
		}

		public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
		{
			return ConvertBack ((T)value, targetType, parameter, culture);
		}

		protected virtual object ConvertBack (T value, Type targetType, object parameter, CultureInfo culture)
		{
			return null;
		}
	}
}


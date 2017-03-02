using System;
using System.Globalization;

namespace LiteHelper.Converters
{
	public class InvertedBooleanConverter : DefaultConverter<bool>
	{
		protected override object Convert (bool value, Type targetType, object parameter, CultureInfo culture)
		{
			return !value;
		}

		protected override object ConvertBack (bool value, Type targetType, object parameter, CultureInfo culture)
		{
			return !value;
		}
	}
}


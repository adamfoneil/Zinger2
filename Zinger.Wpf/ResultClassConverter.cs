using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace Zinger.Wpf;

public class ResultClassConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
		if (values[0] is Dictionary<string, string> dictionary && values[1] is string key && dictionary.TryGetValue(key, out string? value))
		{
			return value;
		}

		return null!;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace Zinger.Wpf;

public class ResultClassConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        var dictionary = values[0] as Dictionary<string, string>;
        var key = values[1] as string;

        if (dictionary != null && key != null && dictionary.TryGetValue(key, out string? value))
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

using System;
using System.Globalization;
using System.Windows.Data;

namespace CarSparePartStore.Converters;

public class LongToShortTextConverter 
    : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var stringValue = value as string;
        if (stringValue is null || stringValue.Length < 50)
        {
            return value;
        }
        return stringValue.Substring(0, 50);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
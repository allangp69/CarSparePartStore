using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace CarSparePartStore;

public class BoolToRedGreenImageConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is true)
        {
            return Application.Current.TryFindResource("ConnectionGreenImage") as BitmapImage;
        }
        return Application.Current.TryFindResource("ConnectionRedImage") as BitmapImage;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
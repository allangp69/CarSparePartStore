using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using CarSparePartStore.ViewModels.DTO;

namespace CarSparePartStore.Converters;

public class OrderItemsToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var retval = new StringBuilder();
        var orderItems = value as IEnumerable<OrderItemDTO>;
        if (orderItems is null)
        {
            return retval.ToString();
        }
        foreach (var orderItem in orderItems)
        {
            retval.AppendLine($"{orderItem.NumberOfItems} {orderItem.Product.Name}");
        }
        return retval.ToString();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
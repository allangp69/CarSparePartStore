using System.Text;

namespace CarSparePartService.ExtensionMethods;

public static class OrderExtensionMethods
{
    public static string ProductsList(this Order order)
    {
        var retval = new StringBuilder();
        if (order is null || order.OrderItems is null || !order.OrderItems.Any())
        {
            return retval.ToString();
        }
        foreach (var item in order.OrderItems)
        {
            retval.AppendLine($"{item.NumberOfItems} items of {item.Product.Name} at {item.Product.Price} per item");
        }
        return retval.ToString();
    }
}
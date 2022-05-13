namespace CarSparePartService.ExtensionMethods;

public static class OrderExtensionMethods
{
    public static string ProductsList(this Order order)
    {
        var retval = string.Empty;
        if (order is null || order.OrderItems is null || !order.OrderItems.Any())
        {
            return retval;
        }
        foreach (var item in order.OrderItems)
        {
            retval += $"{item.NumberOfItems} items of {item.Product.Name} at {item.Product.Price} per item {Environment.NewLine}";
        }
        return retval;
    }
}
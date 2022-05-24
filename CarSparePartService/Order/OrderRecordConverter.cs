using CarSparePartData.Order;
using CarSparePartData.Product;

namespace CarSparePartService.Order;

public class OrderRecordConverter
{
    public IEnumerable<OrderRecord> ConvertToRecord(IEnumerable<global::CarSparePartService.Order.Order> orders)
    {
        var retval = new List<OrderRecord>();
        if (orders is null || !orders.Any())
        {
            return retval;
        }

        retval.AddRange(orders.Select(order => ConvertToRecord(order)));

        return retval;
    }

    public IEnumerable<Order> ConvertFromRecord(IEnumerable<OrderRecord> orderRecords)
    {
        var retval = new List<global::CarSparePartService.Order.Order>();
        if (orderRecords is null || !orderRecords.Any())
        {
            return retval;
        }

        retval.AddRange(orderRecords.Select(orderRecord => ConvertFromRecord(orderRecord)));

        return retval;
    }
    
    #region ConvertToRecord
    private OrderRecord ConvertToRecord(Order order)
    {
        if (order is null)
        {
            return null;
        }
        var retval = new OrderRecord
        {
            OrderId = order.OrderId,
            OrderDateTime = order.OrderDateTime,
            CustomerId = order.CustomerId,
            OrderItems = ConvertToRecord(order.OrderItems).ToList()
        };
        return retval;
    }
    
    private IEnumerable<OrderItemRecord> ConvertToRecord(IEnumerable<OrderItem> orderItems)
    {
        var retval = new List<OrderItemRecord>();
        if (orderItems is null || !orderItems.Any())
        {
            return retval;
        }

        retval.AddRange(orderItems.Select(orderItem => ConvertToRecord(orderItem)));

        return retval;
    }
    
    private OrderItemRecord ConvertToRecord(OrderItem orderItem)
    {
        if (orderItem is null)
        {
            return null;
        }
        var retval = new OrderItemRecord
        {
            NumberOfItems = orderItem.NumberOfItems,
            Product = ConvertToRecord(orderItem.Product)
        };
        return retval;
    }
    
    private ProductRecord ConvertToRecord(Product.Product product)
    {
        if (product is null)
        {
            return null;
        }
        var retval = new ProductRecord
        {
            ProductId = product.ProductId,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Type = product.Type
        };

        return retval;
    }
    #endregion ConvertToRecord
    
    #region ConvertFromRecord
    private Order ConvertFromRecord(OrderRecord order)
    {
        if (order is null)
        {
            return null;
        }
        return new Order(order.OrderId, order.OrderDateTime, order.CustomerId, ConvertFromRecord(order.OrderItems).ToList());
    }
    
    private IEnumerable<OrderItem> ConvertFromRecord(IEnumerable<OrderItemRecord> orderItems)
    {
        var retval = new List<OrderItem>();
        if (orderItems is null || !orderItems.Any())
        {
            return retval;
        }

        retval.AddRange(orderItems.Select(orderItem => ConvertFromRecord(orderItem)));

        return retval;
    }
    
    private OrderItem ConvertFromRecord(OrderItemRecord orderItem)
    {
        if (orderItem is null)
        {
            return null;
        }
        var retval = new OrderItem
        {
            NumberOfItems = orderItem.NumberOfItems,
            Product = ConvertFromRecord(orderItem.Product)
        };
        return retval;
    }
    
    private Product.Product ConvertFromRecord(ProductRecord product)
    {
        if (product is null)
        {
            return null;
        }
        var retval = new Product.Product
        {
            ProductId = product.ProductId,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Type = product.Type
        };

        return retval;
    }
    #endregion ConvertFromRecord
}
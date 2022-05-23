using System.Collections.Generic;
using System.Linq;
using CarSparePartService;
using CarSparePartService.Order;
using CarSparePartService.Product;

namespace CarSparePartStore.ViewModels.DTO;

public class OrderDTOConverter
{
    public IEnumerable<OrderDTO> ConvertToDTO(IEnumerable<Order> orders)
    {
        var retval = new List<OrderDTO>();
        if (orders is null || !orders.Any())
        {
            return retval;
        }

        retval.AddRange(orders.Select(order => ConvertToDTO(order)));

        return retval;
    }

    public IEnumerable<Order> ConvertFromDTO(IEnumerable<OrderDTO> dtoOrders)
    {
        var retval = new List<Order>();
        if (dtoOrders is null || !dtoOrders.Any())
        {
            return retval;
        }

        retval.AddRange(dtoOrders.Select(dtoOrder => ConvertFromDTO(dtoOrder)));

        return retval;
    }
    
    #region ConvertToDTO
    private OrderDTO ConvertToDTO(Order order)
    {
        if (order is null)
        {
            return null;
        }
        var retval = new OrderDTO
        {
            OrderId = order.OrderId,
            OrderDateTime = order.OrderDateTime,
            CustomerId = order.CustomerId,
            OrderItems = ConvertToDTO(order.OrderItems).ToList()
        };
        return retval;
    }
    
    private IEnumerable<OrderItemDTO> ConvertToDTO(IEnumerable<OrderItem> orderItems)
    {
        var retval = new List<OrderItemDTO>();
        if (orderItems is null || !orderItems.Any())
        {
            return retval;
        }

        retval.AddRange(orderItems.Select(orderItem => ConvertToDTO(orderItem)));

        return retval;
    }
    
    private OrderItemDTO ConvertToDTO(OrderItem orderItem)
    {
        if (orderItem is null)
        {
            return null;
        }
        var retval = new OrderItemDTO
        {
            NumberOfItems = orderItem.NumberOfItems,
            Product = ConvertToDTO(orderItem.Product)
        };
        return retval;
    }
    
    internal IEnumerable<ProductDTO> ConvertToDTO(IEnumerable<Product> products)
    {
        var retval = new List<ProductDTO>();
        if (products is null || !products.Any())
        {
            return retval;
        }

        retval.AddRange(products.Select(product => ConvertToDTO(product)));

        return retval;
    }
    
    internal ProductDTO ConvertToDTO(Product product)
    {
        if (product is null)
        {
            return null;
        }
        var retval = new ProductDTO
        {
            ProductId = product.ProductId,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Type = product.Type
        };

        return retval;
    }
    #endregion ConvertToDTO
    
    #region ConvertFromDTO
    internal Order ConvertFromDTO(OrderDTO order)
    {
        if (order is null)
        {
            return null;
        }
        return new Order(order.OrderId, order.OrderDateTime, order.CustomerId, ConvertFromDTO(order.OrderItems).ToList());
    }
    
    private IEnumerable<OrderItem> ConvertFromDTO(IEnumerable<OrderItemDTO> orderItems)
    {
        var retval = new List<OrderItem>();
        if (orderItems is null || !orderItems.Any())
        {
            return retval;
        }

        retval.AddRange(orderItems.Select(orderItem => ConvertFromDTO(orderItem)));

        return retval;
    }
    
    private OrderItem ConvertFromDTO(OrderItemDTO orderItem)
    {
        if (orderItem is null)
        {
            return null;
        }
        var retval = new OrderItem
        {
            NumberOfItems = orderItem.NumberOfItems,
            Product = ConvertFromDTO(orderItem.Product)
        };
        return retval;
    }
    
    public Product ConvertFromDTO(ProductDTO product)
    {
        if (product is null)
        {
            return null;
        }
        var retval = new Product
        {
            ProductId = product.ProductId,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Type = product.Type
        };

        return retval;
    }
    #endregion ConvertFromDTO
}
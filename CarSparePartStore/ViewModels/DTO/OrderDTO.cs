using System;
using System.Collections.Generic;
using System.Linq;

namespace CarSparePartStore.ViewModels.DTO;

public class OrderDTO
{
    public OrderDTO()
    {
        OrderItems = new List<OrderItemDTO>();
    }
    public Guid OrderId { get; set; }
    public DateTime OrderDateTime { get; set; }
    public int CustomerId { get; set; }
    public List<OrderItemDTO> OrderItems { get; set; }
    
    public decimal TotalPrice 
    {
        get
        {
            return OrderItems.Sum(o => o.Product.Price * o.NumberOfItems);
        }
    }
}
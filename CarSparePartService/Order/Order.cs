namespace CarSparePartService.Order;

public class Order
{
    public Order(int customerId, List<OrderItem> orderItems)
    {
        CustomerId = customerId;
        OrderItems = orderItems;
        OrderDateTime = DateTime.Now;
        OrderId = Guid.NewGuid();
    }
    
    public Order(Guid orderId, DateTime orderDateTime, int customerId, List<OrderItem> orderItems)
        : this(customerId, orderItems)
    {
        OrderId = orderId;
        OrderDateTime = orderDateTime;
    }

    public Guid OrderId { get; }
    public DateTime OrderDateTime { get; set; }
    public int CustomerId { get; set; }
    public List<OrderItem> OrderItems { get; set; }
    
}
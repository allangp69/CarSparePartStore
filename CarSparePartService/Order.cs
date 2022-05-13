namespace CarSparePartService;

public class Order
{
    private Order()
    {
        
    }
    public Guid OrderId { get; private set; }
    public DateTime OrderDateTime { get; set; }
    public int CustomerId { get; set; }
    public List<OrderItem> OrderItems { get; set; }
    
    public void AddItem(OrderItem item)
    {
        OrderItems.Add(item);
    }

    public static Order Create(int customerId, IEnumerable<OrderItem> orderItems)
    {
        return new Order { OrderId = Guid.NewGuid(), OrderDateTime = DateTime.Now, CustomerId = customerId, OrderItems = orderItems.ToList()};
    }
    
    public static Order Create(Guid orderId, DateTime orderDateTime, int customerId, IEnumerable<OrderItem> orderItems)
    {
        return new Order { OrderId = orderId, OrderDateTime = orderDateTime, CustomerId = customerId, OrderItems = orderItems.ToList()};
    }
}
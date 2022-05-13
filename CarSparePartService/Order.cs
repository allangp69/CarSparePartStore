namespace CarSparePartService;

public class Order
{
    private Order()
    {
        
    }
    public Guid OrderId { get; private set; }
    public int CustomerId { get; set; }
    public List<OrderItem> OrderItems { get; set; }
    
    public void AddItem(OrderItem item)
    {
        OrderItems.Add(item);
    }

    public static Order Create(int customerId, IEnumerable<OrderItem> orderItems)
    {
        return new Order { OrderId = Guid.NewGuid(), CustomerId = customerId, OrderItems = orderItems.ToList()};
    }
    
    public static Order Create(Guid orderId, int customerId, IEnumerable<OrderItem> orderItems)
    {
        return new Order { OrderId = orderId, CustomerId = customerId, OrderItems = orderItems.ToList()};
    }
}
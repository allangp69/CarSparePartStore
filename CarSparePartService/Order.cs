namespace CarSparePartService;

public class Order 
{
    private Order()
    {
        
    }
    
    public int CustomerId { get; set; }
    public IEnumerable<OrderItem> OrderItems { get; set; }
    
    public void AddItem(OrderItem item)
    {
        throw new NotImplementedException();
    }

    public static Order Create(int customerId, IEnumerable<OrderItem> orderItems)
    {
        return new Order { CustomerId = customerId, OrderItems = orderItems};
    }
}
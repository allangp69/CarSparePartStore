namespace CarSparePartService;

public class Order 
{
    public IEnumerable<OrderItem> OrderItems { get; set; }
    
    public void AddItem(OrderItem item)
    {
        throw new NotImplementedException();
    }

    public static Order Create(IEnumerable<OrderItem> orderItems)
    {
        return new Order {OrderItems = orderItems};
    }
}
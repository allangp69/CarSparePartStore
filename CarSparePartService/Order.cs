namespace CarSparePartService;

public class Order
{
    public Order(int customerId, List<OrderItem> orderItems)
    {
        CustomerId = customerId;
        OrderItems = orderItems;
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
    
    public void AddItem(OrderItem item)
    {
        OrderItems.Add(item);
    }

    public decimal TotalPrice 
    {
        get
        {
            return OrderItems.Sum(o => o.Product.Price * o.NumberOfItems);
        }
    }

    // public static Order Create(int customerId, IEnumerable<OrderItem> orderItems)
    // {
    //     return new Order { OrderId = Guid.NewGuid(), OrderDateTime = DateTime.Now, CustomerId = customerId, OrderItems = orderItems.ToList()};
    // }
    //
    // public static Order Create(Guid orderId, DateTime orderDateTime, int customerId, IEnumerable<OrderItem> orderItems)
    // {
    //     return new Order { OrderId = orderId, OrderDateTime = orderDateTime, CustomerId = customerId, OrderItems = orderItems.ToList()};
    // }
}
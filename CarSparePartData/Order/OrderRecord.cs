namespace CarSparePartData.Order;

public record OrderRecord
{
    public Guid OrderId { get; set; }
    public DateTime OrderDateTime { get; set; }
    public int CustomerId { get; set; }
    public List<OrderItemRecord> OrderItems { get; set; }
}
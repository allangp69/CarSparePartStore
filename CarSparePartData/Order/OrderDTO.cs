namespace CarSparePartService.Backup;

public record OrderDTO
{
    public Guid OrderId { get; set; }
    public DateTime OrderDateTime { get; set; }
    public int CustomerId { get; set; }
    public List<OrderItemDTO> OrderItems { get; set; }
}
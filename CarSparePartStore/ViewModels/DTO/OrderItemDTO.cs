namespace CarSparePartStore.ViewModels.DTO;

public record OrderItemDTO
{
    public ProductDTO Product { get; set; }
    public int NumberOfItems { get; set; }
}
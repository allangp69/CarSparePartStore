using CarSparePartData.Product;

namespace CarSparePartService.Backup;

public record OrderItemDTO
{
    public ProductDTO Product { get; set; }
    public int NumberOfItems { get; set; }
}
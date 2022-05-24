using CarSparePartData.Product;

namespace CarSparePartData.Order;

public record OrderItemRecord
{
    public ProductRecord Product { get; set; }
    public int NumberOfItems { get; set; }
}
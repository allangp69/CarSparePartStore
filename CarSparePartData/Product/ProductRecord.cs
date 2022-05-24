namespace CarSparePartData.Product;

public record ProductRecord
{
    public long ProductId { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Type { get; set; }
    public string Description { get; set; }
}
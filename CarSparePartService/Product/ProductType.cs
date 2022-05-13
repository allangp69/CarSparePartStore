namespace CarSparePartService.Product;

public class ProductType
{
    public string Title { get; }

    public ProductType(string title)
    {
        Title = title;
        Products = new List<global::CarSparePartService.Product.Product>();
    }
    
    public List<global::CarSparePartService.Product.Product> Products { get; set; }
}
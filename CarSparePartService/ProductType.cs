namespace CarSparePartService;

public class ProductType
{
    public string Title { get; }

    public ProductType(string title)
    {
        Title = title;
        Products = new List<Product>();
    }
    
    public List<Product> Products { get; set; }
}
namespace CarSparePartService;

public class ProductListing
{
    public ProductListing(string title)
    {
        Title = title;
        ProductTypeList = new List<ProductType>();
    }

    public string Title { get; set; }
    public List<ProductType> ProductTypeList { get; set; }
}
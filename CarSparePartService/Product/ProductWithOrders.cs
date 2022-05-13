namespace CarSparePartService.Product;

public class ProductWithOrders
{
    private readonly IEnumerable<Guid> _orderIds;
    private global::CarSparePartService.Product.Product Product { get; }

    public ProductWithOrders(global::CarSparePartService.Product.Product product, IEnumerable<Guid> orderIds)
    {
        _orderIds = orderIds;
        Product = product;
    }

    public long ProductId {
        get
        {
            return Product.ProductId;
        }
    }
    
    public string ProductName {
        get
        {
            return Product.Name;
        }
    }
    public string ProductType {
        get
        {
            return Product.Type;
        }
    }
    public string Description {
        get
        {
            return Product.Description;
        }
    }
    public string ShortDescription {
        get
        {
            return $"{Product.Description.Substring(0, 50).TrimStart()} ...";
        }
    }
    public decimal Price {
        get
        {
            return Product.Price;
        }
    }
    public int NumberOfItemsSold
    {
        get { return _orderIds.Count(); }
    }
    public decimal TotalPrice {
        get
        {
            return Price * NumberOfItemsSold;
        }
    }
}
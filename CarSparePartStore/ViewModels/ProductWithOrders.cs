using CarSparePartService;

namespace CarSparePartStore.ViewModels;

internal class ProductWithOrders
{
    private Product Product { get; }

    public ProductWithOrders(Product product, int numberOfItemsSold)
    {
        Product = product;
        NumberOfItemsSold = numberOfItemsSold;
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
    public decimal Price {
        get
        {
            return Product.Price;
        }
    }
    public int NumberOfItemsSold { get; }
    public decimal TotalPrice {
        get
        {
            return Price * NumberOfItemsSold;
        }
    }
}
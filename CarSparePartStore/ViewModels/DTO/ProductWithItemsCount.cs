using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace CarSparePartStore.ViewModels.DTO;

public class ProductWithItemsCount
        : ObservableRecipient
{
    private ProductDTO Product { get; }

    private int _itemsCount;
    public int ItemsCount
    {
        get => _itemsCount;
        set => SetProperty(ref _itemsCount,  value);
    }

    public ProductWithItemsCount(ProductDTO product, int itemsCount)
    {
        ItemsCount = itemsCount;
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

    public decimal TotalPrice {
        get
        {
            return Price * ItemsCount;
        }
    }
}
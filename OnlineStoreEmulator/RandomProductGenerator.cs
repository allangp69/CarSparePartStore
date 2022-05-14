using CarSparePartService;
using CarSparePartService.Interfaces;
using CarSparePartService.Product;
using OnlineStoreEmulator;

namespace TestServicesConfigurator;

public class RandomProductGenerator
    : IRandomProductGenerator
{
    private static readonly Random _random = new Random();
    private readonly IProductFetcher _productFetcher;

    public RandomProductGenerator(IProductFetcher productFetcher)
    {
        _productFetcher = productFetcher;
    }
    public Product GenerateProduct()
    {
        var allCProducts = _productFetcher.GetAllProducts().ToList();
        var i = _random.Next(0, allCProducts.Count());
        return allCProducts[i];
    }
}
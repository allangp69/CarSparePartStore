using CarSparePartService.Interfaces;
using CarSparePartService.Product;

namespace OnlineStoreEmulator;

public class RandomProductGenerator
    : IRandomProductGenerator
{
    private static readonly Random _random = new Random();
    private readonly IProductService _productService;

    public RandomProductGenerator(IProductService productService)
    {
        _productService = productService;
    }
    public Product GenerateProduct()
    {
        var allProducts = _productService.GetAllProducts().ToList();
        var i = _random.Next(0, allProducts.Count());
        return allProducts[i];
    }
}
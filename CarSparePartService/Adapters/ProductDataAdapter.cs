using CarSparePartData.Interfaces;
using CarSparePartService.Product;

namespace CarSparePartService.Adapters;

public class ProductDataAdapter
{
    private readonly IProductRepository _productRepository;
    private readonly ProductDTOConverter _productDtoConverter;

    public ProductDataAdapter(IProductRepository productRepository, ProductDTOConverter productDtoConverter)
    {
        _productRepository = productRepository;
        _productDtoConverter = productDtoConverter;
    }
    public IEnumerable<Product.Product> GetAllProducts()
    {
        return _productDtoConverter.ConvertFromDTO(_productRepository.GetAllProducts());
    }

    public Product.Product FindProduct(long productId)
    {
        return _productDtoConverter.ConvertFromDTO(_productRepository.FindProduct(productId));
    }
}
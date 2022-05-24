using CarSparePartData.Interfaces;
using CarSparePartService.Product;

namespace CarSparePartService.Adapters;

public class ProductDataAdapter
{
    private readonly IProductRepository _productRepository;
    private readonly ProductRecordConverter _productRecordConverter;

    public ProductDataAdapter(IProductRepository productRepository, ProductRecordConverter productRecordConverter)
    {
        _productRepository = productRepository;
        _productRecordConverter = productRecordConverter;
    }
    public IEnumerable<Product.Product> GetAllProducts()
    {
        return _productRecordConverter.ConvertFromRecord(_productRepository.GetAllProducts());
    }

    public Product.Product FindProduct(long productId)
    {
        return _productRecordConverter.ConvertFromRecord(_productRepository.FindProduct(productId));
    }
}
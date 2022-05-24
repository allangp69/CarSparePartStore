using CarSparePartData.Interfaces;
using CarSparePartService.Interfaces;
using CarSparePartService.Product;

namespace CarSparePartService;

public class ProductService
    :IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly ProductRecordConverter _productRecordConverter;

    public ProductService(IProductRepository productRepository, ProductRecordConverter productRecordConverter)
    {
        _productRepository = productRepository;
        _productRecordConverter = productRecordConverter;
    }
    public IEnumerable<Product.Product> GetAllProducts()
    {
        return _productRecordConverter.ConvertFromRecord(_productRepository.GetAllProducts());
    }

    public void LoadProductsFromBackup()
    {
        
        _productRepository.LoadProductsFromBackup();
    }
}
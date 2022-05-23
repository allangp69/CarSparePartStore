using CarSparePartData.Interfaces;
using CarSparePartService.Interfaces;
using CarSparePartService.Product;

namespace CarSparePartService;

public class ProductService
    :IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly ProductDTOConverter _productDtoConverter;

    public ProductService(IProductRepository productRepository, ProductDTOConverter productDtoConverter)
    {
        _productRepository = productRepository;
        _productDtoConverter = productDtoConverter;
    }
    public IEnumerable<Product.Product> GetAllProducts()
    {
        return _productDtoConverter.ConvertFromDTO(_productRepository.GetAllProducts());
    }

    public void LoadProductsFromBackup()
    {
        
        _productRepository.LoadProductsFromBackup();
    }
}
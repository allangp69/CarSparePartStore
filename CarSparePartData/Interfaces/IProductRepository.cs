using CarSparePartData.Product;

namespace CarSparePartData.Interfaces;

public interface IProductRepository
{
    IEnumerable<ProductDTO> GetAllProducts();
    ProductDTO FindProduct(long productId);
    void LoadProductsFromBackup();
}
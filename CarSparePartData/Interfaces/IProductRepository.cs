using CarSparePartData.Product;

namespace CarSparePartData.Interfaces;

public interface IProductRepository
{
    IEnumerable<ProductRecord> GetAllProducts();
    ProductRecord FindProduct(long productId);
    void LoadProductsFromBackup();
}
namespace CarSparePartService.Interfaces;

public interface IProductFetcher
{
    IEnumerable<Product.Product> GetAllProducts();
    Product.Product FindProduct(long productId);
    void LoadProducts(string fileName);
    void LoadProductsFromBackup();
}
namespace CarSparePartService.Interfaces;

public interface IProductFetcher
{
    IEnumerable<Product.Product> GetAllProducts();
    Product.Product FindProduct(int productId);
    void LoadProducts(string fileName);
    void LoadProductsFromBackup();
}
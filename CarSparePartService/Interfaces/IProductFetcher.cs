namespace CarSparePartService.Interfaces;

public interface IProductFetcher
{
    IEnumerable<Product> GetAllProducts();
    Product FindProduct(int productId);
    void LoadProducts(string fileName);
}
namespace CarSparePartService.Interfaces;

public interface IProductService
{
    IEnumerable<Product.Product> GetAllProducts();
    void LoadProductsFromBackup();
}
namespace CarSparePartService.Interfaces;

public interface ICarSparePartService
{
    event EventHandler<OrderAddedEventArgs> OrderAdded;
    event EventHandler BackupCompleted;
    event EventHandler RestoreBackupCompleted;
    public void CreateBackup();
    public void RestoreBackup();
    public void PlaceOrder(Order.Order order);
    IEnumerable<Order.Order> GetAllOrders();
    IEnumerable<Order.Order> GetOrdersForProduct(Product.Product product);
    IEnumerable<Product.Product> GetAllProducts();
    int GetNumberOfItemsSoldForProduct(Product.Product product);
    Product.Product FindProduct(long productId);
}
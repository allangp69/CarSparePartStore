using System.Collections.ObjectModel;
using CarSparePartService.Product;

namespace CarSparePartService.Interfaces;

public interface ICarSparePartService
{
    event EventHandler<OrderAddedEventArgs> OrderAdded;
    event EventHandler BackupCompleted;
    event EventHandler RestoreBackupCompleted;
    public void CreateBackup(string filePath);
    public void LoadBackup(string filePath);
    public void PlaceOrder(Order order);
    IEnumerable<Order> GetAllOrders();
    IEnumerable<ProductWithOrders> GetProductsWithOrders();
}
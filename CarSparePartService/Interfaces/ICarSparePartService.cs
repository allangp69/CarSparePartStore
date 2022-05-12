using System.Collections.ObjectModel;

namespace CarSparePartService.Interfaces;

public interface ICarSparePartService
{
    public void CreateBackup(string filePath);
    public void LoadBackup(string filePath);
    public void PlaceOrder(Order order);
    IEnumerable<Order> GetAllOrders();
}
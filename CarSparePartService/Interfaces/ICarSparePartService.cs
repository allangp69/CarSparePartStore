namespace CarSparePartService.Interfaces;

public interface ICarSparePartService
{
    public void CreateBackup(string filePath);
    public void LoadBackup(string filePath);
    public void PlaceOrder(Customer customer, Order order);
    IEnumerable<Order> GetAllOrders();
}
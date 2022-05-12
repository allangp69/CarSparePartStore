using CarSparePartService.Interfaces;

namespace CarSparePartService;

public class CarSparePartService
        : ICarSparePartService
{
    private readonly IOrderBackupManager _orderBackupManager;
    
    public CarSparePartService(IOrderBackupManager orderBackupManager)
    {
        _orderBackupManager = orderBackupManager;
        Orders = new List<Order>();
    }
    
    public void CreateBackup(string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            return;
        }
        _orderBackupManager.BackupToFile(GetAllOrders(), filePath);
    }

    public void LoadBackup(string filePath)
    {
        Orders = _orderBackupManager.LoadBackupFromFile(filePath).ToList();
    }

    public void PlaceOrder(Order order)
    {
        Orders.Add(order);
    }

    private List<Order> Orders { get; set; }

    public IEnumerable<Order> GetAllOrders()
    {
        return Orders;
    }
}
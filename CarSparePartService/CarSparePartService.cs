using CarSparePartService.Interfaces;

namespace CarSparePartService;

public class CarSparePartService
        : ICarSparePartService
{
    public CarSparePartService()
    {
        CustomersWithOrders = new Dictionary<Customer, List<Order>>();
    }
    
    public void CreateBackup(string filePath)
    {
        throw new NotImplementedException();
    }

    public void LoadBackup(string filePath)
    {
        throw new NotImplementedException();
    }

    public void PlaceOrder(Customer customer, Order order)
    {
        if (!CustomersWithOrders.ContainsKey(customer))
        {
            CustomersWithOrders.Add(customer, new List<Order>());
        }
        CustomersWithOrders[customer].Add(order);
    }

    private Dictionary<Customer, List<Order>> CustomersWithOrders { get; }

    public IEnumerable<Order> GetAllOrders()
    {
        return CustomersWithOrders.SelectMany(c => c.Value);
    }
}
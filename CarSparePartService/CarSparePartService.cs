using CarSparePartService.ExtensionMethods;
using CarSparePartService.Interfaces;
using Serilog;

namespace CarSparePartService;

public class CarSparePartService
        : ICarSparePartService
{
    private readonly IOrderBackupManager _orderBackupManager;
    private readonly IProductFetcher _productFetcher;
    private readonly ILogger _logger;
    public event EventHandler<OrderAddedEventArgs> OrderAdded;
    
    public CarSparePartService(IOrderBackupManager orderBackupManager, IProductFetcher productFetcher, ILogger logger)
    {
        _orderBackupManager = orderBackupManager;
        _productFetcher = productFetcher;
        _logger = logger;
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

    protected virtual void OnOrderAdded(OrderAddedEventArgs e)
    {
        var handler = OrderAdded;
        handler?.Invoke(this, e);
    }
    
    public void PlaceOrder(Order order)
    {
        Orders.Add(order);
        _logger.Information($"Order added - customerId: {order.CustomerId} - products: {order.ProductsList()}");
        OnOrderAdded(new OrderAddedEventArgs
        {
            CustomerId = order.CustomerId,
            Products = order.ProductsList() 
        });
    }

    private List<Order> Orders { get; set; }

    public IEnumerable<Order> GetAllOrders()
    {
        return Orders;
    }
    
    public IEnumerable<ProductWithOrders> GetProductsWithOrders()
    {
        var retval = new List<ProductWithOrders>();
        var products = _productFetcher.GetAllProducts(); 
        foreach (var product in products)
        {
            retval.Add(new ProductWithOrders(product, GetOrdersIdsForProduct(product)));
        }
        return retval;
    }

    private IEnumerable<Guid> GetOrdersIdsForProduct(Product product)
    {
        if (!Orders.Any())
            return new List<Guid>();
        var comparer = new UniqueProductComparer();
        return Orders.Where(o => o.OrderItems.Any(i => comparer.Equals(i.Product, product))).Select(o => o.OrderId).ToList();
    }
}
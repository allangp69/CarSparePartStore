using CarSparePartService.Backup;
using CarSparePartService.EqualityComparers;
using CarSparePartService.ExtensionMethods;
using CarSparePartService.Interfaces;
using CarSparePartService.Product;
using Serilog;

namespace CarSparePartService;

public class CarSparePartService
        : ICarSparePartService
{
    private readonly IOrderBackupManager _orderBackupManager;
    private readonly IProductFetcher _productFetcher;
    private readonly ILogger _logger;
    public event EventHandler<OrderAddedEventArgs> OrderAdded;
    public event EventHandler BackupCompleted;
    public event EventHandler RestoreBackupCompleted;
    
    public CarSparePartService(IOrderBackupManager orderBackupManager, IProductFetcher productFetcher, ILogger logger)
    {
        _orderBackupManager = orderBackupManager;
        _productFetcher = productFetcher;
        _logger = logger;
        Orders = new List<Order>();
    }
    
    
    protected virtual void OnBackupCompleted(EventArgs e)
    {
        var handler = BackupCompleted;
        handler?.Invoke(this, e);
    }
    public void CreateBackup(string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            return;
        }
        var converter = new OrderDTOConverter();
        _orderBackupManager.BackupToFile(converter.ConvertToDTO(GetAllOrders()), filePath);
        OnBackupCompleted(EventArgs.Empty);
    }

    protected virtual void OnRestoreBackupCompleted(EventArgs e)
    {
        var handler = RestoreBackupCompleted;
        handler?.Invoke(this, e);
    }
    public void LoadBackup(string filePath)
    {
        if (!File.Exists(filePath))
        {
            _logger.Error($"Could not restore from backup - file: {filePath} doesn't exist");
        }
        var converter = new OrderDTOConverter();
        Orders = converter.ConvertFromDTO(_orderBackupManager.LoadBackupFromFile(filePath)).ToList();
        OnRestoreBackupCompleted(EventArgs.Empty);
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

    private IEnumerable<Guid> GetOrdersIdsForProduct(Product.Product product)
    {
        if (!Orders.Any())
            return new List<Guid>();
        var comparer = new UniqueProductComparer();
        return Orders.Where(o => o.OrderItems.Any(i => comparer.Equals(i.Product, product))).Select(o => o.OrderId).ToList();
    }
}
using CarSparePartData.Interfaces;
using CarSparePartService.Adapters;
using CarSparePartService.Backup;
using CarSparePartService.EqualityComparers;
using CarSparePartService.ExtensionMethods;
using CarSparePartService.Interfaces;
using Serilog;

namespace CarSparePartService;

public class CarSparePartService
    : ICarSparePartService
{
    private readonly IOrderBackupManager _orderBackupManager;
    private readonly ProductDataAdapter _productDataAdapter;
    private readonly ILogger _logger;
    private static object _ordersLockObject = new object();
    public event EventHandler<OrderAddedEventArgs> OrderAdded;
    public event EventHandler BackupCompleted;
    public event EventHandler RestoreBackupCompleted;

    public CarSparePartService(IOrderBackupManager orderBackupManager, ProductDataAdapter productDataAdapter,
        ILogger logger)
    {
        _orderBackupManager = orderBackupManager;
        _productDataAdapter = productDataAdapter;
        _logger = logger;
        Orders = new List<Order.Order>();
    }

    #region Orders

    protected virtual void OnOrderAdded(OrderAddedEventArgs e)
    {
        var handler = OrderAdded;
        handler?.Invoke(this, e);
    }


    public void PlaceOrder(Order.Order order)
    {
        lock (_ordersLockObject)
        {
            Orders.Add(order);
        }

        _logger.Information($"Order added - customerId: {order.CustomerId} - products: {order.ProductsList()}");
        OnOrderAdded(new OrderAddedEventArgs
        {
            CustomerId = order.CustomerId,
            Products = order.ProductsList()
        });
    }

    private List<Order.Order> Orders { get; set; }

    public IEnumerable<Order.Order> GetAllOrders()
    {
        lock (_ordersLockObject)
        {
            return Orders;
        }
    }

    public IEnumerable<Order.Order> GetOrdersForProduct(Product.Product product)
    {
        var comparer = new UniqueProductComparer();
        lock (_ordersLockObject)
        {
            return Orders.Where(o => o.OrderItems.Any(i => comparer.Equals(i.Product, product)));
        }
    }

    public IEnumerable<Product.Product> GetAllProducts()
    {
        return _productDataAdapter.GetAllProducts();
    }

    public int GetNumberOfItemsSoldForProduct(Product.Product product)
    {
        var retval = 0;
        lock (_ordersLockObject)
        {
            if (!Orders.Any())
                return retval;
            var comparer = new UniqueProductComparer();
            foreach (var order in Orders.Where(o => o.OrderItems.Any(i => comparer.Equals(i.Product, product))))
            {
                retval += order.OrderItems.Where(o => comparer.Equals(o.Product, product)).Sum(o => o.NumberOfItems);
            }
        }

        return retval;
    }

    public Product.Product FindProduct(long productId)
    {
        return _productDataAdapter.FindProduct(productId);
    }

    #endregion Orders

    #region Backup

    private void OnBackupCompleted(EventArgs e)
    {
        var handler = BackupCompleted;
        handler?.Invoke(this, e);
    }

    public void CreateBackup()
    {
        _orderBackupManager.Backup(GetAllOrders());
        OnBackupCompleted(EventArgs.Empty);
    }

    private void OnRestoreBackupCompleted(EventArgs e)
    {
        var handler = RestoreBackupCompleted;
        handler?.Invoke(this, e);
    }

    public void RestoreBackup()
    {
        lock (_ordersLockObject)
        {
            Orders = _orderBackupManager.LoadBackup().ToList();
        }
        OnRestoreBackupCompleted(EventArgs.Empty);
    }

    #endregion Backup
}
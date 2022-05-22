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

    #region Orders

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

    public IEnumerable<Order> GetOrdersForProduct(Product.Product product)
    {
        var comparer = new UniqueProductComparer();
        var retval = Orders.Where(o => o.OrderItems.Any(i => comparer.Equals(i.Product, product)));
        return retval;
    }

    public IEnumerable<Product.Product> GetAllProducts()
    {
        return _productFetcher.GetAllProducts();
    }

    public int GetNumberOfItemsSoldForProduct(Product.Product product)
    {
        var retval = 0;
        if (!Orders.Any())
            return retval;
        var comparer = new UniqueProductComparer();
        foreach (var order in Orders.Where(o => o.OrderItems.Any(i => comparer.Equals(i.Product, product))))
        {
            retval += order.OrderItems.Where(o => comparer.Equals(o.Product, product)).Sum(o => o.NumberOfItems);
        }

        return retval;
    }

    public Product.Product FindProduct(long productId)
    {
        return _productFetcher.FindProduct(productId);
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
        var converter = new OrderDTOConverter();
        _orderBackupManager.Backup(converter.ConvertToDTO(GetAllOrders()));
        OnBackupCompleted(EventArgs.Empty);
    }

    private void OnRestoreBackupCompleted(EventArgs e)
    {
        var handler = RestoreBackupCompleted;
        handler?.Invoke(this, e);
    }

    public void RestoreBackup()
    {
        var converter = new OrderDTOConverter();
        Orders = converter.ConvertFromDTO(_orderBackupManager.LoadBackup()).ToList();
        OnRestoreBackupCompleted(EventArgs.Empty);
    }

    #endregion Backup
}
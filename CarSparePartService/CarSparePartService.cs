﻿using CarSparePartService.Backup;
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

    public IEnumerable<ProductWithItemsCount> GetProductsWithItemsCount()
    {
        var retval = new List<ProductWithItemsCount>();
        var products = _productFetcher.GetAllProducts(); 
        foreach (var product in products)
        {
            retval.Add(new ProductWithItemsCount(product, GetNumberOfItemsSoldForProduct(product)));
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
    
    private int GetNumberOfItemsSoldForProduct(Product.Product product)
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
    
    #endregion Orders
    
    #region Backup
    private void OnBackupCompleted(EventArgs e)
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

    private void OnRestoreBackupCompleted(EventArgs e)
    {
        var handler = RestoreBackupCompleted;
        handler?.Invoke(this, e);
    }
    public void LoadBackup(string filePath)
    {
        if (!File.Exists(filePath))
        {
            _logger.Error($"Could not restore from backup - file: {filePath} doesn't exist");
            return;
        }
        var converter = new OrderDTOConverter();
        Orders = converter.ConvertFromDTO(_orderBackupManager.LoadBackupFromFile(filePath)).ToList();
        OnRestoreBackupCompleted(EventArgs.Empty);
    }

    #endregion Backup
}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using CarSparePartService;
using CarSparePartService.Interfaces;
using CarSparePartStore.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Input;
using OnlineStoreEmulator;

namespace CarSparePartStore.ViewModels;

public class CarSparePartViewModel
    :ObservableRecipient
{
    private readonly IProductFetcher _productFetcher;
    private readonly ICustomerService _customerService;
    private ICarSparePartService _carSparePartService;
    private readonly CancellationTokenSource cancellationTokenSource;
    
    public CarSparePartViewModel(ICarSparePartService carSparePartService, IProductFetcher productFetcher, ICustomerService customerService)
    {
        _productFetcher = productFetcher;
        _customerService = customerService;
        _carSparePartService = carSparePartService;
        ProductsWithOrders = new ObservableCollection<ProductWithOrders>();
        
        cancellationTokenSource = new CancellationTokenSource();
        
        ThreadPool.QueueUserWorkItem(new WaitCallback(LookForOrderUpdates), cancellationTokenSource.Token);
        
        Content = new CarSparePartListView();
    }

    private void LookForOrderUpdates(object? state)
    {
        while (true)
        {
            Application.Current?.Dispatcher?.Invoke(() => UpdateProductsWithOrders());
            Thread.Sleep(TimeSpan.FromSeconds(15));
        }
    }

    public void CancelOrderUpdates()
    {
        cancellationTokenSource.Cancel();
    }

    public void UpdateProductsWithOrders()
    {
        ProductsWithOrders.Clear();
        var productsWithOrders = GetProductsWithOrders();
        foreach (var productWithOrders in productsWithOrders)
        {
            ProductsWithOrders.Add(productWithOrders);   
        }
    }

    public ObservableCollection<ProductWithOrders> ProductsWithOrders { get; private set; }

    private UserControl _content;
    public UserControl Content
    {
        get => _content;
        private set => SetProperty(ref _content, value);
    }

    #region Commands
    private RelayCommand _createOrderCommand;
    public RelayCommand CreateOrderCommand
    {
        get { return _createOrderCommand ?? (_createOrderCommand = new RelayCommand(CreateOrder, CanCreateOrder)); }
    }

    private RelayCommand _placeNewOrderCommand;
    public RelayCommand PlaceNewOrderCommand
    {
        get { return _placeNewOrderCommand ?? (_placeNewOrderCommand = new RelayCommand(PlaceOrder, CanPlaceOrder)); }
    }

    private RelayCommand _cancelNewOrderCommand;
    public RelayCommand CancelNewOrderCommand
    {
        get { return _cancelNewOrderCommand ?? (_cancelNewOrderCommand = new RelayCommand(CancelOrder, CanCancelOrder)); }
    }

    private RelayCommand _backupOrdersCommand;
    public RelayCommand BackupOrdersCommand
    {
        get { return _backupOrdersCommand ?? (_backupOrdersCommand = new RelayCommand(BackupOrders, CanBackupOrders)); }
    }
    
    private RelayCommand _restoreOrdersFromBackupCommand;
    public RelayCommand RestoreOrdersFromBackupCommand
    {
        get { return _restoreOrdersFromBackupCommand ?? (_restoreOrdersFromBackupCommand = new RelayCommand(RestoreOrdersFromBackup, CanRestoreOrders)); }
    }
    #endregion Commands
    
    private bool CanCancelOrder()
    {
        return true;
    }

    private void CancelOrder()
    {
        IsOrderCreationInProgress = false;
        Content = new CarSparePartListView();
    }
    
    private bool CanCreateOrder()
    {
        return !IsOrderCreationInProgress;
    }

    private bool _isOrderCreationInProgress;
    public bool IsOrderCreationInProgress
    {
        get => _isOrderCreationInProgress;
        set
        {
            SetProperty(ref _isOrderCreationInProgress, value); 
            CreateOrderCommand.NotifyCanExecuteChanged();
        }
    }


    private void CreateOrder()
    {
        IsOrderCreationInProgress = true;
        try
        {
            //Create the order instance
            Order = Order.Create(0, new List<OrderItem>());
            //Show the create new order view
            Content = new CarSparePartNewOrder();
        }
        catch
        {
            IsOrderCreationInProgress = false;
        }
    }

    private bool CanPlaceOrder()
    {
        if (SelectedCustomer is null || SelectedProduct is null)
        {
            return false;
        }
        return true;
    }

    private void PlaceOrder()
    {
        Order.CustomerId = SelectedCustomer.CustomerId;
        Order.AddItem(new OrderItem{Product = SelectedProduct, NumberOfItems = NumberOfItems});
        _carSparePartService.PlaceOrder(Order);
    }
    
    private bool CanBackupOrders()
    {
        return true;
    }

    private void BackupOrders()
    {
        StopEmulator();
        var configuration = Ioc.Default.GetRequiredService<IConfiguration>();
        var backupFilename = configuration.GetSection("ApplicationSettings").GetSection("OrdersBackup").Value;
        _carSparePartService.CreateBackup(backupFilename);
        StartEmulator();
    }
    
    private bool CanRestoreOrders()
    {
        return true;
    }
    
    private void RestoreOrdersFromBackup()
    {
        StopEmulator();
        var configuration = Ioc.Default.GetRequiredService<IConfiguration>();
        var backupFilename = configuration.GetSection("ApplicationSettings").GetSection("OrdersBackup").Value;
        if (!File.Exists(backupFilename))
        {
            //Show message to user
            return;
        }
        _carSparePartService.LoadBackup(backupFilename);
        StartEmulator();
        UpdateProductsWithOrders();
    }

    private void StopEmulator()
    {
        var emulator = Ioc.Default.GetRequiredService<IOnlineStoreEmulator>();
        emulator.Stop();
    }
    
    private void StartEmulator()
    {
        var emulator = Ioc.Default.GetRequiredService<IOnlineStoreEmulator>();
        emulator.Start();
    }

    public ObservableCollection<Customer> Customers
    {
        get
        {
            return new ObservableCollection<Customer>(_customerService.GetAllCustomers());
        }
    }

    private Customer _selectedCustomer;
    public Customer SelectedCustomer
    {
        get => _selectedCustomer;
        set
        {
            SetProperty(ref _selectedCustomer, value); 
            PlaceNewOrderCommand.NotifyCanExecuteChanged();
        }
    }

    public ObservableCollection<Product> Products
    {
        get
        {
            return new ObservableCollection<Product>(_productFetcher.GetAllProducts());
        }
    }
    private Product _selectedProduct;
    public Product SelectedProduct
    {
        get => _selectedProduct;
        set
        {
            SetProperty(ref _selectedProduct, value); 
            PlaceNewOrderCommand.NotifyCanExecuteChanged();
        }
    }

    private int _numberOfItems;
    public int NumberOfItems
    {
        get => _numberOfItems;
        set => SetProperty(ref _numberOfItems, value);
    }
    
    private Order _order;
    public Order Order 
    {
        get => _order;
        set => SetProperty(ref _order, value);
    }

    public IEnumerable<ProductWithOrders> GetProductsWithOrders()
    {
        var retval = new List<ProductWithOrders>();
        var allOrders = _carSparePartService.GetAllOrders().ToList();
        var products = allOrders.SelectMany(o => o.OrderItems.Select(i => i.Product)).Distinct(new UniqueProductComparer()).ToList();
        foreach (var product in products)
        {
            retval.Add(new ProductWithOrders(product, GetNumberOfItemsSold(allOrders, product)));
        }
        return retval;
    }

    private int GetNumberOfItemsSold(List<Order> allOrders, Product product)
    {
        if (allOrders == null || allOrders.Count == 0)
            return 0;
        var retval = 0;
        foreach (var order in allOrders.Where(o => o.OrderItems.Any(i => i.Product.ProductId == product.ProductId)))
        {
            retval += order.OrderItems.Where(i => i.Product.ProductId == product.ProductId).Sum(item => item.NumberOfItems);
        }
        return retval;
    }
}
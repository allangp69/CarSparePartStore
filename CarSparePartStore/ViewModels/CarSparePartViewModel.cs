using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CarSparePartService;
using CarSparePartService.Interfaces;
using CarSparePartService.Product;
using CarSparePartStore.ExtensionMethods;
using CarSparePartStore.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Input;
using OnlineStoreEmulator;

namespace CarSparePartStore.ViewModels;

public sealed  class CarSparePartViewModel
    : ObservableRecipient
{
    private readonly IProductFetcher _productFetcher;
    private readonly ICustomerService _customerService;
    private readonly IOnlineStoreEmulator _onlineStoreEmulator;
    private readonly ICarSparePartService _carSparePartService;

    public CarSparePartViewModel(ICarSparePartService carSparePartService, IProductFetcher productFetcher,
        ICustomerService customerService, IOnlineStoreEmulator onlineStoreEmulator)
    {
        _productFetcher = productFetcher;
        _customerService = customerService;
        _onlineStoreEmulator = onlineStoreEmulator;
        _onlineStoreEmulator.IsRunningChanged += OnlineStoreEmulatorIsRunningChanged;
        _carSparePartService = carSparePartService;
        _carSparePartService.OrderAdded += CarSparePartServiceOrderAdded;
        _carSparePartService.RestoreBackupCompleted += CarSparePartServiceRestoreBackupCompleted;
        _carSparePartService.BackupCompleted += CarSparePartServiceBackupCompleted;
        ProductsWithItemsCount = new ObservableCollection<ProductWithItemsCount>();
        OrdersForProduct = new ObservableCollection<Order>();
        Notifications = new List<string>();
        PeriodFromDate = DateTime.Today;
        PeriodFromTime = "0000";
        PeriodToDate = DateTime.Today.AddDays(1);
        PeriodToTime = "0000";
    }

    protected override void OnActivated()
    {
        ShowDefaultView();
        StartEmulator();
    }
    private List<string> Notifications { get; }

    private void OnlineStoreEmulatorIsRunningChanged(object? sender, IsRunningEventArgs e)
    {
        IsOnlineStoreRunning = e.IsRunning;
    }

    private bool _isOnlineStoreRunning;
    public bool IsOnlineStoreRunning
    {
        get => _isOnlineStoreRunning;
        set
        {
            SetProperty(ref _isOnlineStoreRunning, value);
            OnPropertyChanged(nameof(IsOnlineStoreRunningText));
            StartEmulatorCommand.NotifyCanExecuteChanged();
            StopEmulatorCommand.NotifyCanExecuteChanged();
        }
    }

    public string IsOnlineStoreRunningText
    {
        get
        {
            var notOrEmptyString = IsOnlineStoreRunning ? "" : "not";
            return $"The OnlineStoreEmulator is {notOrEmptyString} running.";
        }
    }
    private void CarSparePartServiceOrderAdded(object? sender, OrderAddedEventArgs e)
    {
        Application.Current?.Dispatcher?.Invoke(() =>
        {
            UpdateProductsWithOrders();
            AddNotification($"Order added - customerId: {e.CustomerId} - products: {e.Products}");
        });
    }

    private void AddNotification(string notification)
    {
        Notifications.Add(notification);
        OnPropertyChanged(nameof(LatestNotification));
    }

    private void CarSparePartServiceBackupCompleted(object? sender, EventArgs e)
    {
        Application.Current?.Dispatcher?.Invoke(() => { AddNotification($"Backup of orders completed"); });
    }

    private void CarSparePartServiceRestoreBackupCompleted(object? sender, EventArgs e)
    {
        Application.Current?.Dispatcher?.Invoke(() =>
        {
            AddNotification($"Restore orders from backup completed");
            UpdateProductsWithOrders();
        });
    }

    public void UpdateProductsWithOrders()
    {
        ProductsWithItemsCount.Clear();
        var productsWithOrders = _carSparePartService.GetProductsWithItemsCount();
        foreach (var productWithOrders in productsWithOrders)
        {
            ProductsWithItemsCount.Add(productWithOrders);
        }
    }

    private ProductWithItemsCount _orderListSelectedProduct;
    public ProductWithItemsCount OrderListSelectedProduct
    {
        get => _orderListSelectedProduct;
        set
        {
            SetProperty(ref _orderListSelectedProduct, value); 
            OrdersForProductCommand.NotifyCanExecuteChanged();   
        }
    }
    
    public ObservableCollection<ProductWithItemsCount> ProductsWithItemsCount { get; private set; }

    private UserControl _content;
    public UserControl Content
    {
        get => _content;
        private set => SetProperty(ref _content, value);
    }

    #region Commands

    private RelayCommand _ordersForProductCommand;
    public RelayCommand OrdersForProductCommand
    {
        get
        {
            return _ordersForProductCommand ?? (_ordersForProductCommand = new RelayCommand(ShowOrdersForProduct, CanShowOrdersForProduct));
        }
    }
    
    private RelayCommand _closeOrdersForProductCommand;
    public RelayCommand CloseOrdersForProductCommand
    {
        get
        {
            return _closeOrdersForProductCommand ?? (_closeOrdersForProductCommand = new RelayCommand(CloseOrdersForProduct, CanCloseOrdersForProduct));
        }
    }
    
    

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
        get
        {
            return _cancelNewOrderCommand ?? (_cancelNewOrderCommand = new RelayCommand(CancelOrder, CanCancelOrder));
        }
    }

    private RelayCommand _backupOrdersCommand;
    public RelayCommand BackupOrdersCommand
    {
        get { return _backupOrdersCommand ?? (_backupOrdersCommand = new RelayCommand(BackupOrders, CanBackupOrders)); }
    }

    private RelayCommand _restoreOrdersFromBackupCommand;
    public RelayCommand RestoreOrdersFromBackupCommand
    {
        get
        {
            return _restoreOrdersFromBackupCommand ?? (_restoreOrdersFromBackupCommand = new RelayCommand(RestoreOrdersFromBackup, CanRestoreOrders));
        }
    }

    private RelayCommand _startEmulatorCommand;
    public RelayCommand StartEmulatorCommand
    {
        get
        {
            return _startEmulatorCommand ?? (_startEmulatorCommand = new RelayCommand(StartEmulator, CanStartEmulator));
        }
    }

    private RelayCommand _stopEmulatorCommand;
    public RelayCommand StopEmulatorCommand
    {
        get
        {
            return _stopEmulatorCommand ?? (_stopEmulatorCommand = new RelayCommand(StopEmulator, CanStopEmulator));
        }
    }

    #endregion Commands

    private bool CanShowOrdersForProduct()
    {
        return OrderListSelectedProduct is not null;
    }

    private void ShowOrdersForProduct()
    {
        OrdersForProductSelectedProduct = GetProductByProductId(OrderListSelectedProduct.ProductId);
        //Show the orders for product view
        Content = new OrdersForProductView();
    }

    private Product GetProductByProductId(long productId)
    {
        return _productFetcher.FindProduct(productId);
    }

    private bool CanCloseOrdersForProduct()
    {
        return true;
    }
    
    private void CloseOrdersForProduct()
    {
        ShowDefaultView();
    }

    private void ShowDefaultView()
    {
        Content = new CarSparePartListView();
    }

    private bool CanCancelOrder()
    {
        return true;
    }

    private void CancelOrder()
    {
        IsOrderCreationInProgress = false;
        ShowDefaultView();
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
        Order.AddItem(new OrderItem {Product = SelectedProduct, NumberOfItems = NumberOfItems});
        _carSparePartService.PlaceOrder(Order);
        ClearSelections();
    }

    private void ClearSelections()
    {
        SelectedCustomer = null;
        SelectedProduct = null;
        NumberOfItems = 0;
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
        _onlineStoreEmulator.Stop();
    }

    private bool CanStopEmulator()
    {
        return IsOnlineStoreRunning;
    }
    
    private void StartEmulator()
    {
        _onlineStoreEmulator.Start();
    }

    private bool CanStartEmulator()
    {
        return !IsOnlineStoreRunning;
    }
    
    public ObservableCollection<Customer> Customers
    {
        get { return new ObservableCollection<Customer>(_customerService.GetAllCustomers()); }
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
        get { return new ObservableCollection<Product>(_productFetcher.GetAllProducts()); }
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

    public string LatestNotification
    {
        get { return Notifications.Any() ? Notifications.Last() : string.Empty; }
    }
    
    private DateTime _periodFromDate;
    public DateTime PeriodFromDate
    {
        get => _periodFromDate;
        set
        {
            SetProperty(ref _periodFromDate, value);
            UpdateOrdersForProduct();
        }
    }

    private string _periodFromTime;
    public string PeriodFromTime
    {
        get => _periodFromTime;
        set
        {
            SetProperty(ref _periodFromTime, value);
            UpdateOrdersForProduct();
        }
    }

    private DateTime _periodToDate;
    public DateTime PeriodToDate
    {
        get => _periodToDate;
        set
        {
            SetProperty(ref _periodToDate, value);
            UpdateOrdersForProduct();
        }
    }

    private string _periodToTime;
    public string PeriodToTime
    {
        get => _periodToTime;
        set
        {
            SetProperty(ref _periodToTime, value);
            UpdateOrdersForProduct();
        }
    }

    public Product _ordersForProductSelectedProduct;
    public Product OrdersForProductSelectedProduct
    {
        get => _ordersForProductSelectedProduct;
        set
        {
            SetProperty(ref _ordersForProductSelectedProduct, value);
            UpdateOrdersForProduct();
        }
    }

    private void UpdateOrdersForProduct()
    {
        OrdersForProduct.Clear();
        var orders = _carSparePartService.GetOrdersForProduct(OrdersForProductSelectedProduct);
        var fromDateTime = PeriodFromDate.AddTime(PeriodFromTime);
        var toDateTime = PeriodToDate.AddTime(PeriodToTime);
        foreach (var order in orders.Where(o => o.OrderDateTime >= fromDateTime && o.OrderDateTime <= toDateTime))
        {
            OrdersForProduct.Add(order);
        }
    }

    private ObservableCollection<Order> _ordersForProduct;
    public ObservableCollection<Order> OrdersForProduct
    {
        get => _ordersForProduct;
        set => SetProperty(ref _ordersForProduct, value);
    }
    
    private Order _ordersForProductSelectedOrder;
    public Order OrdersForProductSelectedOrder
    {
        get => _ordersForProductSelectedOrder;
        set => SetProperty(ref _ordersForProductSelectedOrder, value);
    }
}
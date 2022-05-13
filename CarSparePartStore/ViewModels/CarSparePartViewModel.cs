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
using CarSparePartStore.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Input;
using OnlineStoreEmulator;

namespace CarSparePartStore.ViewModels;

public class CarSparePartViewModel
    : ObservableRecipient
{
    private readonly IProductFetcher _productFetcher;
    private readonly ICustomerService _customerService;
    private ICarSparePartService _carSparePartService;

    public CarSparePartViewModel(ICarSparePartService carSparePartService, IProductFetcher productFetcher,
        ICustomerService customerService)
    {
        _productFetcher = productFetcher;
        _customerService = customerService;
        _carSparePartService = carSparePartService;
        ProductsWithOrders = new ObservableCollection<ProductWithOrders>();
        Content = new CarSparePartListView();
        Notifications = new List<string>();
        _carSparePartService.OrderAdded += CarSparePartServiceOrderAdded;
        _carSparePartService.RestoreBackupCompleted += CarSparePartServiceRestoreBackupCompleted;
        _carSparePartService.BackupCompleted += CarSparePartServiceBackupCompleted;
        StartEmulator();
    }

    private List<string> Notifications { get; }

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
        ProductsWithOrders.Clear();
        var productsWithOrders = GetProductsWithOrders();
        foreach (var productWithOrders in productsWithOrders)
        {
            ProductsWithOrders.Add(productWithOrders);
        }
    }

    private ProductWithOrders _orderListSelectedProject;
    public ProductWithOrders OrderListSelectedProject
    {
        get => _orderListSelectedProject;
        set
        {
            SetProperty(ref _orderListSelectedProject, value); 
            OrdersForProductCommand.NotifyCanExecuteChanged();   
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

    private RelayCommand _ordersForProductCommand;

    public RelayCommand OrdersForProductCommand
    {
        get
        {
            return _ordersForProductCommand ?? (_ordersForProductCommand = new RelayCommand(ShowOrdersForProduct, CanShowOrdersForProduct));
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

    #endregion Commands

    private bool CanShowOrdersForProduct()
    {
        return OrderListSelectedProject is not null;
    }

    private void ShowOrdersForProduct()
    {
        //Show the orders for product view
        Content = new OrdersForProductView();
    }

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

    public IEnumerable<ProductWithOrders> GetProductsWithOrders()
    {
        return _carSparePartService.GetProductsWithOrders().ToList();
    }
}
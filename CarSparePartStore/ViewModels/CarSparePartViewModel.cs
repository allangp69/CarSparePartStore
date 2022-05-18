using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using CarSparePartService;
using CarSparePartService.Interfaces;
using CarSparePartService.Product;
using CarSparePartStore.ViewModels.Notification;
using Microsoft.Extensions.Configuration;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Input;
using OnlineStoreEmulator;

namespace CarSparePartStore.ViewModels;

public sealed  class CarSparePartViewModel
    : ObservableRecipient, IDisposable
{
    private readonly IOnlineStoreEmulator _onlineStoreEmulator;
    private readonly ICarSparePartService _carSparePartService;
    private readonly NotificationHandler _notificationHandler;

    public CarSparePartViewModel(ICarSparePartService carSparePartService, IOnlineStoreEmulator onlineStoreEmulator, NotificationHandler notificationHandler)
    {
        ActiveNotifications = new ObservableCollection<Notification.Notification>();
        _notificationHandler = notificationHandler;
        _notificationHandler.NotificationAdded += NotificationHandlerOnNotificationAdded;
        _notificationHandler.NotificationRemoved += NotificationHandlerOnNotificationRemoved;
        _onlineStoreEmulator = onlineStoreEmulator;
        _onlineStoreEmulator.IsRunningChanged += OnlineStoreEmulatorIsRunningChanged;
        _carSparePartService = carSparePartService;
        _carSparePartService.OrderAdded += CarSparePartServiceOrderAdded;
        _carSparePartService.RestoreBackupCompleted += CarSparePartServiceRestoreBackupCompleted;
        _carSparePartService.BackupCompleted += CarSparePartServiceBackupCompleted;
        ProductsWithItemsCount = new ObservableCollection<ProductWithItemsCount>();
        Notifications = new List<Notification.Notification>();
    }

    private void NotificationHandlerOnNotificationAdded(object? sender, NotificationAddedEventArgs e)
    {
        AddActiveNotification(e.Notification);
    }
    
    private void NotificationHandlerOnNotificationRemoved(object? sender, NotificationRemovedEventArgs e)
    {
        RemoveActiveNotification(e.Notification);
    }

    private void AddActiveNotification(Notification.Notification notification)
    {
        Application.Current?.Dispatcher?.Invoke(() =>
        {
            ActiveNotifications.Add(notification);
            OnPropertyChanged(nameof(HasActiveNotifications));
        });
    }
    
    private void RemoveActiveNotification(Notification.Notification notification)
    {
        Application.Current?.Dispatcher?.Invoke(() =>
        {
            ActiveNotifications.Remove(notification);
            OnPropertyChanged(nameof(HasActiveNotifications));
        });
    }

    protected override void OnActivated()
    {
        ShowDefaultView();
        StartEmulator();
    }
    private List<Notification.Notification> Notifications { get; }

    private ObservableCollection<Notification.Notification> _activeNotifications; 
    public ObservableCollection<Notification.Notification> ActiveNotifications 
    {
        get => _activeNotifications;
        set => SetProperty(ref _activeNotifications, value);
    }

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

    private void UpdateProductsWithOrders()
    {
        var vm = Content as CarSparePartListViewModel;
        if (vm is null)
        {
            return;
        }
        vm.UpdateProductsWithOrders();
    }

    private void AddNotification(string notificationMessage)
    {
        var notification = new Notification.Notification(notificationMessage);
        Notifications.Add(notification);
        OnPropertyChanged(nameof(LatestNotification));
        _notificationHandler.AddNotification(notification);
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
    
    public ObservableCollection<ProductWithItemsCount> ProductsWithItemsCount { get; }

    private ObservableRecipient _content;
    public ObservableRecipient Content
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
        return ListSelectedProductId > 0;
    }

    private void ShowOrdersForProduct()
    {
        var vm = Ioc.Default.GetService<OrdersForProductViewModel>();
        if (vm is null)
        {
            return;
        }
        vm.ProductId = ListSelectedProductId;
        vm.OrdersForProductClosed += OrdersForProductClosed;
        ShowView(vm);
    }

    private void OrdersForProductClosed(object? sender, EventArgs e)
    {
        ShowDefaultView();
    }

    public long ListSelectedProductId
    {
        get => _listSelectedProductId;
        set
        {
            SetProperty(ref _listSelectedProductId, value);
            OrdersForProductCommand.NotifyCanExecuteChanged();
        }
    }

    private void ShowDefaultView()
    {
        var vm = Ioc.Default.GetService<CarSparePartListViewModel>();
        vm.ProductSelected += CarSparePartListProductSelected;
        ShowView(vm);
    }

    private void CarSparePartListProductSelected(object? sender, ProductSelectedEventArgs e)
    {
        ListSelectedProductId = e.ProductId;
    }

    private void ShowView(ObservableRecipient content)
    {
        Content = null;
        Content = content;
        content.IsActive = true;
    }
    
    private bool CanCreateOrder()
    {
        return !IsOrderCreationInProgress;
    }

    private bool _isOrderCreationInProgress;
    private long _listSelectedProductId;

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
            //Show the create new order view
            var vm = Ioc.Default.GetService<CarSparePartNewOrderViewModel>();
            if (vm is null)
                return;
            vm.NewOrderCancelled += CarSparePartNewOrderNewOrderCancelled;
            vm.NewOrderClosed += CarSparePartNewOrderNewOrderClosed;
            ShowView(vm);
        }
        catch
        {
            IsOrderCreationInProgress = false;
        }
    }

    private void CarSparePartNewOrderNewOrderCancelled(object? sender, EventArgs e)
    {
        IsOrderCreationInProgress = false;
        ShowDefaultView();
    }
    
    private void CarSparePartNewOrderNewOrderClosed(object? sender, EventArgs e)
    {
        IsOrderCreationInProgress = false;
        ShowDefaultView();
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

    public string LatestNotification
    {
        get { return Notifications.Any() ? Notifications.Last().Message : string.Empty; }
    }

    public bool HasActiveNotifications
    {
        get { return ActiveNotifications.Any(); }
    }

    public void Dispose()
    {
        _notificationHandler.Dispose();
        _notificationHandler.NotificationAdded -= NotificationHandlerOnNotificationAdded;
        _notificationHandler.NotificationRemoved -= NotificationHandlerOnNotificationRemoved;
        _onlineStoreEmulator.IsRunningChanged -= OnlineStoreEmulatorIsRunningChanged;
        _carSparePartService.OrderAdded -= CarSparePartServiceOrderAdded;
        _carSparePartService.RestoreBackupCompleted -= CarSparePartServiceRestoreBackupCompleted;
        _carSparePartService.BackupCompleted -= CarSparePartServiceBackupCompleted;
    }
}
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
using CarSparePartStore.ViewModels.Notification;
using CarSparePartStore.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Input;
using OnlineStoreEmulator;

namespace CarSparePartStore.ViewModels;

public sealed  class OrdersForProductViewModel
    : ObservableRecipient, IDisposable
{
    private readonly IProductFetcher _productFetcher;
    private readonly ICarSparePartService _carSparePartService;

    public OrdersForProductViewModel(ICarSparePartService carSparePartService, IProductFetcher productFetcher)
    {
        _productFetcher = productFetcher;
        _carSparePartService = carSparePartService;
        OrdersForProduct = new ObservableCollection<Order>();
        PeriodFromDate = DateTime.Today;
        PeriodFromTime = "0000";
        PeriodToDate = DateTime.Today.AddDays(1);
        PeriodToTime = "0000";
    }

    protected override void OnActivated()
    {
        SelectedProduct = _productFetcher.FindProduct(ProductId);
        UpdateOrdersForProduct();
    }
    
    #region Commands
    
    private RelayCommand _closeOrdersForProductCommand;
    public RelayCommand CloseOrdersForProductCommand
    {
        get
        {
            return _closeOrdersForProductCommand ?? (_closeOrdersForProductCommand = new RelayCommand(CloseOrdersForProduct, CanCloseOrdersForProduct));
        }
    }

    #endregion Commands

    private bool CanCloseOrdersForProduct()
    {
        return true;
    }
    
    private void CloseOrdersForProduct()
    {
        throw new Exception();
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

    private Product _selectedProduct;
    public Product SelectedProduct
    {
        get => _selectedProduct;
        set
        {
            SetProperty(ref _selectedProduct, value);
            UpdateOrdersForProduct();
        }
    }

    private void UpdateOrdersForProduct()
    {
        OrdersForProduct.Clear();
        var orders = _carSparePartService.GetOrdersForProduct(SelectedProduct);
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
    
    private Order _selectedOrder;
    public Order SelectedOrder
    {
        get => _selectedOrder;
        set => SetProperty(ref _selectedOrder, value);
    }

    public long ProductId { get; set; }

    public void Dispose()
    {
        //Dispose;
    }
}
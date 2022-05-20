using System;
using System.Collections.ObjectModel;
using System.Linq;
using CarSparePartService;
using CarSparePartService.Interfaces;
using CarSparePartService.Product;
using CarSparePartStore.Controller;
using CarSparePartStore.ExtensionMethods;
using Microsoft.Toolkit.Mvvm.Input;

namespace CarSparePartStore.ViewModels;

public sealed  class OrdersForProductViewModel
    : CarSparePartViewContent, IDisposable
{
    private readonly IProductFetcher _productFetcher;
    private readonly ICarSparePartService _carSparePartService;
    
    public event EventHandler OrdersForProductClosed;

    public OrdersForProductViewModel(ICarSparePartService carSparePartService, IProductFetcher productFetcher)
    {
        _productFetcher = productFetcher;
        _carSparePartService = carSparePartService;
        OrdersForProduct = new ObservableCollection<Order>();
        Products = new ObservableCollection<Product>(productFetcher.GetAllProducts());
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
        OnOrdersForProductClosed();
    }
    
    private void OnOrdersForProductClosed()
    {
        var handler = OrdersForProductClosed;
        handler?.Invoke(this, System.EventArgs.Empty);
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

    private ObservableCollection<Product> _products;
    public ObservableCollection<Product> Products
    {
        get => _products;
        private set => _products = value;
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
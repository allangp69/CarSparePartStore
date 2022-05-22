using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CarSparePartStore.Adapters;
using CarSparePartStore.ExtensionMethods;
using CarSparePartStore.ViewModels.DTO;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace CarSparePartStore.ViewModels;

public sealed  class OrdersForProductViewModel
    : ObservableRecipient, IDisposable
{
    private readonly IProductsAndOrdersAdapter _productsAndOrdersAdapter;
   
    public event EventHandler OrdersForProductClosed;

    public OrdersForProductViewModel(IProductsAndOrdersAdapter productsAndOrdersAdapter)
    {
        _productsAndOrdersAdapter = productsAndOrdersAdapter;
        OrdersForProduct = new ObservableCollection<OrderDTO>();
        Products = new ObservableCollection<ProductDTO>(_productsAndOrdersAdapter.GetAllProducts());
        PeriodFromDate = DateTime.Today;
        PeriodFromTime = "0000";
        PeriodToDate = DateTime.Today.AddDays(1);
        PeriodToTime = "0000";
    }

    protected override void OnActivated()
    {
        SelectedProduct = _productsAndOrdersAdapter.FindProduct(ProductId);
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
        handler?.Invoke(this, EventArgs.Empty);
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

    private ObservableCollection<ProductDTO> _products;
    public ObservableCollection<ProductDTO> Products
    {
        get => _products;
        private set => _products = value;
    }

    private ProductDTO _selectedProduct;
    public ProductDTO SelectedProduct
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
        var orders = GetOrdersForProduct(SelectedProduct);
        var fromDateTime = PeriodFromDate.AddTime(PeriodFromTime);
        var toDateTime = PeriodToDate.AddTime(PeriodToTime);
        foreach (var order in orders.Where(o => o.OrderDateTime >= fromDateTime && o.OrderDateTime <= toDateTime))
        {
            OrdersForProduct.Add(order);
        }
    }

    private IEnumerable<OrderDTO> GetOrdersForProduct(ProductDTO product)
    {
        return _productsAndOrdersAdapter.GetOrdersForProduct(product);
    }

    private ObservableCollection<OrderDTO> _ordersForProduct;
    public ObservableCollection<OrderDTO> OrdersForProduct
    {
        get => _ordersForProduct;
        set => SetProperty(ref _ordersForProduct, value);
    }
    
    private OrderDTO _selectedOrder;
    public OrderDTO SelectedOrder
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
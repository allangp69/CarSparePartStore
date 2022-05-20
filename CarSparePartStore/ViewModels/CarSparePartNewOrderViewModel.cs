using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CarSparePartService;
using CarSparePartService.Interfaces;
using CarSparePartService.Product;
using CarSparePartStore.Controller;
using Microsoft.Toolkit.Mvvm.Input;

namespace CarSparePartStore.ViewModels;

public sealed  class CarSparePartNewOrderViewModel
    : CarSparePartViewContent, IDisposable
{
    private readonly ICarSparePartService _carSparePartService;

    public event EventHandler NewOrderCancelled;
    public event EventHandler NewOrderClosed;
    
    public CarSparePartNewOrderViewModel(ICarSparePartService carSparePartService, IProductFetcher productFetcher, ICustomerService customerService)
    {
        _carSparePartService = carSparePartService;
        Order = Order.Create(0, new List<OrderItem>());
        Customers = new ObservableCollection<Customer>(customerService.GetAllCustomers());
        Products = new ObservableCollection<Product>(productFetcher.GetAllProducts());
    }
    
    #region Commands
    
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
    
    #endregion Commands

    private bool CanCancelOrder()
    {
        return true;
    }

    private void CancelOrder()
    {
        OnNewOrderCancelled();
    }

    private void OnNewOrderCancelled()
    {
        var handler = NewOrderCancelled;
        handler?.Invoke(this, System.EventArgs.Empty);
    }
    
    private void OnNewOrderClosed()
    {
        var handler = NewOrderClosed;
        handler?.Invoke(this, System.EventArgs.Empty);
    }

    private bool CanPlaceOrder()
    {
        return SelectedCustomer is not null && SelectedProduct is not null && NumberOfItems > 0;
    }

    private void PlaceOrder()
    {
        Order.CustomerId = SelectedCustomer.CustomerId;
        Order.AddItem(new OrderItem {Product = SelectedProduct, NumberOfItems = NumberOfItems});
        _carSparePartService.PlaceOrder(Order);
        ClearSelections();
        OnNewOrderClosed();
    }

    private void ClearSelections()
    {
        SelectedCustomer = null;
        SelectedProduct = null;
        NumberOfItems = 0;
    }

    private ObservableCollection<Customer> _customers;
    public ObservableCollection<Customer> Customers
    {
        get => _customers;
        private set => _customers = value;
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
    
    public void Dispose()
    {
        //Dispose
    }
}
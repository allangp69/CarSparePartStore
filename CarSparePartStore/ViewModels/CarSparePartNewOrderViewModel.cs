using System;
using System.Collections.ObjectModel;
using CarSparePartStore.Adapters;
using CarSparePartStore.ViewModels.DTO;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace CarSparePartStore.ViewModels;

public sealed  class CarSparePartNewOrderViewModel
    : ObservableRecipient, IDisposable
{
    private readonly ProductsAndOrdersAdapter _productsAndOrdersAdapter;
    private readonly ICustomerAdapter _customerAdapter;

    public event EventHandler NewOrderCancelled;
    public event EventHandler NewOrderClosed;
    
    public CarSparePartNewOrderViewModel(ProductsAndOrdersAdapter productsAndOrdersAdapter, ICustomerAdapter customerAdapter)
    {
        _productsAndOrdersAdapter = productsAndOrdersAdapter;
        _customerAdapter = customerAdapter;
        Order = new OrderDTO();
        Customers = new ObservableCollection<CustomerDTO>(_customerAdapter.GetAllCustomers());
        Products = new ObservableCollection<ProductDTO>(_productsAndOrdersAdapter.GetAllProducts());
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
        handler?.Invoke(this, EventArgs.Empty);
    }
    
    private void OnNewOrderClosed()
    {
        var handler = NewOrderClosed;
        handler?.Invoke(this, EventArgs.Empty);
    }

    private bool CanPlaceOrder()
    {
        return SelectedCustomer is not null && SelectedProduct is not null && NumberOfItems > 0;
    }

    private void PlaceOrder()
    {
        Order.CustomerId = SelectedCustomer.CustomerId;
        Order.OrderItems.Add(new OrderItemDTO {Product = SelectedProduct, NumberOfItems = NumberOfItems});
        _productsAndOrdersAdapter.PlaceOrder(Order);
        ClearSelections();
        OnNewOrderClosed();
    }

    private void ClearSelections()
    {
        SelectedCustomer = null;
        SelectedProduct = null;
        NumberOfItems = 0;
    }

    private ObservableCollection<CustomerDTO> _customers;
    public ObservableCollection<CustomerDTO> Customers
    {
        get => _customers;
        private set => _customers = value;
    }

    private CustomerDTO _selectedCustomer;
    public CustomerDTO SelectedCustomer
    {
        get => _selectedCustomer;
        set
        {
            SetProperty(ref _selectedCustomer, value);
            PlaceNewOrderCommand.NotifyCanExecuteChanged();
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
            PlaceNewOrderCommand.NotifyCanExecuteChanged();
        }
    }

    private int _numberOfItems;
    public int NumberOfItems
    {
        get => _numberOfItems;
        set => SetProperty(ref _numberOfItems, value);
    }

    private OrderDTO _order;
    public OrderDTO Order
    {
        get => _order;
        set => SetProperty(ref _order, value);
    }
    
    public void Dispose()
    {
        //Dispose
    }
}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using CarSparePartService;
using CarSparePartService.Interfaces;
using CarSparePartService.Order;
using CarSparePartStore.Adapters;
using CarSparePartStore.ViewModels.DTO;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace CarSparePartStore.ViewModels;

public sealed class CarSparePartListViewModel
    : ObservableRecipient, IDisposable
{
    private readonly ICarSparePartService _carSparePartService;
    private readonly IProductsAndOrdersAdapter _productsAndOrdersAdapter;
    public event EventHandler<ProductSelectedEventArgs> ProductSelected;

    public CarSparePartListViewModel(ICarSparePartService carSparePartService, IProductsAndOrdersAdapter productsAndOrdersAdapter)
    {
        _carSparePartService = carSparePartService;
        _productsAndOrdersAdapter = productsAndOrdersAdapter;
        _carSparePartService.OrderAdded += CarSparePartServiceOrderAdded;
        ProductsWithItemsCount = new ObservableCollection<ProductWithItemsCount>();
        UpdateProductsWithOrders();
    }

    private void CarSparePartServiceOrderAdded(object? sender, OrderAddedEventArgs e)
    {
        Application.Current?.Dispatcher?.Invoke(() => { UpdateProductsWithOrders(); });
    }

    public void UpdateProductsWithOrders()
    {
        var productsWithOrders = GetProductsWithItemsCount();
        foreach (var productWithOrders in productsWithOrders)
        {
            var itemFromList = ProductsWithItemsCount.FirstOrDefault(p => p.ProductId == productWithOrders.ProductId);
            if (itemFromList is null)
            {
                itemFromList = productWithOrders;
                ProductsWithItemsCount.Add(productWithOrders);
            }

            itemFromList.ItemsCount = productWithOrders.ItemsCount;
        }
    }

    private IEnumerable<ProductWithItemsCount> GetProductsWithItemsCount()
    {
        return _productsAndOrdersAdapter.GetProductsWithItemsCount();
    }

    private ProductWithItemsCount _selectedProduct;

    public ProductWithItemsCount SelectedProduct
    {
        get => _selectedProduct;
        set
        {
            SetProperty(ref _selectedProduct, value);
            OnProductSelected();
        }
    }

    private void OnProductSelected()
    {
        var handler = ProductSelected;
        handler?.Invoke(this, new ProductSelectedEventArgs(SelectedProduct is null ? 0 : SelectedProduct.ProductId));
    }

    public ObservableCollection<ProductWithItemsCount> ProductsWithItemsCount { get; }

    public void Dispose()
    {
        _carSparePartService.OrderAdded -= CarSparePartServiceOrderAdded;
    }
}
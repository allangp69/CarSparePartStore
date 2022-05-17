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

public sealed  class CarSparePartListViewModel
    : ObservableRecipient, IDisposable
{
    private readonly ICarSparePartService _carSparePartService;

    public CarSparePartListViewModel(ICarSparePartService carSparePartService)
    {
        _carSparePartService = carSparePartService;
        _carSparePartService.OrderAdded += CarSparePartServiceOrderAdded;
        ProductsWithItemsCount = new ObservableCollection<ProductWithItemsCount>();
        UpdateProductsWithOrders();
    }
    
    private void CarSparePartServiceOrderAdded(object? sender, OrderAddedEventArgs e)
    {
        Application.Current?.Dispatcher?.Invoke(() =>
        {
            UpdateProductsWithOrders();
        });
    }

    public void UpdateProductsWithOrders()
    {
        var productsWithOrders = _carSparePartService.GetProductsWithItemsCount();
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

    private ProductWithItemsCount _selectedProduct;
    public ProductWithItemsCount SelectedProduct
    {
        get => _selectedProduct;
        set
        {
            SetProperty(ref _selectedProduct, value);
        }
    }
    
    public ObservableCollection<ProductWithItemsCount> ProductsWithItemsCount { get; private set; }
    

    public void Dispose()
    {
        //Dispose
    }
}
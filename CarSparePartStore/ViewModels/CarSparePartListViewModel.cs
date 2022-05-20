using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using CarSparePartService;
using CarSparePartService.Interfaces;
using CarSparePartService.Product;
using CarSparePartStore.Controller;
using CarSparePartStore.ViewModels.EventArgs;

namespace CarSparePartStore.ViewModels;

public sealed  class CarSparePartListViewModel
    : CarSparePartViewContent, IDisposable
{
    private readonly ICarSparePartService _carSparePartService;
    public event EventHandler<ProductSelectedEventArgs> ProductSelected;
    
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
            OnProductSelected();
        }
    }

    private void OnProductSelected()
    {
        var handler = ProductSelected;
        handler?.Invoke(this, new ProductSelectedEventArgs(SelectedProduct is null ? 0 : SelectedProduct.ProductId));
    }

    public ObservableCollection<ProductWithItemsCount> ProductsWithItemsCount { get; private set; }

    public void Dispose()
    {
        _carSparePartService.OrderAdded -= CarSparePartServiceOrderAdded;
    }
}
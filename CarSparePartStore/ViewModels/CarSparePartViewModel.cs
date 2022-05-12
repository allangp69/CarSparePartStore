using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows;
using CarSparePartService;
using CarSparePartService.Interfaces;

namespace CarSparePartStore.ViewModels;

public class CarSparePartViewModel
{
    private ICarSparePartService CarSparePartService { get; }

    public CarSparePartViewModel(ICarSparePartService carSparePartService)
    {
        ProductsWithOrders = new ObservableCollection<ProductWithOrders>();
        CarSparePartService = carSparePartService;
        var thread = new Thread(() =>
        {
            while (true)
            {
                Application.Current.Dispatcher.Invoke(() => UpdateProductsWithOrders());
                Thread.Sleep(TimeSpan.FromSeconds(15));
            }
        });
        thread.Start();
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

    public ObservableCollection<ProductWithOrders> ProductsWithOrders { get; private set; }

    public IEnumerable<ProductWithOrders> GetProductsWithOrders()
    {
        var retval = new List<ProductWithOrders>();
        var allOrders = CarSparePartService.GetAllOrders().ToList();
        var products = allOrders.SelectMany(o => o.OrderItems.Select(i => i.Product)).Distinct(new UniqueProductComparer()).ToList();
        foreach (var product in products)
        {
            retval.Add(new ProductWithOrders(product, GetNumberOfItemsSold(allOrders, product)));
        }
        return retval;
    }

    private int GetNumberOfItemsSold(List<Order> allOrders, Product product)
    {
        if (allOrders == null || allOrders.Count == 0)
            return 0;
        var retval = 0;
        foreach (var order in allOrders.Where(o => o.OrderItems.Any(i => i.Product.ProductId == product.ProductId)))
        {
            retval += order.OrderItems.Where(i => i.Product.ProductId == product.ProductId).Sum(item => item.NumberOfItems);
        }
        return retval;
    }
}

public class UniqueProductComparer 
    : IEqualityComparer<Product>
{
    public bool Equals(Product x, Product y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (ReferenceEquals(x, null)) return false;
        if (ReferenceEquals(y, null)) return false;
        if (x.GetType() != y.GetType()) return false;
        return x.ProductId == y.ProductId && x.Name == y.Name && x.Type == y.Type;
    }

    public int GetHashCode(Product obj)
    {
        return HashCode.Combine(obj.ProductId, obj.Name, obj.Type);
    }
}
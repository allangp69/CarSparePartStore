using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CarSparePartService;
using CarSparePartService.Interfaces;

namespace CarSparePartStore.ViewModels;

internal class CarSparePartViewModel
{
    private ICarSparePartService CarSparePartService { get; }

    public CarSparePartViewModel(ICarSparePartService carSparePartService)
    {
        CarSparePartService = carSparePartService;
    }

    public ObservableCollection<ProductWithOrders> ProductsWithOrders
    {
        get
        {
            return new ObservableCollection<ProductWithOrders>(GetProductsWithOrders());
        }
    }

    public IEnumerable<ProductWithOrders> GetProductsWithOrders()
    {
        var retval = new List<ProductWithOrders>();
        var allOrders = CarSparePartService.GetAllOrders().ToList();
        var products = allOrders.SelectMany(o => o.OrderItems.Select(i => i.Product)).ToList();
        foreach (var product in products)
        {
            retval.Add(new ProductWithOrders(product, GetNumberOfItemsSold(allOrders, product)));
        }
        return retval;
    }

    private int GetNumberOfItemsSold(List<Order> allOrders, Product product)
    {
        var retval = 0;
        foreach (var order in allOrders.Where(o => o.OrderItems.Any(i => i.Product.ProductId == product.ProductId)))
        {
            foreach (var item in order.OrderItems.Where(i => i.Product.ProductId == product.ProductId))
            {
                retval += item.NumberOfItems;
            }
        }

        return retval;
    }
}
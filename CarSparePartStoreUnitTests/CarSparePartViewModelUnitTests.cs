using System.Collections.Generic;
using System.Linq;
using CarSparePartService;
using CarSparePartService.Interfaces;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using NUnit.Framework;
using CarSparePartStore.ViewModels;

namespace CarSparePartStoreUnitTests;

public class Tests
{
    private CarSparePartViewModel viewModel;

    [SetUp]
    public void Setup()
    {
        ConfigureServices();
        var carSparepartService = Ioc.Default.GetRequiredService<ICarSparePartService>();
        carSparepartService.PlaceOrder(Order.Create(0, new List<OrderItem>
            {
                new OrderItem{NumberOfItems = 3, Product = new Product{ProductId = 1, Name = "Dummy 1", Description = "Dummy Product 1", Price = 1234.56m, Type = "Car SparePart"}},
                new OrderItem{NumberOfItems = 2, Product = new Product{ProductId = 2, Name = "Dummy 2", Description = "Dummy Product 2", Price = 134.56m, Type = "Bike SparePart"}},
                new OrderItem{NumberOfItems = 5, Product = new Product{ProductId = 1, Name = "Dummy 1", Description = "Dummy Product 1", Price = 1234.56m, Type = "Car SparePart"}}
            }));
        carSparepartService.PlaceOrder(Order.Create(1, new List<OrderItem>
            {
                new OrderItem{NumberOfItems = 3, Product = new Product{ProductId = 1, Name = "Dummy 1", Description = "Dummy Product 1", Price = 1234.56m, Type = "Car SparePart"}},
                new OrderItem{NumberOfItems = 2, Product = new Product{ProductId = 2, Name = "Dummy 2", Description = "Dummy Product 2", Price = 134.56m, Type = "Bike SparePart"}},
                new OrderItem{NumberOfItems = 5, Product = new Product{ProductId = 1, Name = "Dummy 1", Description = "Dummy Product 1", Price = 1234.56m, Type = "Car SparePart"}}
            }));
        viewModel = new CarSparePartViewModel(carSparepartService);
    }

    [Test]
    public void TestGetProductsWithOrders()
    {
        var result = viewModel.GetProductsWithOrders();
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Count() > 1);
    }
    
    /// <summary>
    /// Configures the services for the application.
    /// </summary>
    private static void ConfigureServices()
    {
        TestServicesConfigurator.TestServicesConfigurator.ConfigureServices();
    }
}
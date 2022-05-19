using System.Collections.Generic;
using System.Linq;
using CarSparePartService;
using CarSparePartService.Interfaces;
using CarSparePartService.Product;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using NUnit.Framework;

namespace CarSparePartServiceUnitTests;

public class GetOrdersForProductUnitTest
{
    [SetUp]
    public void Setup()
    {
        ConfigureServices();
    }

    [Test]
    public void TestGetOrdersForProduct()
    {
        var carSparepartService = Ioc.Default.GetRequiredService<ICarSparePartService>();
        var productFetcher = Ioc.Default.GetRequiredService<IProductFetcher>();
        var product = productFetcher.GetAllProducts().First();
        carSparepartService.PlaceOrder(new Order(0, new List<OrderItem>{ new OrderItem{NumberOfItems = 1, Product = product}}));
        var orders = carSparepartService.GetOrdersForProduct(product);
        Assert.IsTrue(orders.Any());
    }
    
    /// <summary>
    /// Configures the services for the application.
    /// </summary>
    private static void ConfigureServices()
    {
        TestServicesConfigurator.TestServicesConfigurator.ConfigureServices();
    }
}
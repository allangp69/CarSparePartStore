using System.Collections.Generic;
using System.Linq;
using CarSparePartService;
using CarSparePartService.Interfaces;
using CarSparePartService.Product;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using NUnit.Framework;

namespace CarSparePartServiceUnitTests;

public class GetProductWithItemsCountUnitTest
{
    [SetUp]
    public void Setup()
    {
        ConfigureServices();
    }

    [Test]
    public void TestGetProductItemsCount()
    {
        var carSparepartService = Ioc.Default.GetRequiredService<ICarSparePartService>();
        var numberOfItems = 4;
        var productFetcher = Ioc.Default.GetRequiredService<IProductFetcher>();
        var product = productFetcher.GetAllProducts().First();
        carSparepartService.PlaceOrder(new Order(0, new List<OrderItem>{ new OrderItem{NumberOfItems = numberOfItems, Product = product}}));
        var itemsCount = carSparepartService.GetNumberOfItemsSoldForProduct(product);
        Assert.IsTrue(itemsCount == numberOfItems);
    }
    
    /// <summary>
    /// Configures the services for the application.
    /// </summary>
    private static void ConfigureServices()
    {
        TestServicesConfigurator.TestServicesConfigurator.ConfigureServices();
    }
}
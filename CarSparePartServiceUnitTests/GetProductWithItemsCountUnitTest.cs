using System.Collections.Generic;
using System.Linq;
using CarSparePartData.Interfaces;
using CarSparePartService;
using CarSparePartService.Interfaces;
using CarSparePartService.Order;
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
        var product = new Product{ProductId = 12345, Name = "Test product", Description = "Test test test test test test", Price = 1234.67m, Type = "Product type"};
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
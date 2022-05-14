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
    public void TestGetProductWithItemsCount()
    {
        var carSparepartService = Ioc.Default.GetRequiredService<ICarSparePartService>();
        var numberOfItems = 4;
        var productFetcher = Ioc.Default.GetRequiredService<IProductFetcher>();
        var product = productFetcher.GetAllProducts().First();
        carSparepartService.PlaceOrder(Order.Create(0, new List<OrderItem>{ new OrderItem{NumberOfItems = numberOfItems, Product = product}}));
        var productWithItemsCount = carSparepartService.GetProductsWithItemsCount().FirstOrDefault(p => p.ProductId == product.ProductId);
        Assert.IsTrue(productWithItemsCount.ItemsCount == numberOfItems);
    }
    
    /// <summary>
    /// Configures the services for the application.
    /// </summary>
    private static void ConfigureServices()
    {
        TestServicesConfigurator.TestServicesConfigurator.ConfigureServices();
    }
}
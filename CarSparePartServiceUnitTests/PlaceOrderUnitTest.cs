using System.Collections.Generic;
using System.Linq;
using CarSparePartService;
using CarSparePartService.Interfaces;
using CarSparePartService.Product;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using NUnit.Framework;

namespace CarSparePartServiceUnitTests;

public class PlaceOrderUnitTest
{
    [SetUp]
    public void Setup()
    {
        ConfigureServices();
    }

    [Test]
    public void TestGetAllOrders()
    {
        var carSparepartService = Ioc.Default.GetRequiredService<ICarSparePartService>();
        carSparepartService.PlaceOrder(new Order(0, new List<OrderItem>{ new OrderItem{NumberOfItems = 1, Product = new Product{ProductId = 1, 
                                                                            Name = "Test Product", Description = "Test test test", Type = "Test type", Price = 1234.56m}}}));
        var allOrders = carSparepartService.GetAllOrders();
        Assert.IsTrue(allOrders.Any());
    }
    
    /// <summary>
    /// Configures the services for the application.
    /// </summary>
    private static void ConfigureServices()
    {
        TestServicesConfigurator.TestServicesConfigurator.ConfigureServices();
    }
}
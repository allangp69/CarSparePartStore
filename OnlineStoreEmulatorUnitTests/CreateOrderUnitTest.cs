using System.Linq;
using CarSparePartService;
using CarSparePartService.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using NUnit.Framework;
using TestConfiguration;

namespace OnlineStoreEmulatorUnitTests;

public class CreateOrderUnitTests
{
    private global::OnlineStoreEmulator.OnlineStoreEmulator _emulator;

    [OneTimeSetUp]
    public void Setup()
    {
        ConfigureServices();
        var customerService = Ioc.Default.GetRequiredService<ICustomerService>();
        var carSparepartService = Ioc.Default.GetRequiredService<ICarSparePartService>();
        var productFetcher = Ioc.Default.GetRequiredService<IProductFetcher>();
        _emulator = new global::OnlineStoreEmulator.OnlineStoreEmulator(carSparepartService, customerService, productFetcher);
    }

    [Test]
    public void TestCreateOrder()
    {
        var carSparepartService = Ioc.Default.GetRequiredService<ICarSparePartService>();
        var numberOfOrdersBefore = carSparepartService.GetAllOrders().Count();
        _emulator.CreateOrder();
        var numberOfOrdersAfter = carSparepartService.GetAllOrders().Count();
        Assert.IsTrue(numberOfOrdersAfter == numberOfOrdersBefore + 1); 
    }
    
    /// <summary>
    /// Configures the services for the application.
    /// </summary>
    private static void ConfigureServices()
    {
        SetupTestServices.ConfigureServices();
    }
}
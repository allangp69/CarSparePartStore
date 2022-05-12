using System.Linq;
using CarSparePartService.Interfaces;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using NUnit.Framework;
using OnlineStoreEmulator;

namespace OnlineStoreEmulatorUnitTests;

public class CreateOrderUnitTests
{
    private IOnlineStoreEmulator _emulator;

    [OneTimeSetUp]
    public void Setup()
    {
        ConfigureServices();
        _emulator = Ioc.Default.GetRequiredService<IOnlineStoreEmulator>();
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
        TestServicesConfigurator.TestServicesConfigurator.ConfigureServices();
    }
}
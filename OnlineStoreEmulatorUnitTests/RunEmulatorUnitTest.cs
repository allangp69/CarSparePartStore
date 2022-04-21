using System;
using System.Linq;
using System.Threading;
using CarSparePartService.Interfaces;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using NUnit.Framework;

namespace OnlineStoreEmulatorUnitTests;

public class RunEmulatorUnitTests
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
    public void TestRunEmulator()
    {
        var carSparepartService = Ioc.Default.GetRequiredService<ICarSparePartService>();
        var numberOfOrdersBefore = carSparepartService.GetAllOrders().Count();
        _emulator.Start();
        Thread.Sleep(TimeSpan.FromSeconds(30));
        _emulator.Stop();
        var numberOfOrdersAfter = carSparepartService.GetAllOrders().Count();
        Assert.IsTrue(numberOfOrdersAfter > numberOfOrdersBefore); 
    }
    
    /// <summary>
    /// Configures the services for the application.
    /// </summary>
    private static void ConfigureServices()
    {
        SetupTestServices.ConfigureServices();
    }
}
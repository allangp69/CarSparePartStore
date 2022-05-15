using System;
using System.Linq;
using System.Threading;
using CarSparePartService.Interfaces;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using NUnit.Framework;
using OnlineStoreEmulator;

namespace OnlineStoreEmulatorUnitTests;

public class RunEmulatorUnitTests
{
    private IOnlineStoreEmulator _emulator;

    [OneTimeSetUp]
    public void Setup()
    {
        ConfigureServices();
        _emulator = Ioc.Default.GetRequiredService<IOnlineStoreEmulator>();
    }

    [Test]
    public void TestRunEmulator()
    {
        var carSparepartService = Ioc.Default.GetRequiredService<ICarSparePartService>();
        var numberOfOrdersBefore = carSparepartService.GetAllOrders().Count();
        _emulator.Start();
        Thread.Sleep(TimeSpan.FromSeconds(3));
        _emulator.Stop();
        var numberOfOrdersAfter = carSparepartService.GetAllOrders().Count();
        Assert.IsTrue(numberOfOrdersAfter > numberOfOrdersBefore); 
    }
    
    /// <summary>
    /// Configures the services for the application.
    /// </summary>
    private static void ConfigureServices()
    {
        TestServicesConfigurator.TestServicesConfigurator.ConfigureServices();
    }
}
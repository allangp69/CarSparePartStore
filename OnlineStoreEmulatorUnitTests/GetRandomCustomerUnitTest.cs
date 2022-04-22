using CarSparePartService.Interfaces;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using NUnit.Framework;
using TestConfiguration;

namespace OnlineStoreEmulatorUnitTests;

public class GetRandomCustomerUnitTests
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
    public void TestGetRandomCustomer()
    {
        var customer = _emulator.GetRandomCustomer();
        Assert.IsNotNull(customer);
        Assert.IsFalse(string.IsNullOrEmpty(customer.FirstName));
        Assert.IsFalse(string.IsNullOrEmpty(customer.LastName));
        Assert.IsTrue(customer.CustomerId > 0);
    }
    
    /// <summary>
    /// Configures the services for the application.
    /// </summary>
    private static void ConfigureServices()
    {
        SetupTestServices.ConfigureServices();
    }
}
using CarSparePartService.Interfaces;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using NUnit.Framework;
using TestConfiguration;

namespace OnlineStoreEmulatorUnitTests;

public class GetRandomProductUnitTests
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
    public void TestGetRandomProduct()
    {
        var product = _emulator.GetRandomProduct();
        Assert.IsNotNull(product);
        Assert.IsFalse(string.IsNullOrEmpty(product.Name));
        Assert.IsTrue(product.ProductId > 0);
        Assert.IsTrue(product.Price > 0);
    }
    
    /// <summary>
    /// Configures the services for the application.
    /// </summary>
    private static void ConfigureServices()
    {
        SetupTestServices.ConfigureServices();
    }
}
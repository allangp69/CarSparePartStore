using Microsoft.Toolkit.Mvvm.DependencyInjection;
using NUnit.Framework;
using OnlineStoreEmulator;

namespace OnlineStoreEmulatorUnitTests;

public class GetRandomProductUnitTests
{
    private IRandomProductGenerator _generator;

    [OneTimeSetUp]
    public void Setup()
    {
        ConfigureServices();
        _generator = Ioc.Default.GetRequiredService<IRandomProductGenerator>();
    }

    [Test]
    public void TestGetRandomProduct()
    {
        var product = _generator.GenerateProduct();
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
        TestServicesConfigurator.TestServicesConfigurator.ConfigureServices();
    }
}
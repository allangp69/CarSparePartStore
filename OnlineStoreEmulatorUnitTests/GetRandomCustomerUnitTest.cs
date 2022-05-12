using Microsoft.Toolkit.Mvvm.DependencyInjection;
using NUnit.Framework;
using OnlineStoreEmulator;

namespace OnlineStoreEmulatorUnitTests;

public class GetRandomCustomerUnitTests
{
    private IRandomCustomerGenerator _generator;

    [OneTimeSetUp]
    public void Setup()
    {
        ConfigureServices();
        ConfigureServices();
        _generator = Ioc.Default.GetRequiredService<IRandomCustomerGenerator>();
    }

    [Test]
    public void TestGetRandomCustomer()
    {
        var customer = _generator.GenerateCustomer();
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
        TestServicesConfigurator.TestServicesConfigurator.ConfigureServices();
    }
}
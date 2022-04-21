using System.Linq;
using CarSparePartService;
using CarSparePartService.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using NUnit.Framework;

namespace CustomerServiceUnitTests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
        ConfigureServices();
    }

    [Test]
    public void TestGetAllCustomers()
    {
        var service = Ioc.Default.GetRequiredService<ICustomerService>();
        var allCustomers = service.GetAllCustomers();
        Assert.IsTrue(allCustomers.Count() > 0);
    }
    
    // <summary>
    /// Configures the services for the application.
    /// </summary>
    private static void ConfigureServices()
    {
        Ioc.Default.ConfigureServices(
            new ServiceCollection()
                .AddSingleton<ICustomerService, CustomerService>()
                .BuildServiceProvider());
    }
}
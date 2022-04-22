using CarSparePartService;
using CarSparePartService.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.DependencyInjection;

namespace TestConfiguration;

public class SetupTestServices
{
    private static object _configureServicesLockObject = new object();
    private static bool HaveServicesBeenSetup;

    public static void ConfigureServices()
    {
        lock (_configureServicesLockObject)
        {
            if (HaveServicesBeenSetup)
            {
                return;
            }

            Ioc.Default.ConfigureServices(
                new ServiceCollection()
                    .AddSingleton<ICustomerService, CustomerService>()
                    .AddSingleton<ICarSparePartService, CarSparePartService.CarSparePartService>()
                    .AddSingleton<IProductFetcher, ProductFetcher>()
                    .BuildServiceProvider());
            var file = new FileInfo(@".\Resources\SpareParts.xml");
            var productFetcher = Ioc.Default.GetRequiredService<IProductFetcher>();
            productFetcher.LoadProducts(file.FullName);
            HaveServicesBeenSetup = true;
        }
    }
}
using CarSparePartService;
using CarSparePartService.Backup;
using CarSparePartService.Interfaces;
using CarSparePartService.Product;
using CarSparePartStore.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using OnlineStoreEmulator;
using Serilog;

namespace TestServicesConfigurator;

public class TestServicesConfigurator
{
    private static object _configureServicesLockObject = new object();
    private static bool HaveServicesBeenSetup;

    private static IConfigurationRoot ReadConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddEnvironmentVariables();

        return builder.Build();
    }
    
    public static void ConfigureServices()
    {
        lock (_configureServicesLockObject)
        {
            if (HaveServicesBeenSetup)
            {
                return;
            }

            var configuration = ReadConfiguration();

            Ioc.Default.ConfigureServices(
                new ServiceCollection()
                    .AddSingleton<IConfiguration>(configuration)
                    .AddSingleton<ICustomerService, CustomerService>()
                    .AddSingleton<IRandomCustomerGenerator, RandomCustomerGenerator>()
                    .AddSingleton<ICarSparePartService, CarSparePartService.CarSparePartService>()
                    .AddSingleton<IProductFetcher, ProductFetcher>()
                    .AddSingleton<IRandomProductGenerator, RandomProductGenerator>()
                    .AddSingleton<IOrderBackupWriter, XmlOrderBackupWriter>()
                    .AddSingleton<IOrderBackupReader, XmlOrderBackupReader>()
                    .AddSingleton<IOrderBackupManager, OrderBackupManager>()
                    .AddSingleton<IOnlineStoreEmulator, OnlineStoreEmulator.OnlineStoreEmulator>()
                    .AddSingleton((ILogger)new LoggerConfiguration()
                        .MinimumLevel.Information()
                        .WriteTo.Console()
                        .CreateLogger())
                    .AddTransient<CarSparePartViewModel>()
                    .BuildServiceProvider());
            var file = new FileInfo(@".\Resources\SpareParts.xml");
            var productFetcher = Ioc.Default.GetRequiredService<IProductFetcher>();
            productFetcher.LoadProducts(file.FullName);
            HaveServicesBeenSetup = true;
        }
    }
}
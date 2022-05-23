using CarSparePartData;
using CarSparePartData.Customer;
using CarSparePartData.Interfaces;
using CarSparePartData.Product;
using CarSparePartService;
using CarSparePartService.Adapters;
using CarSparePartService.Backup;
using CarSparePartService.Customer;
using CarSparePartService.Interfaces;
using CarSparePartService.Order;
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

            var logger = (ILogger) new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .CreateLogger();
            Ioc.Default.ConfigureServices(
                new ServiceCollection()
                    .AddSingleton<IConfiguration>(configuration)
                    //Customers
                    .AddSingleton<ICustomerService, CustomerService>()
                    .AddSingleton<IRandomCustomerGenerator, RandomCustomerGenerator>()
                    .AddSingleton<CustomerDataAdapter>()
                    .AddSingleton<CarSparePartService.Customer.CustomerDTOConverter>()
                    .AddSingleton<ICustomerRepository, CustomerRepository>()
                    //Products
                    .AddSingleton<IProductService, ProductService>()
                    .AddSingleton<IRandomProductGenerator, RandomProductGenerator>()
                    .AddSingleton<ProductDataAdapter>()
                    .AddSingleton<ProductDTOConverter>()
                    .AddSingleton<IProductRepository, ProductRepository>()
                    .AddSingleton(new ProductRepositoryConfig{BackupFilePath = new FileInfo(@".\Resources\SpareParts.xml").FullName})
                    //Orders
                    .AddSingleton<OrderDTOConverter>()
                    //CarSparePartService
                    .AddSingleton<ICarSparePartService, CarSparePartService.CarSparePartService>()
                    //OnlineStoreEmulator
                    .AddSingleton<IOnlineStoreEmulator, OnlineStoreEmulator.OnlineStoreEmulator>()
                    //Backup
                    .AddSingleton<IOrderBackupManager, OrderBackupManager>()
                    .AddSingleton((IOrderBackupWriter)new XmlOrderBackupWriter(configuration.GetSection("ApplicationSettings").GetSection("OrdersBackup").Value, logger))
                    .AddSingleton((IOrderBackupReader)new XmlOrderBackupReader(configuration.GetSection("ApplicationSettings").GetSection("OrdersBackup").Value, logger))
                    //CarSparePartStore/ViewModels
                    //.AddTransient<CarSparePartViewModel>()
                    //Logger
                    .AddSingleton(logger)
                    .BuildServiceProvider());
            var productRepository = Ioc.Default.GetRequiredService<IProductRepository>();
            productRepository.LoadProductsFromBackup();
            HaveServicesBeenSetup = true;
        }
    }
}
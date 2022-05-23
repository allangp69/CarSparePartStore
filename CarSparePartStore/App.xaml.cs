using System.IO;
using System.Windows;
using CarSparePartData.Customer;
using CarSparePartData.Interfaces;
using CarSparePartData.Product;
using CarSparePartService;
using CarSparePartService.Adapters;
using CarSparePartService.Backup;
using CarSparePartService.Interfaces;
using CarSparePartService.Product;
using CarSparePartStore.Adapters;
using CarSparePartStore.ViewModels;
using CarSparePartStore.ViewModels.Notification;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using OnlineStoreEmulator;
using Serilog;
using TestServicesConfigurator;
using OrderDTOConverter = CarSparePartStore.ViewModels.DTO.OrderDTOConverter;

namespace CarSparePartStore
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            var configuration = ReadConfiguration();
            ConfigureServices(configuration);
            var productService = Ioc.Default.GetRequiredService<IProductService>();
            productService.LoadProductsFromBackup();

            LoadBackup();
            
            this.InitializeComponent();
        }

        private void LoadBackup()
        {
            var carSparePartService = Ioc.Default.GetRequiredService<ICarSparePartService>();
            carSparePartService?.RestoreBackup();
        }

        private IConfiguration ReadConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

             return builder.Build();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            var emulator = Ioc.Default.GetRequiredService<IOnlineStoreEmulator>();
            emulator.Stop();
            CreateBackup();
            base.OnExit(e);
        }
        
        private void CreateBackup()
        {
            var carSparePartService = Ioc.Default.GetRequiredService<ICarSparePartService>();
            carSparePartService.CreateBackup();
        }
        
        // <summary>
        /// Configures the services for the application.
        /// </summary>
        /// <param name="ConfigureServices"></param>
        private void ConfigureServices(IConfiguration configuration)
        {
            var logger = (ILogger) new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .WriteTo.File(configuration.GetSection("Logging").GetValue<string>("LogFilePath"))
                .CreateLogger();
            Ioc.Default.ConfigureServices(
                new ServiceCollection()
                    .AddSingleton(configuration)
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
                    .AddSingleton(new ProductRepositoryConfig{BackupFilePath = configuration.GetSection("ApplicationSettings").GetSection("ProductsBackup").Value})
                    //Orders
                    .AddSingleton<OrderDTOConverter>()
                    .AddSingleton<IProductsAndOrdersAdapter, ProductsAndOrdersAdapter>()
                    //CarSparePartService
                    .AddSingleton<ICarSparePartService, CarSparePartService.CarSparePartService>()
                    //OnlineStoreEmulator
                    .AddSingleton<IOnlineStoreEmulator, OnlineStoreEmulator.OnlineStoreEmulator>()
                    //Backup
                    .AddSingleton<IOrderBackupManager, OrderBackupManager>()
                    .AddSingleton((IOrderBackupWriter)new XmlOrderBackupWriter(configuration.GetSection("ApplicationSettings").GetSection("OrdersBackup").Value, logger))
                    .AddSingleton((IOrderBackupReader)new XmlOrderBackupReader(configuration.GetSection("ApplicationSettings").GetSection("OrdersBackup").Value, logger))
                    //NotificationHandler
                    .AddSingleton<NotificationHandler>()
                    //CarSparePartStore/ViewModels
                    .AddSingleton<ViewModels.DTO.CustomerDTOConverter>()
                    .AddSingleton<ICustomerAdapter, CustomerAdapter>()
                    .AddTransient<CarSparePartViewModel>()
                    .AddTransient<CarSparePartListViewModel>()
                    .AddTransient<CarSparePartNewOrderViewModel>()
                    .AddTransient<OrdersForProductViewModel>()
                    //Logger
                    .AddSingleton(logger)
                    .BuildServiceProvider());
        }
    }
}
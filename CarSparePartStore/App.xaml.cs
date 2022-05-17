using System.Configuration;
using System.IO;
using System.Windows;
using CarSparePartService;
using CarSparePartService.Backup;
using CarSparePartService.Interfaces;
using CarSparePartService.Product;
using CarSparePartStore.ViewModels;
using CarSparePartStore.ViewModels.Notification;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using OnlineStoreEmulator;
using Serilog;
using TestServicesConfigurator;

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
            var productFetcher = Ioc.Default.GetRequiredService<IProductFetcher>();
            productFetcher.LoadProductsFromBackup();

            LoadBackup(configuration);
            
            this.InitializeComponent();
        }

        private void LoadBackup(IConfiguration configuration)
        {
            var carSparePartService = Ioc.Default.GetRequiredService<ICarSparePartService>();
            var backupFilename = configuration.GetSection("ApplicationSettings").GetSection("OrdersBackup").Value;
            if (File.Exists(backupFilename))
            {
                carSparePartService.LoadBackup(backupFilename);
            }
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
            var configuration = Ioc.Default.GetRequiredService<IConfiguration>();
            var carSparePartService = Ioc.Default.GetRequiredService<ICarSparePartService>();
            var backupFilename = configuration.GetSection("ApplicationSettings").GetSection("OrdersBackup").Value;
            carSparePartService.CreateBackup(backupFilename);
        }
        
        // <summary>
        /// Configures the services for the application.
        /// </summary>
        /// <param name="readConfiguration"></param>
        private void ConfigureServices(IConfiguration configuration)
        {
            Ioc.Default.ConfigureServices(
                new ServiceCollection()
                    .AddSingleton<IConfiguration>(configuration)
                    .AddSingleton<ICustomerService, CustomerService>()
                    .AddSingleton<IRandomCustomerGenerator, RandomCustomerGenerator>()
                    .AddSingleton<ICarSparePartService, CarSparePartService.CarSparePartService>()
                    .AddSingleton<IProductFetcher, ProductFetcher>()
                    .AddSingleton<IRandomProductGenerator, RandomProductGenerator>()
                    .AddSingleton<IOnlineStoreEmulator, OnlineStoreEmulator.OnlineStoreEmulator>()
                    .AddSingleton<IOrderBackupManager, OrderBackupManager>()
                    .AddSingleton<IOrderBackupWriter, XmlOrderBackupWriter>()
                    .AddSingleton<IOrderBackupReader, XmlOrderBackupReader>()
                    .AddSingleton<NotificationHandler>()
                    .AddSingleton((ILogger)new LoggerConfiguration()
                        .MinimumLevel.Information()
                        .WriteTo.Console()
                        .WriteTo.File(configuration.GetSection("Logging").GetValue<string>("LogFilePath"))
                        .CreateLogger())
                    .AddTransient<CarSparePartViewModel>()
                    .AddTransient<CarSparePartListViewModel>()
                    .AddTransient<CarSparePartNewOrderViewModel>()
                    .AddTransient<OrdersForProductViewModel>()
                    .BuildServiceProvider());
        }
    }
}
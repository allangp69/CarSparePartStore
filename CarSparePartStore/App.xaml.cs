using System.Configuration;
using System.IO;
using System.Windows;
using CarSparePartService;
using CarSparePartService.Interfaces;
using CarSparePartStore.ViewModels;
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
        private readonly IOnlineStoreEmulator _emulator;
        private static IConfigurationRoot Configuration { get; set; }
        
        public App()
        {
            ReadConfiguration();
            ConfigureServices();
            var productFetcher = Ioc.Default.GetRequiredService<IProductFetcher>();
            productFetcher.LoadProductsFromBackup();
            _emulator = Ioc.Default.GetRequiredService<IOnlineStoreEmulator>();
            this.InitializeComponent();
        }

        private void ReadConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

             Configuration = builder.Build();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _emulator.Stop();
            base.OnExit(e);
        }
        
        // <summary>
        /// Configures the services for the application.
        /// </summary>
        private static void ConfigureServices()
        {
            Ioc.Default.ConfigureServices(
                new ServiceCollection()
                    .AddSingleton<IConfiguration>(Configuration)
                    .AddSingleton<ICustomerService, CustomerService>()
                    .AddSingleton<IRandomCustomerGenerator, RandomCustomerGenerator>()
                    .AddSingleton<ICarSparePartService, CarSparePartService.CarSparePartService>()
                    .AddSingleton<IProductFetcher, ProductFetcher>()
                    .AddSingleton<IRandomProductGenerator, RandomProductGenerator>()
                    .AddSingleton<IOnlineStoreEmulator, OnlineStoreEmulator.OnlineStoreEmulator>()
                    .AddSingleton<IOrderBackupManager, OrderBackupManager>()
                    .AddSingleton<IOrderBackupWriter, XmlOrderBackupWriter>()
                    .AddSingleton<IOrderBackupReader, XmlOrderBackupReader>()
                    .AddSingleton((ILogger)new LoggerConfiguration()
                        .MinimumLevel.Information()
                        .WriteTo.Console()
                        .WriteTo.File(Configuration.GetSection("Logging").GetValue<string>("LogFilePath"))
                        .CreateLogger())
                    .AddTransient<CarSparePartViewModel>()
                    .BuildServiceProvider());
        }
    }
}
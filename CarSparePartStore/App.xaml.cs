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
            _emulator.Start();
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
                    .AddSingleton<ICustomerService, CustomerService>()
                    .AddSingleton<ICarSparePartService, CarSparePartService.CarSparePartService>()
                    .AddSingleton<IProductFetcher, ProductFetcher>()
                    .AddSingleton<IOnlineStoreEmulator, OnlineStoreEmulator.OnlineStoreEmulator>()
                    .AddSingleton<IConfiguration>(Configuration)
                    .AddSingleton<IOrderBackupManager, OrderBackupManager>()
                    .AddSingleton<IOrderBackupWriter, XmlOrderBackupWriter>()
                    .AddSingleton<IOrderBackupReader, XmlOrderBackupReader>()
                    .AddTransient<CarSparePartViewModel>()
                    .BuildServiceProvider());
        }
    }
}
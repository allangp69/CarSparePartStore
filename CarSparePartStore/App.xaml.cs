using System.IO;
using System.Windows;
using CarSparePartService;
using CarSparePartService.Interfaces;
using CarSparePartStore.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.DependencyInjection;

namespace CarSparePartStore
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            ConfigureServices();
            this.InitializeComponent();
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
                    .AddTransient<CarSparePartViewModel>()
                    .BuildServiceProvider());
        }
    }
}
using System.Windows;
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
                    .AddSingleton<ICarSparePartService, CarSparePartService.CarSparePartService>()
                    .AddTransient<CarSparePartViewModel>()
                    .BuildServiceProvider());
        }
    }
}
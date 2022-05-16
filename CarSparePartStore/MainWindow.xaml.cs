using System.ComponentModel;
using System.Windows;
using CarSparePartStore.ViewModels;
using Microsoft.Toolkit.Mvvm.DependencyInjection;

namespace CarSparePartStore
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var vm = Ioc.Default.GetService<CarSparePartViewModel>();
            this.DataContext = vm;
            vm.IsActive = true;
        }

        private void MainWindow_OnClosing(object? sender, CancelEventArgs e)
        {
            var vm = this.DataContext as CarSparePartViewModel;
            if (vm is null)
            {
                return;
            }
            vm.Dispose();
        }
    }
}
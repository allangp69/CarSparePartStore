using CarSparePartService;
using CarSparePartService.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Toolkit.Mvvm.DependencyInjection;

namespace OnlineStoreEmulator;

public class OnlineStoreEmulator
    : IOnlineStoreEmulator
{
    private static readonly Random _random = new Random();
    private ICarSparePartService _carSparePartService { get; }
    private ICustomerService _customerService { get; }
    private IProductFetcher _productFetcher { get; }
    private readonly IRandomCustomerGenerator _randomCustomerGenerator;
    private readonly IRandomProductGenerator _randomProductGenerator;
    private int _intervalSeconds = 15;
    CancellationTokenSource cts = new CancellationTokenSource();
        
    public OnlineStoreEmulator(ICarSparePartService carSparePartService, ICustomerService customerService, IRandomCustomerGenerator randomCustomerGenerator, IProductFetcher productFetcher, IRandomProductGenerator randomProductGenerator)
    {
        _randomCustomerGenerator = randomCustomerGenerator;
        _randomProductGenerator = randomProductGenerator;
        _carSparePartService = carSparePartService;
        _customerService = customerService;
        _productFetcher = productFetcher;
        SetOrderIntervalFromConfiguration();
    }

    private void SetOrderIntervalFromConfiguration()
    {
        var configuration = Ioc.Default.GetRequiredService<IConfiguration>();
        var interval = configuration.GetSection("OnlineStorEmulator").GetSection("CreateOrdersIntervalSeconds").Value;
        if (interval is null) 
            return;
        if (!int.TryParse(interval, out var intervalSeconds)) 
            return;
        if (intervalSeconds > 0)
        {
            _intervalSeconds = intervalSeconds;    
        }
    }

    public void Start()
    {
        var onlineSaleThread = new Thread(() =>
        {
            while (!cts.IsCancellationRequested)
            {
                Console.WriteLine("Creating order");
                CreateOrder();
                Thread.Sleep(TimeSpan.FromSeconds(_intervalSeconds));
            }
        });
        onlineSaleThread.Start();
    }
    
    public void Stop()
    {
        cts.Cancel();
    }
    
    public void CreateOrder()
    {
        var customer = _randomCustomerGenerator.GenerateCustomer();
        var product = _randomProductGenerator.GenerateProduct();
        var orderItems = new List<OrderItem>{new OrderItem{Product = product, NumberOfItems = _random.Next(1, 11)}};
        _carSparePartService.PlaceOrder(Order.Create(customer.CustomerId, orderItems));
    }
}
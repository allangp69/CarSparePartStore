using CarSparePartService;
using CarSparePartService.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Toolkit.Mvvm.DependencyInjection;

namespace OnlineStoreEmulator;

public class OnlineStoreEmulator
    : IOnlineStoreEmulator
{
    private static readonly Random Random = new Random();
    private readonly ICarSparePartService _carSparePartService;
    private readonly IRandomCustomerGenerator _randomCustomerGenerator;
    private readonly IRandomProductGenerator _randomProductGenerator;
    private int _intervalSeconds = 15;
    private CancellationTokenSource _cts;
    
    private bool _isRunning;

    private bool IsRunning
    {
        get => _isRunning;
        set
        {
            _isRunning = value;
            OnIsRunningChanged(_isRunning);
        }
    }

    private void OnIsRunningChanged(bool isRunning)
    {
        var handler = IsRunningChanged;
        handler?.Invoke(this,  new IsRunningEventArgs(isRunning));
    }

    public OnlineStoreEmulator(ICarSparePartService carSparePartService, IRandomCustomerGenerator randomCustomerGenerator, IRandomProductGenerator randomProductGenerator)
    {
        _randomCustomerGenerator = randomCustomerGenerator;
        _randomProductGenerator = randomProductGenerator;
        _carSparePartService = carSparePartService;
        SetOrderIntervalFromConfiguration();
    }

    public event EventHandler<IsRunningEventArgs>? IsRunningChanged;
    
    private void SetOrderIntervalFromConfiguration()
    {
        var configuration = Ioc.Default.GetRequiredService<IConfiguration>();
        var interval = configuration.GetSection("OnlineStorEmulator").GetSection("CreateOrdersIntervalSeconds").Value;
        // _intervalSeconds = int.TryParse(interval, out var intervalseconds) && intervalseconds > 0
        //     ? intervalseconds
        //     : _intervalSeconds;
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
        _cts = new CancellationTokenSource();
        var onlineSaleThread = new Thread(() =>
        {
            while (!_cts.IsCancellationRequested)
            {
                Console.WriteLine("Creating order");
                CreateOrder();
                //Thread.Sleep(TimeSpan.FromSeconds(_intervalSeconds));
                _cts.Token.WaitHandle.WaitOne(TimeSpan.FromSeconds(_intervalSeconds));
            }
        });
        onlineSaleThread.Start();
        IsRunning = true;
    }

    public void Stop()
    {
        _cts.Cancel();
        IsRunning = false;
    }

    public void CreateOrder()
    {
        var customer = _randomCustomerGenerator.GenerateCustomer();
        var product = _randomProductGenerator.GenerateProduct();
        var orderItems = new List<OrderItem>{new OrderItem{Product = product, NumberOfItems = Random.Next(1, 11)}};
        _carSparePartService.PlaceOrder(Order.Create(customer.CustomerId, orderItems));
    }
}
﻿using CarSparePartService;
using CarSparePartService.Interfaces;

namespace OnlineStoreEmulator;

public class OnlineStoreEmulator
{
    private ICarSparePartService CarSparePartService { get; }
    private ICustomerService CustomerService { get; }
    private IProductFetcher ProductFetcher { get; }
    private static readonly Random _random = new Random();
    private int _intervalSeconds = 15;
        
    public OnlineStoreEmulator(ICarSparePartService carSparePartService, ICustomerService customerService, IProductFetcher productFetcher)
    {
        CarSparePartService = carSparePartService;
        CustomerService = customerService;
        ProductFetcher = productFetcher;
        var onlineSaleThread = new Thread(() =>
        {
            while (true)
            {
                CreateOrder();
                Thread.Sleep(TimeSpan.FromSeconds(_intervalSeconds));
            }
        });
        onlineSaleThread.Start();
    }

    public void Start()
    {
        var onlineSaleThread = new Thread(() =>
        {
            while (true)
            {
                if (IsStopRequested)
                {
                    break;
                }
                Console.WriteLine("Creating order");
                CreateOrder();
                Thread.Sleep(TimeSpan.FromSeconds(_intervalSeconds));
            }
        });
        onlineSaleThread.Start();
    }

    private bool IsStopRequested { get; set; }

    public void Stop()
    {
        IsStopRequested = true;
    }
    
    internal void CreateOrder()
    {
        var customer = GetRandomCustomer();
        var product = GetRandomProduct();
        var orderItems = new List<OrderItem>{new OrderItem{Product = product, NumberOfItems = _random.Next(1, 11)}};
        CarSparePartService.PlaceOrder(customer, Order.Create(orderItems));
    }

    internal Customer GetRandomCustomer()
    {
        var allCustomers = CustomerService.GetAllCustomers().ToList();
        var i = _random.Next(0, allCustomers.Count());
        return allCustomers[i];
    }
    
    internal Product GetRandomProduct()
    {
        var allCProducts = ProductFetcher.GetAllProducts().ToList();
        var i = _random.Next(0, allCProducts.Count());
        return allCProducts[i];
    }
}
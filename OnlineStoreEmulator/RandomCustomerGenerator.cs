using CarSparePartService;
using CarSparePartService.Customer;
using CarSparePartService.Interfaces;
using OnlineStoreEmulator;

namespace TestServicesConfigurator;

public class RandomCustomerGenerator
    : IRandomCustomerGenerator
{
    private static readonly Random _random = new Random();
    private readonly ICustomerService _customerService;

    public RandomCustomerGenerator(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    public Customer GenerateCustomer()
    {
        var allCustomers = _customerService.GetAllCustomers().ToList();
        var i = _random.Next(0, allCustomers.Count());
        return allCustomers[i];
    }
}
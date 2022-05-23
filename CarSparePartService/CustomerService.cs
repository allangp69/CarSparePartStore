using CarSparePartService.Adapters;
using CarSparePartService.Interfaces;

namespace CarSparePartService;

public class CustomerService
    : ICustomerService 
{
    private readonly CustomerDataAdapter _customerDataAdapter;

    public CustomerService(CustomerDataAdapter customerDataAdapter)
    {
        _customerDataAdapter = customerDataAdapter;
    }
    
    public IEnumerable<Customer.Customer> GetAllCustomers()
    {
        return _customerDataAdapter.GetAllCustomers();
    }
}
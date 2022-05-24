using CarSparePartData.Customer;
using CarSparePartService.Customer;

namespace CarSparePartService.Adapters;

public class CustomerDataAdapter
{
    private readonly ICustomerRepository _customerRepository;
    private readonly CustomerRecordConverter _customerRecordConverter;

    public CustomerDataAdapter(ICustomerRepository customerRepository, CustomerRecordConverter customerRecordConverter)
    {
        _customerRepository = customerRepository;
        _customerRecordConverter = customerRecordConverter;
    }

    public IEnumerable<Customer.Customer> GetAllCustomers()
    {
        return _customerRecordConverter.ConvertFromRecord(_customerRepository.GetAllCustomers());
    }
}
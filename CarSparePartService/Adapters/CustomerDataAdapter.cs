using CarSparePartData.Customer;
using CarSparePartService.Customer;

namespace CarSparePartService.Adapters;

public class CustomerDataAdapter
{
    private readonly ICustomerRepository _customerRepository;
    private readonly CustomerDTOConverter _customerDtoConverter;

    public CustomerDataAdapter(ICustomerRepository customerRepository, CustomerDTOConverter customerDtoConverter)
    {
        _customerRepository = customerRepository;
        _customerDtoConverter = customerDtoConverter;
    }

    public IEnumerable<Customer.Customer> GetAllCustomers()
    {
        return _customerDtoConverter.ConvertFromDTO(_customerRepository.GetAllCustomers());
    }
}
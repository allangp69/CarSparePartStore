using System.Collections.Generic;
using CarSparePartService.Interfaces;
using CarSparePartStore.ViewModels.DTO;

namespace CarSparePartStore.Adapters;

public class CustomerAdapter : ICustomerAdapter
{
    private readonly ICustomerService _customerService;
    private readonly CustomerDTOConverter _customerDtoConverter;

    public CustomerAdapter(ICustomerService customerService, CustomerDTOConverter customerDtoConverter)
    {
        _customerService = customerService;
        _customerDtoConverter = customerDtoConverter;
    }
    public IEnumerable<CustomerDTO> GetAllCustomers()
    {
        return _customerDtoConverter.ConvertToDTO(_customerService.GetAllCustomers());
    }
}
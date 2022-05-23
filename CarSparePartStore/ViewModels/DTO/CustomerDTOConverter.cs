using System.Collections.Generic;
using System.Linq;
using CarSparePartService;
using CarSparePartService.Customer;

namespace CarSparePartStore.ViewModels.DTO;

public class CustomerDTOConverter
{
    public IEnumerable<CustomerDTO> ConvertToDTO(IEnumerable<Customer> customers)
    {
        var retval = new List<CustomerDTO>();
        if (customers is null || !customers.Any())
        {
            return retval;
        }

        retval.AddRange(customers.Select(customer => ConvertToDTO(customer)));

        return retval;
    }
    
    #region ConvertToDTO
    private CustomerDTO ConvertToDTO(Customer customer)
    {
        if (customer is null)
        {
            return null;
        }
        var retval = new CustomerDTO
        {
            CustomerId = customer.CustomerId,
            FirstName = customer.FirstName,
            LastName = customer.LastName
        };
        return retval;
    }
    #endregion ConvertToDTO
}
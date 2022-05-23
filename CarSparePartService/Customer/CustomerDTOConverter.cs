using CarSparePartData.Customer;

namespace CarSparePartService.Customer;

public class CustomerDTOConverter
{
    public IEnumerable<Customer> ConvertFromDTO(IEnumerable<CustomerDTO> customers)
    {
        var retval = new List<Customer>();
        if (customers is null || !customers.Any())
        {
            return retval;
        }

        retval.AddRange(customers.Select(customer => ConvertFromDTO(customer)));

        return retval;
    }

    #region ConvertFromDTO
    private Customer ConvertFromDTO(CustomerDTO customer)
    {
        if (customer is null)
        {
            return null;
        }
        var retval = new Customer
        {
            CustomerId = customer.CustomerId,
            FirstName = customer.FirstName,
            LastName = customer.LastName
        };
        return retval;
    }
    #endregion ConvertFromDTO
}
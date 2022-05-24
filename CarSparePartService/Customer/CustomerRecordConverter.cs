using CarSparePartData.Customer;

namespace CarSparePartService.Customer;

public class CustomerRecordConverter
{
    public IEnumerable<Customer> ConvertFromRecord(IEnumerable<CustomerRecord> customers)
    {
        var retval = new List<Customer>();
        if (customers is null || !customers.Any())
        {
            return retval;
        }

        retval.AddRange(customers.Select(customer => ConvertFromRecord(customer)));

        return retval;
    }

    #region ConvertFromRecord
    private Customer ConvertFromRecord(CustomerRecord customer)
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
    #endregion ConvertFromRecord
}
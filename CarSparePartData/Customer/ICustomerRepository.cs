namespace CarSparePartData.Customer;

public interface ICustomerRepository
{
    IEnumerable<CustomerRecord> GetAllCustomers();
}
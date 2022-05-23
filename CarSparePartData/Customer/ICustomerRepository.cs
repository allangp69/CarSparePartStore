namespace CarSparePartData.Customer;

public interface ICustomerRepository
{
    IEnumerable<CustomerDTO> GetAllCustomers();
}
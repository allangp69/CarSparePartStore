namespace CarSparePartService.Interfaces;

public interface ICustomerService
{
    IEnumerable<Customer.Customer> GetAllCustomers();
}
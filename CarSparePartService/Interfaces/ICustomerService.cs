namespace CarSparePartService.Interfaces;

public interface ICustomerService
{
    IEnumerable<Customer> GetAllCustomers();
}
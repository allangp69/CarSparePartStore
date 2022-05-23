using CarSparePartService;
using CarSparePartService.Customer;

namespace OnlineStoreEmulator;

public interface IRandomCustomerGenerator
{
    Customer GenerateCustomer();
}
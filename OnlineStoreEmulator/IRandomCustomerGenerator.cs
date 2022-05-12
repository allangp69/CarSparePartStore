using CarSparePartService;

namespace OnlineStoreEmulator;

public interface IRandomCustomerGenerator
{
    Customer GenerateCustomer();
}
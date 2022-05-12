using CarSparePartService;

namespace OnlineStoreEmulator;

public interface IRandomProductGenerator
{
    Product GenerateProduct();
}
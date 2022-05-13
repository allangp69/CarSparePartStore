using CarSparePartService;
using CarSparePartService.Product;

namespace OnlineStoreEmulator;

public interface IRandomProductGenerator
{
    Product GenerateProduct();
}
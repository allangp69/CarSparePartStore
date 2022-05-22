using System.Collections.Generic;
using CarSparePartStore.ViewModels.DTO;

namespace CarSparePartStore.Adapters;

public interface IProductsAndOrdersAdapter
{
    IEnumerable<ProductWithItemsCount> GetProductsWithItemsCount();
    IEnumerable<ProductDTO> GetAllProducts();
    IEnumerable<OrderDTO> GetOrdersForProduct(ProductDTO productDto);
    ProductDTO FindProduct(long productId);
    void PlaceOrder(OrderDTO order);
}
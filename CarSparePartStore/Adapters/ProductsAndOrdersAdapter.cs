using System.Collections.Generic;
using CarSparePartService.Interfaces;
using CarSparePartStore.ViewModels.DTO;

namespace CarSparePartStore.Adapters;

public class ProductsAndOrdersAdapter : IProductsAndOrdersAdapter
{
    private readonly ICarSparePartService _carSparePartService;
    private readonly OrderDTOConverter _orderDtoConverter;

    public ProductsAndOrdersAdapter(ICarSparePartService carSparePartService, OrderDTOConverter orderDtoConverter)
    {
        _carSparePartService = carSparePartService;
        _orderDtoConverter = orderDtoConverter;
    }
    public IEnumerable<ProductWithItemsCount> GetProductsWithItemsCount()
    {
        var retval = new List<ProductWithItemsCount>();
        var products = _carSparePartService.GetAllProducts();
        foreach (var product in products)
        {
            retval.Add(new ProductWithItemsCount(_orderDtoConverter.ConvertToDTO(product), _carSparePartService.GetNumberOfItemsSoldForProduct(product)));
        }
        return retval;
    }

    public IEnumerable<ProductDTO> GetAllProducts()
    {
        return _orderDtoConverter.ConvertToDTO(_carSparePartService.GetAllProducts());
    }

    public IEnumerable<OrderDTO> GetOrdersForProduct(ProductDTO productDto)
    {
        return _orderDtoConverter.ConvertToDTO(_carSparePartService.GetOrdersForProduct(_orderDtoConverter.ConvertFromDTO(productDto)));
    }

    public ProductDTO FindProduct(long productId)
    {
        return _orderDtoConverter.ConvertToDTO(_carSparePartService.FindProduct(productId));
    }

    public void PlaceOrder(OrderDTO order)
    {
        _carSparePartService.PlaceOrder(_orderDtoConverter.ConvertFromDTO(order));
    }
}
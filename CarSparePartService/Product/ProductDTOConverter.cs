using CarSparePartData.Product;

namespace CarSparePartService.Product;

public class ProductDTOConverter
{
    public IEnumerable<ProductDTO> ConvertToDTO(IEnumerable<Product> products)
    {
        var retval = new List<ProductDTO>();
        if (products is null || !products.Any())
        {
            return retval;
        }

        retval.AddRange(products.Select(product => ConvertToDTO(product)));

        return retval;
    }

    public IEnumerable<Product> ConvertFromDTO(IEnumerable<ProductDTO> products)
    {
        var retval = new List<Product>();
        if (products is null || !products.Any())
        {
            return retval;
        }

        retval.AddRange(products.Select(product => ConvertFromDTO(product)));

        return retval;
    }
    
    #region ConvertToDTO
    private ProductDTO ConvertToDTO(Product product)
    {
        if (product is null)
        {
            return null;
        }
        var retval = new ProductDTO
        {
            ProductId = product.ProductId,
            Name = product.Name,
            Price = product.Price,
            Type = product.Type,
            Description = product.Description
        };
        return retval;
    }
    #endregion ConvertToDTO
    
    #region ConvertFromDTO
    internal Product ConvertFromDTO(ProductDTO product)
    {
        if (product is null)
        {
            return null;
        }
        var retval = new Product
        {
            ProductId = product.ProductId,
            Name = product.Name,
            Price = product.Price,
            Type = product.Type,
            Description = product.Description
        };
        return retval;
    }
    #endregion ConvertFromDTO
}
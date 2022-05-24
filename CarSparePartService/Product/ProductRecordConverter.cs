using CarSparePartData.Product;

namespace CarSparePartService.Product;

public class ProductRecordConverter
{
    public IEnumerable<Product> ConvertFromRecord(IEnumerable<ProductRecord> products)
    {
        var retval = new List<Product>();
        if (products is null || !products.Any())
        {
            return retval;
        }

        retval.AddRange(products.Select(product => ConvertFromRecord(product)));

        return retval;
    }

    #region ConvertFromRecord
    internal Product ConvertFromRecord(ProductRecord product)
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
    #endregion ConvertFromRecord
}
using System.Data;
using CarSparePartData.Interfaces;

namespace CarSparePartData.Product;

public class ProductRepository
: IProductRepository
{
    private readonly ProductRepositoryConfig _productRepositoryConfig;

    public ProductRepository(ProductRepositoryConfig productRepositoryConfig)
    {
        _productRepositoryConfig = productRepositoryConfig;
    }

    public void LoadProductsFromBackup()
    {
        Products = ReadProductsFromXML(_productRepositoryConfig.BackupFilePath);
    }

    private IEnumerable<ProductDTO> Products { get;  set; }

    private IEnumerable<ProductDTO> ReadProductsFromXML(string fileName)
    {
        var file = new FileInfo(fileName);
        var dataset = new DataSet();
        dataset.ReadXml(file.FullName);
        var retval = new List<ProductDTO>();
        var productsTable = dataset.Tables[2];
        foreach (DataRow row in productsTable.Rows)
        {
            var product = new ProductDTO
            {
                Name = row[0].ToString(),
                Type = row[1].ToString(),
                Description = row[2].ToString(),
                ProductId = Convert.ToInt64(row[3].ToString()),
                Price = Convert.ToDecimal(row[4])
            };
            retval.Add(product);
        }
        return retval;
    }

    public IEnumerable<ProductDTO> GetAllProducts()
    {
        return Products;
    }

    public ProductDTO FindProduct(long productId)
    {
        return Products.FirstOrDefault(p => p.ProductId == productId);
    }
}
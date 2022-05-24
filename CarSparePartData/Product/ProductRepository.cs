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

    private IEnumerable<ProductRecord> Products { get;  set; }

    private IEnumerable<ProductRecord> ReadProductsFromXML(string fileName)
    {
        var file = new FileInfo(fileName);
        var dataset = new DataSet();
        dataset.ReadXml(file.FullName);
        var retval = new List<ProductRecord>();
        var productsTable = dataset.Tables[2];
        foreach (DataRow row in productsTable.Rows)
        {
            var product = new ProductRecord
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

    public IEnumerable<ProductRecord> GetAllProducts()
    {
        return Products;
    }

    public ProductRecord FindProduct(long productId)
    {
        return Products.FirstOrDefault(p => p.ProductId == productId);
    }
}
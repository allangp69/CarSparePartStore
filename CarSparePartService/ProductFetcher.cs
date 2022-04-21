using System.Data;
using CarSparePartService.Interfaces;

namespace CarSparePartService;

public class ProductFetcher
: IProductFetcher
{

    public void LoadProducts(string fileName)
    {
        Products = ReadProductsFromXML(fileName);
    }
    
    private IEnumerable<Product> Products { get;  set; }

    private IEnumerable<Product> ReadProductsFromXML(string fileName)
    {
        //var file = new FileInfo(@".\Resources\
        var file = new FileInfo(fileName);
        var dataset = new DataSet();
        dataset.ReadXml(file.FullName);
        var retval = new List<Product>();
        var productsTable = dataset.Tables[2];
        foreach (DataRow row in productsTable.Rows)
        {
            var product = new Product
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

    public IEnumerable<Product> GetAllProducts()
    {
        return Products;
    }

    public Product FindProduct(int productId)
    {
        return Products.FirstOrDefault(p => p.ProductId == productId);
    }
}
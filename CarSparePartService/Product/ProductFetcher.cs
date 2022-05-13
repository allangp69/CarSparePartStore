using System.Data;
using CarSparePartService.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Toolkit.Mvvm.DependencyInjection;

namespace CarSparePartService.Product;

public class ProductFetcher
: IProductFetcher
{

    public void LoadProducts(string fileName)
    {
        Products = ReadProductsFromXML(fileName);
    }

    public void LoadProductsFromBackup()
    {
        var configuration = Ioc.Default.GetRequiredService<IConfiguration>();
        var productsBackupFile = new FileInfo(configuration.GetSection("ApplicationSettings").GetSection("ProductsBackup").Value);
        LoadProducts(productsBackupFile.FullName);
    }

    private IEnumerable<global::CarSparePartService.Product.Product> Products { get;  set; }

    private IEnumerable<global::CarSparePartService.Product.Product> ReadProductsFromXML(string fileName)
    {
        var file = new FileInfo(fileName);
        var dataset = new DataSet();
        dataset.ReadXml(file.FullName);
        var retval = new List<global::CarSparePartService.Product.Product>();
        var productsTable = dataset.Tables[2];
        foreach (DataRow row in productsTable.Rows)
        {
            var product = new global::CarSparePartService.Product.Product
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

    public IEnumerable<global::CarSparePartService.Product.Product> GetAllProducts()
    {
        return Products;
    }

    public global::CarSparePartService.Product.Product FindProduct(int productId)
    {
        return Products.FirstOrDefault(p => p.ProductId == productId);
    }
}
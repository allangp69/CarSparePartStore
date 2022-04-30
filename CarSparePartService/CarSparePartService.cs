using System.Data;
using System.Xml.Serialization;
using CarSparePartService.Interfaces;

namespace CarSparePartService;

public class CarSparePartService
        : ICarSparePartService
{
    public CarSparePartService()
    {
        CustomersWithOrders = new Dictionary<Customer, List<Order>>();
    }
    
    public void CreateBackup(string filePath)
    {
        var productListing = CreateProductListing();
        var serializer = new XmlSerializer(productListing.GetType());
        using (TextWriter textWriter = new StreamWriter(filePath))
        {
            serializer.Serialize(textWriter, productListing);
        }
    }

    private ProductListing CreateProductListing()
    {
        var retval = new ProductListing("Motorudstyr butik");
        var allOrders = GetAllOrders();
        var products = allOrders.SelectMany(o => o.OrderItems.Select(i => i.Product));
        foreach (var product in products.Distinct())
        {
            if (!retval.ProductTypeList.Any(t => t.Title == product.Type))
            {
                retval.ProductTypeList.Add(new ProductType(product.Type));
            }
            var productType = retval.ProductTypeList.First(t => t.Title == product.Type); 
            if (!productType.Products.Any(p => p.Name == product.Name))
            {
                productType.Products.Add(product);
            }
        }
        return retval;
    }

    public void LoadBackup(string filePath)
    {
        var file = new FileInfo(filePath);
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
        var test = retval;
    }

    public void PlaceOrder(Customer customer, Order order)
    {
        if (!CustomersWithOrders.ContainsKey(customer))
        {
            CustomersWithOrders.Add(customer, new List<Order>());
        }
        CustomersWithOrders[customer].Add(order);
    }

    private Dictionary<Customer, List<Order>> CustomersWithOrders { get; }

    public IEnumerable<Order> GetAllOrders()
    {
        return CustomersWithOrders.SelectMany(c => c.Value);
    }
}
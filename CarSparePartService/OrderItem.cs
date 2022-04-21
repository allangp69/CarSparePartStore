using CarSparePartService.Interfaces;

namespace CarSparePartService;

public class OrderItem
{
    public Product Product { get; set; }
    public int NumberOfItems { get; set; }
}
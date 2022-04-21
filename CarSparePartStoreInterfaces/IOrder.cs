namespace CarSparePartStoreInterfaces;

public interface IOrder
{
    IEnumerable<IOrderItem> OrderItems { get; set; }
    decimal TotalCost();
    void AddItem(IOrderItem item);
}
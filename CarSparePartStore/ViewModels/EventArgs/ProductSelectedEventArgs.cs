namespace CarSparePartStore.ViewModels.EventArgs;

public class ProductSelectedEventArgs 
    : System.EventArgs
{
    public long ProductId { get; }

    public ProductSelectedEventArgs(long productId)
    {
        ProductId = productId;
    }
}
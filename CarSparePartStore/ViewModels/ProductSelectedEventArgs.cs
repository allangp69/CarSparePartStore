using System;

namespace CarSparePartStore.ViewModels;

public class ProductSelectedEventArgs 
    : EventArgs
{
    public long ProductId { get; }

    public ProductSelectedEventArgs(long productId)
    {
        ProductId = productId;
    }
}
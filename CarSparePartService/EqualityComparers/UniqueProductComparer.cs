namespace CarSparePartService.EqualityComparers;

public class UniqueProductComparer 
    : IEqualityComparer<Product.Product>
{
    public bool Equals(Product.Product x, Product.Product y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (ReferenceEquals(x, null)) return false;
        if (ReferenceEquals(y, null)) return false;
        if (x.GetType() != y.GetType()) return false;
        return x.ProductId == y.ProductId && x.Name == y.Name && x.Type == y.Type;
    }

    public int GetHashCode(Product.Product obj)
    {
        return HashCode.Combine(obj.ProductId, obj.Name, obj.Type);
    }
}
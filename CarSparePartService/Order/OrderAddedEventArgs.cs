namespace CarSparePartService.Order;

public class OrderAddedEventArgs
        :EventArgs
{
        public OrderAddedEventArgs(int customerId, string productsDescription)
        {
                CustomerId = customerId;
                ProductsDescription = productsDescription;
        }
        public int CustomerId { get; set; }
        public string ProductsDescription { get; set; }
}
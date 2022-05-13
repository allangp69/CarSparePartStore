namespace CarSparePartService;

public class OrderAddedEventArgs
        :EventArgs
{
        public int CustomerId { get; set; }
        public string Products { get; set; }
}
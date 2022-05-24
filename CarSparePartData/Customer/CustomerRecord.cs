namespace CarSparePartData.Customer;

public record CustomerRecord
{
    public int CustomerId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}
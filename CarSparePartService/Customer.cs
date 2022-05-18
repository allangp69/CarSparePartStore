namespace CarSparePartService;

public class Customer
{
    public int CustomerId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public string FirstAndLastName
    {
        get { return $"{FirstName} {LastName}"; }
    }

    public override string ToString()
    {
        return FirstAndLastName;
    }
}
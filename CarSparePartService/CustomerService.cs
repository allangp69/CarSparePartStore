using CarSparePartService.Interfaces;

namespace CarSparePartService;

public class CustomerService
    : ICustomerService
{
    private static Random random = new Random();
    private List<string> LastNames = new List<string>()
    {
        "Elsher",
        "Solace",
        "Levine",
        "Thatcher",
        "Raven",
        "Bardot",
        "St. James",
        "Hansley",
        "Cromwell",
        "Ashley",
        "Monroe",
        "West",
        "Langley",
        "Daughtler",
        "Madison",
        "Marley",
        "Ellis",
        "Hope",
        "Cassidy",
        "Lopez",
        "Jenkins",
        "Poverly",
        "McKenna",
        "Gonzales",
        "Keller",
        "Collymore",
        "Stoll",
        "Verlice",
        "Adler",
        "Huxley",
        "Ledger",
        "Hayes",
        "Ford",
        "Finnegan",
        "Beckett",
        "Gatlin",
        "Pierce",
        "Zimmerman",
        "Dawson",
        "Wilson",
        "Adair",
        "Gray",
        "Curran",
        "Crassus",
        "Anderson",
        "Adams",
        "Carter",
        "Hendrix",
        "Lennon",
        "Gasper",
        "Mintz",
        "Ashbluff",
        "Marblemaw",
        "Bozzelli",
        "Fellowes",
        "Windward",
        "Yarrow",
        "Yearwood",
        "Wixx",
        "Humblecut",
        "Dustfinger",
        "Biddercombe",
        "Kicklighter",
        "Vespertine",
        "October",
        "Gannon",
        "Truthbelly",
        "Woodgrip",
        "Gorestriker",
        "Caskcut",
        "Oatrun",
        "Sagespark",
        "Strongblossom",
        "Hydrafist",
        "Snakeleaf"
    };
    
    private List<string> FirstNames = new List<string>()
    {
        "Ava",
        "Amelia",
        "Abigail",
        "Alexander",
        "Aiden",
        "Anthony",
        "Emma",
        "Evelyn",
        "Emily",
        "Elijah",
        "Ethan",
        "Ezra",
        "Luna",
        "Layla",
        "Lily",
        "Liam",
        "Lucas",
        "Logan",
        "Olivia",
        "Oliver",
        "Owen",
        "Sophia",
        "Scarlett",
        "Stella",
        "Sebastian",
        "Samuel",
        "Santiago"
    };

    public CustomerService()
    {
        Customers = new List<Customer>();
        GenerateCustomers();
    }

    private void GenerateCustomers()
    {
        for (var i = 0; i < 1000; i++)
        {
            var customer = new Customer
            {
                FirstName = GetRandomStringFromList(FirstNames),
                LastName = GetRandomStringFromList(LastNames),
                CustomerId = i
            };
            Customers.Add(customer);
        }
    }

    private List<Customer> Customers { get; set; }

    private string GetRandomStringFromList(List<string> stringList)
    {
        var i = random.Next(0, stringList.Count); 
        return stringList[i];
    }

    public IEnumerable<Customer> GetAllCustomers()
    {
        return Customers;
    }
}
namespace CarSparePartData.Order;

public class OrderBackupConfig
{
    public OrderBackupConfig(string filePath)
    {
        FilePath = filePath;
    }
    public string FilePath { get; set; }
}
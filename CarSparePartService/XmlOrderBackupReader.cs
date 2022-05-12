using System.Xml.Serialization;
using CarSparePartService.Interfaces;

namespace CarSparePartService;

public class XmlOrderBackupReader
    : IOrderBackupReader
{
    public IEnumerable<Order> ReadBackup(string backupFile)
    {
        var serializer = new XmlSerializer(typeof(List<Order>));
        using (Stream reader = new FileStream(backupFile, FileMode.Open))
        {
            return (List<Order>)serializer.Deserialize(reader);
        }
    }
}
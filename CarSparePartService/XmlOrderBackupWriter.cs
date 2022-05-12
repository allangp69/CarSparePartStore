using System.Xml.Serialization;
using CarSparePartService.Interfaces;

namespace CarSparePartService;

public class XmlOrderBackupWriter
    : IOrderBackupWriter
{
    public bool WriteBackup(IEnumerable<Order> orders, string backupFile)
    {
        var serializer = new XmlSerializer(typeof(List<Order>));
        using (var fileStream = File.Create(backupFile))
        {
            { 
                serializer.Serialize(fileStream, orders.ToList());
            }
        }
        return true;
    }
}
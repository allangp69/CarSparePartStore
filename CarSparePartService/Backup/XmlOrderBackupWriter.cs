using System.Xml.Serialization;
using CarSparePartService.Interfaces;

namespace CarSparePartService.Backup;

public class XmlOrderBackupWriter
    : IOrderBackupWriter
{
    public bool WriteBackup(IEnumerable<OrderDTO> orders, string backupFile)
    {
        var serializer = new XmlSerializer(typeof(List<OrderDTO>));
        using (var fileStream = File.Create(backupFile))
        {
            { 
                serializer.Serialize(fileStream, orders.ToList());
            }
        }
        return true;
    }
}
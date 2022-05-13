using System.Xml.Serialization;
using CarSparePartService.Interfaces;

namespace CarSparePartService.Backup;

public class XmlOrderBackupReader
    : IOrderBackupReader
{
    public IEnumerable<OrderDTO> ReadBackup(string backupFile)
    {
        var serializer = new XmlSerializer(typeof(List<OrderDTO>));
        using (Stream reader = new FileStream(backupFile, FileMode.Open))
        {
            return (List<OrderDTO>)serializer.Deserialize(reader);
        }
    }
}
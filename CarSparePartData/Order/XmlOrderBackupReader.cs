using System.Xml.Serialization;
using CarSparePartService.Backup;
using CarSparePartService.Interfaces;
using Serilog;

namespace CarSparePartData.Order;

public class XmlOrderBackupReader
    : IOrderBackupReader
{
    private readonly string _backupFile;
    private readonly ILogger _logger;

    public XmlOrderBackupReader(string backupFile, ILogger logger)
    {
        _backupFile = backupFile;
        _logger = logger;
    }
    public IEnumerable<OrderDTO> ReadBackup()
    {
        if (!File.Exists(_backupFile))
        {
            _logger.Error($"Could not restore from backup - file: {_backupFile} doesn't exist");
            return new List<OrderDTO>();
        }
        var serializer = new XmlSerializer(typeof(List<OrderDTO>));
        using (Stream reader = new FileStream(_backupFile, FileMode.Open))
        {
            return (List<OrderDTO>)serializer.Deserialize(reader);
        }
    }
}
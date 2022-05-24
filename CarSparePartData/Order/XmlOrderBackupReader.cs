using System.Xml.Serialization;
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
    public IEnumerable<OrderRecord> ReadBackup()
    {
        if (!File.Exists(_backupFile))
        {
            _logger.Error($"Could not restore from backup - file: {_backupFile} doesn't exist");
            return new List<OrderRecord>();
        }
        var serializer = new XmlSerializer(typeof(List<OrderRecord>));
        using (Stream reader = new FileStream(_backupFile, FileMode.Open))
        {
            return (List<OrderRecord>)serializer.Deserialize(reader);
        }
    }
}
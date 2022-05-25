using System.Xml.Serialization;
using CarSparePartService.Interfaces;
using Serilog;

namespace CarSparePartData.Order;

public class XmlOrderBackupReader
    : IOrderBackupReader
{
    private readonly OrderBackupConfig _config;
    private readonly ILogger _logger;

    public XmlOrderBackupReader(OrderBackupConfig config, ILogger logger)
    {
        _config = config;
        _logger = logger;
    }
    public IEnumerable<OrderRecord> ReadBackup()
    {
        var backupFile = _config.FilePath;
        if (!File.Exists(backupFile))
        {
            _logger.Error($"Could not restore from backup - file: {backupFile} doesn't exist");
            return new List<OrderRecord>();
        }
        var serializer = new XmlSerializer(typeof(List<OrderRecord>));
        using (Stream reader = new FileStream(backupFile, FileMode.Open))
        {
            return (List<OrderRecord>)serializer.Deserialize(reader);
        }
    }
}
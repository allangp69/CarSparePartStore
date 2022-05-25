using System.Xml.Serialization;
using CarSparePartService.Interfaces;
using Serilog;

namespace CarSparePartData.Order;

public class XmlOrderBackupWriter
    : IOrderBackupWriter
{
    private readonly OrderBackupConfig _config;
    private readonly ILogger _logger;

    public XmlOrderBackupWriter(OrderBackupConfig config, ILogger logger)
    {
        _config = config;
        _logger = logger;
    }
    
    public bool WriteBackup(IEnumerable<OrderRecord> orders)
    {
        var backupFile = _config.FilePath;
        if (string.IsNullOrEmpty(backupFile))
        {
            _logger.Error($"Could not create backup - filename: {backupFile} is invalid");
            return false;
        }
        var serializer = new XmlSerializer(typeof(List<OrderRecord>));
        using (var fileStream = File.Create(backupFile))
        {
            { 
                serializer.Serialize(fileStream, orders.ToList());
            }
        }
        return true;
    }
}
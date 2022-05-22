﻿using System.Xml.Serialization;
using CarSparePartService.Interfaces;
using Serilog;

namespace CarSparePartService.Backup;

public class XmlOrderBackupWriter
    : IOrderBackupWriter
{
    private readonly string _backupFile;
    private readonly ILogger _logger;

    public XmlOrderBackupWriter(string backupFile, ILogger logger)
    {
        _backupFile = backupFile;
        _logger = logger;
    }
    
    public bool WriteBackup(IEnumerable<OrderDTO> orders)
    {
        if (string.IsNullOrEmpty(_backupFile))
        {
            _logger.Error($"Could not create backup - filename: {_backupFile} is invalid");
            return false;
        }
        var serializer = new XmlSerializer(typeof(List<OrderDTO>));
        using (var fileStream = File.Create(_backupFile))
        {
            { 
                serializer.Serialize(fileStream, orders.ToList());
            }
        }
        return true;
    }
}
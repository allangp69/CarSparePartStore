using CarSparePartService.Interfaces;

namespace CarSparePartService.Backup;

public class OrderBackupManager
    : IOrderBackupManager
{
    private readonly IOrderBackupWriter _backupWriter;
    private readonly IOrderBackupReader _backupReader;

    public OrderBackupManager(IOrderBackupWriter backupWriter, IOrderBackupReader backupReader)
    {
        _backupWriter = backupWriter;
        _backupReader = backupReader;
    }

    public void BackupToFile(IEnumerable<OrderDTO> orders, string backupFile)
    {
        _backupWriter.WriteBackup(orders, backupFile);
    }

    IEnumerable<OrderDTO> IOrderBackupManager.LoadBackupFromFile(string backupFile)
    {
        return _backupReader.ReadBackup(backupFile);
    }
}
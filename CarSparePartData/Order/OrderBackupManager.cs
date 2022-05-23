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

    public void Backup(IEnumerable<OrderDTO> orders)
    {
        _backupWriter.WriteBackup(orders);
    }

    public IEnumerable<OrderDTO> LoadBackup()
    {
        return _backupReader.ReadBackup();
    }
}
using CarSparePartService.Backup;

namespace CarSparePartService.Interfaces;

public interface IOrderBackupManager
{
    void BackupToFile(IEnumerable<OrderDTO> orders, string backupFile);
    IEnumerable<OrderDTO> LoadBackupFromFile(string backupFile);
}
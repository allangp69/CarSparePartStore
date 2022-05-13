using CarSparePartService.Backup;

namespace CarSparePartService.Interfaces;

public interface IOrderBackupWriter
{
    bool WriteBackup(IEnumerable<OrderDTO> orders, string backupFile);
}
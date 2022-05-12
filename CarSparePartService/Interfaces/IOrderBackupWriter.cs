namespace CarSparePartService.Interfaces;

public interface IOrderBackupWriter
{
    bool WriteBackup(IEnumerable<Order> orders, string backupFile);
}
namespace CarSparePartService.Interfaces;

public interface IOrderBackupManager
{
    void BackupToFile(IEnumerable<Order> orders, string backupFile);
    IEnumerable<Order> LoadBackupFromFile(string backupFile);
}
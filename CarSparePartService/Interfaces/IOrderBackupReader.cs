namespace CarSparePartService.Interfaces;

public interface IOrderBackupReader
{
    IEnumerable<Order> ReadBackup(string backupFile);
}
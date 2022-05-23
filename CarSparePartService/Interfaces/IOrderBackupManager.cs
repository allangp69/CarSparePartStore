namespace CarSparePartService.Interfaces;

public interface IOrderBackupManager
{
    void Backup(IEnumerable<Order.Order> orders);
    IEnumerable<Order.Order> LoadBackup();
}
using CarSparePartService.Backup;

namespace CarSparePartService.Interfaces;

public interface IOrderBackupManager
{
    void Backup(IEnumerable<OrderDTO> orders);
    IEnumerable<OrderDTO> LoadBackup();
}
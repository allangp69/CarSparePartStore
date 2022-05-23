using CarSparePartService.Backup;

namespace CarSparePartService.Interfaces;

public interface IOrderBackupReader
{
    IEnumerable<OrderDTO> ReadBackup();
}
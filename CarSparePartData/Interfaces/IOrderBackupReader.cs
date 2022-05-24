using CarSparePartData.Order;

namespace CarSparePartService.Interfaces;

public interface IOrderBackupReader
{
    IEnumerable<OrderRecord> ReadBackup();
}
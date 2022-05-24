using CarSparePartData.Order;

namespace CarSparePartService.Interfaces;

public interface IOrderBackupWriter
{
    bool WriteBackup(IEnumerable<OrderRecord> orders);
}
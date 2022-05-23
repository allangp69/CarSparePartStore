using CarSparePartService.Backup;
using CarSparePartService.Interfaces;

namespace CarSparePartService.Order;

public class OrderBackupManager
    : IOrderBackupManager
{
    private readonly IOrderBackupWriter _backupWriter;
    private readonly IOrderBackupReader _backupReader;
    private readonly OrderDTOConverter _orderDtoConverter;

    public OrderBackupManager(IOrderBackupWriter backupWriter, IOrderBackupReader backupReader, OrderDTOConverter orderDtoConverter)
    {
        _backupWriter = backupWriter;
        _backupReader = backupReader;
        _orderDtoConverter = orderDtoConverter;
    }

    public void Backup(IEnumerable<Order> orders)
    {
        _backupWriter.WriteBackup(_orderDtoConverter.ConvertToDTO(orders));
    }

    public IEnumerable<Order> LoadBackup()
    {
        return _orderDtoConverter.ConvertFromDTO(_backupReader.ReadBackup());
    }
}
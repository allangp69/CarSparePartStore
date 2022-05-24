using CarSparePartService.Interfaces;

namespace CarSparePartService.Order;

public class OrderBackupManager
    : IOrderBackupManager
{
    private readonly IOrderBackupWriter _backupWriter;
    private readonly IOrderBackupReader _backupReader;
    private readonly OrderRecordConverter _orderRecordConverter;

    public OrderBackupManager(IOrderBackupWriter backupWriter, IOrderBackupReader backupReader, OrderRecordConverter orderRecordConverter)
    {
        _backupWriter = backupWriter;
        _backupReader = backupReader;
        _orderRecordConverter = orderRecordConverter;
    }

    public void Backup(IEnumerable<Order> orders)
    {
        _backupWriter.WriteBackup(_orderRecordConverter.ConvertToRecord(orders));
    }

    public IEnumerable<Order> LoadBackup()
    {
        return _orderRecordConverter.ConvertFromRecord(_backupReader.ReadBackup());
    }
}
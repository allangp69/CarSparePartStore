using CarSparePartService.Interfaces;

namespace CarSparePartService;

public class XmlOrderBackupWriter
        : IOrderBackupWriter
{
        public bool WriteBackup(IEnumerable<Order> orders, string backupFile)
        {
                throw new NotImplementedException();
        }
}
using CarSparePartService.Interfaces;

namespace CarSparePartService;

public class XmlOrderBackupReader
        : IOrderBackupReader
{
        public IEnumerable<Order> ReadBackup(string backupFile)
        {
                throw new NotImplementedException();
        }
}
using System.Collections.Generic;
using CarSparePartStore.ViewModels.DTO;

namespace CarSparePartStore.Adapters;

public interface ICustomerAdapter
{
    IEnumerable<CustomerDTO> GetAllCustomers();
}
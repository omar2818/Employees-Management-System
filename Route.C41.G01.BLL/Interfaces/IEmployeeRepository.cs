using Route.C41.G01.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Route.C41.G01.BLL.Interfaces
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        Task<IEnumerable<Employee>> GetEmployeesByAddressAsync(string address);
        Task<IEnumerable<Employee>> GetEmployeesByNameAsync(string Name);
    }
}

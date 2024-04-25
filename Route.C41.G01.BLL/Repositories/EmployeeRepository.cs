using Microsoft.EntityFrameworkCore;
using Route.C41.G01.BLL.Interfaces;
using Route.C41.G01.DAL.Data;
using Route.C41.G01.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Route.C41.G01.BLL.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(ApplicationDbContext applicationDb)
            :base(applicationDb)
        {
            
        }

        public async Task<IEnumerable<Employee>> GetEmployeesByAddressAsync(string address)
        {
            return await _dbcontext.Employees.Where(E => E.Address.Contains(address)).Include(E => E.Department).AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<Employee>> GetEmployeesByNameAsync(string Name)
        {
            return await _dbcontext.Employees.Where(E => E.Name.Contains(Name)).Include(E => E.Department).AsNoTracking().ToListAsync();
        }
    }
}

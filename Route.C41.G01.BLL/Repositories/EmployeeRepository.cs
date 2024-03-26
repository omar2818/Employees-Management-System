using Microsoft.EntityFrameworkCore;
using Route.C41.G01.BLL.Interfaces;
using Route.C41.G01.DAL.Data;
using Route.C41.G01.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public IQueryable<Employee> GetEmployeesByAddress(string address)
        {
            return _dbcontext.Employees.Where(E => address.Equals(E.Address, StringComparison.OrdinalIgnoreCase));
        }
    }
}

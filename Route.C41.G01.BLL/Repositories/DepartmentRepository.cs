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
    public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(ApplicationDbContext applicationDb)
            :base(applicationDb)
        {
            
        }

        public async Task<IEnumerable<Department>> GetDepartmentsByNameAsync(string Name)
        {
            return await _dbcontext.Departments.Where(D => D.Name.Contains(Name)).ToListAsync();

            //return _dbcontext.Departments.ToList<Department>().Where(D => Name.Equals(D.Name, StringComparison.OrdinalIgnoreCase));
        }
    }
}

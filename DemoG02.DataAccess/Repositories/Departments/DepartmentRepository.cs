using DemoG02.DataAccess.Data.Contexts;
using DemoG02.DataAccess.Models.Departments;
using DemoG02.DataAccess.Repositories.Generics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoG02.DataAccess.Repositories.Departments
{
    public class DepartmentRepository : GenericRepository<Department> ,IDepartmentRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public DepartmentRepository(ApplicationDbContext dbContext) : base(dbContext) // Inject
                                                                    // Ask CLR for Creating object of type ApplicationDbContext
        {
            _dbContext = dbContext;
        }

    }
}

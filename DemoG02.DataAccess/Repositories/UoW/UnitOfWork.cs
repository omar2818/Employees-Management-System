using DemoG02.DataAccess.Data.Contexts;
using DemoG02.DataAccess.Repositories.Departments;
using DemoG02.DataAccess.Repositories.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoG02.DataAccess.Repositories.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        private Lazy<IEmployeeRepository> _employeeRepository;
        private Lazy<IDepartmentRepository> _departmentRepository;
        private readonly ApplicationDbContext _dbContext;
        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _employeeRepository = new Lazy<IEmployeeRepository>(() => new EmployeeRepository(_dbContext)) ;
            _departmentRepository = new Lazy<IDepartmentRepository>(() => new DepartmentRepository(_dbContext)) ;
        }
        public IEmployeeRepository EmployeeRepository => _employeeRepository.Value;

        public IDepartmentRepository DepartmentRepository => _departmentRepository.Value;

        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }
    }
}

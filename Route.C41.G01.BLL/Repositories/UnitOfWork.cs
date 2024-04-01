using Route.C41.G01.BLL.Interfaces;
using Route.C41.G01.DAL.Data;
using Route.C41.G01.DAL.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Route.C41.G01.BLL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;

        //private Dictionary<string, IGenericRepository<ModelBase>> _repositories;
        private Hashtable _repositories;

        //public IEmployeeRepository EmployeeRepository { get; set; }
        //public IDepartmentRepository DepartmentRepository { get; set; }

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _repositories = new Hashtable();
            //EmployeeRepository = new EmployeeRepository(_dbContext);
            //DepartmentRepository = new DepartmentRepository(_dbContext);
        }

        public IGenericRepository<T> Repository<T>() where T : ModelBase
        {
            var key = typeof(T).Name;

            if(!_repositories.ContainsKey(key))
            {
                if (key == nameof(Employee))
                {
                    var repository = new EmployeeRepository(_dbContext);
                    _repositories.Add(key, repository);
                }
                else if (key == nameof(Department))
                {
                    var repository = new DepartmentRepository(_dbContext);
                    _repositories.Add(key, repository);
                }
                else
                {
                    var repository =new GenericRepository<T>(_dbContext);
                    _repositories.Add(key, repository);
                }
            }

            return _repositories[key] as IGenericRepository<T>;
        }
        
        public int Complete()
        {
            return _dbContext.SaveChanges();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

    }
}

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
    public class GenericRepository<T> : IGenericRepository<T> where T : ModelBase
    {
        private protected readonly ApplicationDbContext _dbcontext;
        public GenericRepository(ApplicationDbContext applicationDb)
        {
            _dbcontext = applicationDb;
        }
        public void Add(T entity)
        {
            _dbcontext.Set<T>().Add(entity);
            //return _dbcontext.SaveChanges();
        }
        public void Update(T entity)
        {
            _dbcontext.Set<T>().Update(entity);
            //return _dbcontext.SaveChanges();
        }

        public void Delete(T entity)
        {
            _dbcontext.Set<T>().Remove(entity);
            //return _dbcontext.SaveChanges();
        }

        public async Task<T> GetAsync(int id)
        {
            //return _dbcontext.Employees.Find(id);

            return await _dbcontext.FindAsync<T>(id);
            ///var Employee = _dbcontext.Employees.Local.Where(D => D.Id == id).FirstOrDefault();
            ///if(Employee == null)
            ///{
            ///    Employee = _dbcontext.Employees.Where(D => D.Id == id).FirstOrDefault();
            ///}
            ///return Employee;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            if(typeof(T) == typeof(Employee))
            {
                return (IEnumerable<T>) await _dbcontext.Employees.Include(E => E.Department).AsNoTracking().ToListAsync();
            }
            return await _dbcontext.Set<T>().AsNoTracking().ToListAsync();
        }
    }
}

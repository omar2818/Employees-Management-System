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
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _dbcontext;
        public EmployeeRepository(ApplicationDbContext applicationDb)
        {
            _dbcontext = applicationDb;
        }
        public int Add(Employee entity)
        {
            _dbcontext.Add(entity);
            return _dbcontext.SaveChanges();
        }
        public int Update(Employee entity)
        {
            _dbcontext.Update(entity);
            return _dbcontext.SaveChanges();
        }

        public int Delete(Employee entity)
        {
            _dbcontext.Remove(entity);
            return _dbcontext.SaveChanges();
        }

        public Employee Get(int id)
        {
            //return _dbcontext.Employees.Find(id);

            return _dbcontext.Find<Employee>(id);
            ///var Employee = _dbcontext.Employees.Local.Where(D => D.Id == id).FirstOrDefault();
            ///if(Employee == null)
            ///{
            ///    Employee = _dbcontext.Employees.Where(D => D.Id == id).FirstOrDefault();
            ///}
            ///return Employee;
        }

        public IEnumerable<Employee> GetAll()
        {
            return _dbcontext.Employees.AsNoTracking().ToList();
        }
    }
}

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
    internal class DepartmentRepository : IDepartmentRepository
    {
        private readonly ApplicationDbContext _dbcontext;
        public DepartmentRepository(ApplicationDbContext applicationDb)
        {
            _dbcontext = applicationDb;
        }
        public int Add(Department entity)
        {
            _dbcontext.Add(entity);
            return _dbcontext.SaveChanges();
        }
        public int Update(Department entity)
        {
            _dbcontext.Update(entity);
            return _dbcontext.SaveChanges();
        }

        public int Delete(Department entity)
        {
            _dbcontext.Remove(entity);
            return _dbcontext.SaveChanges();
        }

        public Department Get(int id)
        {
            //return _dbcontext.Departments.Find(id);

            return _dbcontext.Find< Department>(id);
            ///var department = _dbcontext.Departments.Local.Where(D => D.Id == id).FirstOrDefault();
            ///if(department == null)
            ///{
            ///    department = _dbcontext.Departments.Where(D => D.Id == id).FirstOrDefault();
            ///}
            ///return department;
        }

        public IEnumerable<Department> GetAll()
        {
            return _dbcontext.Departments.AsNoTracking().ToList();
        }

    }
}

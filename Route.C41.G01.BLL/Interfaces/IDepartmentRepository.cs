using Route.C41.G01.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Route.C41.G01.BLL.Interfaces
{
    public interface IDepartmentRepository
    {
        IEnumerable<Department> GetAll();
        
        Department Get(int id);
        
        int Add(Department entity);
        
        int Update(Department entity);
        
        int Delete(Department entity);
    }
}

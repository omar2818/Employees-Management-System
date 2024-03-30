using Route.C41.G01.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Route.C41.G01.BLL.Interfaces
{
    public interface IGenericRepository<T>  where T : class
    {
        IEnumerable<T> GetAll();

        T Get(int id);

        void Add(T entity);

        void Update(T entity);

        void Delete(T entity);
    }
}

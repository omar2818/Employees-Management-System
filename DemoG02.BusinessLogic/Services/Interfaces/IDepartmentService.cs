using DemoG02.BusinessLogic.DataTransferObjects.Departments;
using DemoG02.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoG02.BusinessLogic.Services.Interfaces
{
    public interface IDepartmentService
    {
        public IEnumerable<DepartmentDto> GetAllDepartments();
        public DepartmentDetailsDto? GetDepartmentById(int id);
        public int AddDepartment(CreatedDepartmentDto departmentDto);
        public int UpdateDepartment(UpdatedDepartmentDto departmentDto);
        public bool DeleteDepartment(int id);
    }
}

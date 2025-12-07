using DemoG02.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoG02.BusinessLogic.DataTransferObjects.Departments
{
    public class DepartmentDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Code { get; set; } = null!;
        public string? Descripation { get; set; }
        public int CreatedBy { get; set; }
        public DateOnly DateofCreation { get; set; }
        public int LastModifiedBy { get; set; }
        public bool isDeleted { get; set; }
    }
}

using Route.C41.G01.DAL.Models;
using System.ComponentModel.DataAnnotations;
using System;

namespace Route.C41.G01.PL.ViewModels
{
    public class EmployeeResponseViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? Age { get; set; }

        public string Address { get; set; }

        public decimal Salary { get; set; }

        public bool IsActive { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime HiringDate { get; set; }

        public Gender Gender { get; set; }

        public EmpType EmployeeType { get; set; }

        public string ImageName { get; set; }

        //[ForeignKey("Department")]
        // Foreign Key Column
        public int? DepartmentId { get; set; }

        //[InverseProperty(nameof(Models.Department.Employees))]
        // Navigational Property [One]
        public virtual Department Department { get; set; }
    }
}

using DemoG02.BusinessLogic.DataTransferObjects.Departments;
using DemoG02.DataAccess.Models.Departments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoG02.BusinessLogic.Factories
{
    internal static class DepartmentFactory
    {
        public static DepartmentDetailsDto ToDepartmentDetailsDto(this Department department)
        {
            return new DepartmentDetailsDto()
            {
                Id = department.Id,
                Code = department.Code,
                Name = department.Name,
                CreatedBy = department.CreatedBy,
                Descripation = department.Descripation,
                LastModifiedBy = department.LastModifiedBy,
                isDeleted = department.isDeleted,
                DateofCreation = DateOnly.FromDateTime(department.CreatedOn ?? DateTime.Now)
            };
        }

        public static DepartmentDto ToDepartmentDto(this Department department)
        {
            return new DepartmentDto()
            {
                Id = department.Id,
                Code = department.Code,
                Name = department.Name,
                Descripation = department.Descripation,
                DateofCreation = DateOnly.FromDateTime(department.CreatedOn ?? DateTime.Now)
            };
        }

        public static Department ToEntity(this CreatedDepartmentDto departmentDto)
        {
            return new Department()
            {
                Code = departmentDto.Code,
                Name = departmentDto.Name,
                Descripation = departmentDto.Descripation,
                CreatedOn = departmentDto.DateofCreation.ToDateTime(new TimeOnly())
            };
        }

        public static Department ToEntity(this UpdatedDepartmentDto departmentDto)
        {
            return new Department()
            {
                Id = departmentDto.Id,
                Code = departmentDto.Code,
                Name = departmentDto.Name,
                Descripation = departmentDto.Description,
                CreatedOn = departmentDto.DateofCreation.ToDateTime(new TimeOnly())
            };
        }
    }
}

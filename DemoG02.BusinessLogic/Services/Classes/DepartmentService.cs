using DemoG02.BusinessLogic.DataTransferObjects.Departments;
using DemoG02.BusinessLogic.Factories;
using DemoG02.BusinessLogic.Services.Interfaces;
using DemoG02.DataAccess.Models;
using DemoG02.DataAccess.Repositories.Departments;
using DemoG02.DataAccess.Repositories.UoW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoG02.BusinessLogic.Services.Classes
{
    public class DepartmentService(IUnitOfWork unitOfWork) : IDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;


        public IEnumerable<DepartmentDto> GetAllDepartments()
        {
            var departments = _unitOfWork.DepartmentRepository.GetAll();
            var departmentsToReturn = departments.Select(D => D.ToDepartmentDto());

            return departmentsToReturn;
        }

        public DepartmentDetailsDto? GetDepartmentById(int id)
        {
            var department = _unitOfWork.DepartmentRepository.GetById(id);
            ///if(department is null)
            ///{
            ///    return null;
            ///}
            ///else
            ///{
            ///    return new DepartmentDetailsDto()
            ///    {
            ///        Id = id,
            ///        Code = department.Code,
            ///        Name = department.Name,
            ///        CreatedBy = department.CreatedBy,
            ///        Descripation = department.Descripation,
            ///        LastModifiedBy = department.LastModifiedBy,
            ///        isDeleted = department.isDeleted,
            ///        DateofCreation = DateOnly.FromDateTime(department.CreatedOn ?? DateTime.Now)
            ///    };
            ///}

            // Manual Mapping
            // AutoMapper
            // Custructor Mapping
            // Extension Method
            return department is null ? null : department.ToDepartmentDetailsDto();
        }
        public int AddDepartment(CreatedDepartmentDto departmentDto)
        {
            _unitOfWork.DepartmentRepository.Add(departmentDto.ToEntity());
            return _unitOfWork.SaveChanges();
        }

        public int UpdateDepartment(UpdatedDepartmentDto departmentDto)
        {
            _unitOfWork.DepartmentRepository.Update(departmentDto.ToEntity());
            return _unitOfWork.SaveChanges();
        }
        public bool DeleteDepartment(int id)
        {
            var department = _unitOfWork.DepartmentRepository.GetById(id);
            if(department is null)
            {
                return false;
            }
            else
            {
                _unitOfWork.DepartmentRepository.Remove(department);
                var Result = _unitOfWork.SaveChanges();
                return Result > 0 ? true : false;
            }
        }
    }
}

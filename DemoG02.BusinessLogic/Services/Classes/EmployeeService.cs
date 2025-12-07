using AutoMapper;
using DemoG02.BusinessLogic.DataTransferObjects.Employees;
using DemoG02.BusinessLogic.Services.Interfaces;
using DemoG02.DataAccess.Models.Employees;
using DemoG02.DataAccess.Repositories.Employees;
using DemoG02.DataAccess.Repositories.UoW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoG02.BusinessLogic.Services.Classes
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAttachmentService _attachmentService;
        private readonly IMapper _mapper;

        public EmployeeService(IUnitOfWork unitOfWork,
                               IAttachmentService attachmentService,
                               IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _attachmentService = attachmentService;
            _mapper = mapper;
        }
        public IEnumerable<EmployeeDto> GetAllEmployees(string? EmployeeSearchName, bool withTracking = false)
        {

            ///var employeesToReturn = employees.Select(E => new EmployeeDto()
            ///{
            ///    Id = E.Id,
            ///    Name = E.Name,
            ///    Age = E.Age,
            ///    Email = E.Email,
            ///    Salary = E.Salary,
            ///    IsActive = E.IsActive,
            ///    Gender = E.Gender.ToString(),
            ///    EmployeeType = E.EmployeeType.ToString()
            ///});

            IEnumerable<Employee> employees;
            if (string.IsNullOrWhiteSpace(EmployeeSearchName))
            {
                employees = _unitOfWork.EmployeeRepository.GetAll(withTracking);
            }
            else
            {
                employees = _unitOfWork.EmployeeRepository.GetAll(E => E.Name.ToLower().Contains(EmployeeSearchName.ToLower()));
            }

            // TSource => src => Employee
            // TDestination => Dist => EmployeeDto
            var employeesToReturn = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeDto>>(employees);
            return employeesToReturn;
        }

        public EmployeeDetailsDto? GetEmployeeById(int id)
        {
            var employee = _unitOfWork.EmployeeRepository.GetById(id);

            return employee is null ? null : _mapper.Map<Employee, EmployeeDetailsDto>(employee);
            ///return employee is null ? null : new EmployeeDetailsDto()
            ///{
            ///    Id = employee.Id,
            ///    Name = employee.Name,
            ///    Age = employee.Age,
            ///    Email = employee.Email,
            ///    Address = employee.Address,
            ///    Salary = employee.Salary,
            ///    PhoneNumber = employee.PhoneNumber,
            ///    IsActive = employee.IsActive,
            ///    HiringDate = DateOnly.FromDateTime(employee.HiringDate),
            ///    Gender = employee.Gender.ToString(),
            ///    EmployeeType = employee.EmployeeType.ToString(),
            ///    CreatedBy = employee.CreatedBy,
            ///    CreatedOn = employee.CreatedOn,
            ///    LastModifiedBy = employee.LastModifiedBy,
            ///    LastModifiedOn = employee.LastModifiedOn
            ///};
        }
        public int CreateEmployee(CreatedEmployeeDto employeeDto)
        {
            var employee = _mapper.Map<CreatedEmployeeDto, Employee>(employeeDto);
            if(employeeDto.Image is not null)
            {
                employee.ImageName = _attachmentService.Upload(employeeDto.Image, "Images");
            }
            _unitOfWork.EmployeeRepository.Add(employee);
            return _unitOfWork.SaveChanges();
        }
        public int UpdateEmployee(UpdatedEmployeeDto employeeDto)
        {
            _unitOfWork.EmployeeRepository.Update(_mapper.Map<UpdatedEmployeeDto, Employee>(employeeDto));
            return _unitOfWork.SaveChanges();
        }

        public bool DeleteEmployee(int id)
        {
            var employee = _unitOfWork.EmployeeRepository.GetById(id);
            if (employee is null) return false;

            employee.isDeleted = true;
            _unitOfWork.EmployeeRepository.Update(employee);
            var result = _unitOfWork.SaveChanges();
            if(result > 0) return true;
            return false;

            /// Hard Delete
            ///var result = _employeeRepository.Remove(employee);
            ///if(result > 0) return true;
            ///return false;
        }


    }
}

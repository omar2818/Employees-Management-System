using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Route.C41.G01.BLL.Interfaces;
using Route.C41.G01.BLL.Repositories;
using Route.C41.G01.DAL.Models;
using Route.C41.G01.PL.Hepers;
using Route.C41.G01.PL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Route.C41.G01.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;
        //private readonly IEmployeeRepository _employeesRepo;
        //private readonly IDepartmentRepository _departmentRepo;

        public EmployeeController(IMapper mapper,
            IUnitOfWork unitOfWork,
            IWebHostEnvironment env
            //IEmployeeRepository employeeRepository, 
            /*IDepartmentRepository departmentRepo,*/ )
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            //_employeesRepo = employeeRepository;
            _env = env;
            //_departmentRepo = departmentRepo;
        }

        // /Employee/Index
        public IActionResult Index(string searching)
        {
            ///TempData.Keep();
            ///TempData.Save();
            /// Binding Through View's Dictionary : Transfer Data from Action to View [One Way]
            /// 1. ViewData
            ///ViewData["Message"] = "Hello Data";
            /// 2. ViewBag
            ///ViewBag.Message = "Hello Bag";

            var Employees = Enumerable.Empty<Employee>();
            var employeeRepo = _unitOfWork.Repository<Employee>() as EmployeeRepository;
            if (string.IsNullOrEmpty(searching)) {
                Employees = employeeRepo.GetAll();
            }else
            {
                Employees = employeeRepo.GetEmployeesByName(searching);
            }

            var mappedEmps = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeResponseViewModel>>(Employees);
            return View(mappedEmps);
        }

        // /Employee/Create
        //[HttpGet]
        public IActionResult Create()
        {
            //ViewData["Departments"] = _departmentRepo.GetAll();

            return View();
        }

        [HttpPost]
        public IActionResult Create(EmployeeViewModel employeevm)
        {
            if (ModelState.IsValid) // server side validation
            {
                // Manual Mapping
                // 1. using object initializer
                // 2. overload casting operator

                // Automatic Mappings

                employeevm.ImageName = DocumentSettings.UploadFile(employeevm.Image, "Images");
                var mappedEmp = _mapper.Map<EmployeeViewModel, Employee>(employeevm);

                _unitOfWork.Repository<Employee>().Add(mappedEmp);

                var count = _unitOfWork.Complete();

                // 3. TempData

                if (count > 0)
                {
                    TempData["Message"] = "Employee is Created Successfuly";
                }
                else
                {
                    TempData["Message"] = "An Error Has Occured, Employee Not Created";
                }
                return RedirectToAction(nameof(Index));
            }
            return View(employeevm);
        }

        // /Employee/Details/10
        // /Employee/Details
        //[HttpGet]
        public IActionResult Details(int? id, string viewName = "Details")
        {
            if (id is null)
            {
                return BadRequest(); // 400
            }

            var employee = _unitOfWork.Repository<Employee>().Get(id.Value);

            if (employee is null)
            {
                return NotFound();  // 404
            }

            if(viewName == "Edit")
            {
                var mapperEmp1 = _mapper.Map<Employee, EmployeeViewModel>(employee);

                return View(viewName, mapperEmp1);
            }

            var mapperEmp = _mapper.Map<Employee, EmployeeResponseViewModel>(employee);
            
            return View(viewName, mapperEmp);
        }

        // /Employee/Edit/10
        // /Employee/Edit
        //[HttpGet]
        public IActionResult Edit(int? id)
        {
            //ViewData["Departments"] = _departmentRepo.GetAll();

            return Details(id, "Edit");
            ///if (!id.HasValue)
            ///{
            ///    return BadRequest(); // 400
            ///}
            ///var Employee = _EmployeesRebo.Get(id.Value);
            ///if (Employee is null)
            ///{
            ///    return NotFound(); // 404
            ///}
            ///return View(Employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, EmployeeViewModel employeeVM)
        {
            //if (id != employeeVM.Id)
            //{
            //    return BadRequest();
            //}

            if (!ModelState.IsValid)
            {
                return View(employeeVM);
            }

            try
            {
                var mappedEmp = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);

                _unitOfWork.Repository<Employee>().Update(mappedEmp);
                _unitOfWork.Complete();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // 1. Log Exception
                // 2. Friendly Message

                if (_env.IsDevelopment())
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "An Error Has Occured During Updating the Employee");
                }

                return View(employeeVM);

            }
        }

        // /Employee/Delete/10
        // /Employee/Delete
        //[HttpGet]
        public IActionResult Delete(int? id)
        {
            return Details(id, "Delete");
        }

        [HttpPost]
        public IActionResult Delete(EmployeeResponseViewModel employeeVM)
        {
            try
            {
                var mappedEmp = _mapper.Map<EmployeeResponseViewModel, Employee>(employeeVM);


                _unitOfWork.Repository<Employee>().Delete(mappedEmp);
                _unitOfWork.Complete();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // 1. Log Exception
                // 2. Frindly Message

                if (_env.IsDevelopment())
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "An Error Has Occured During Updating the Employee");
                }

                return View(employeeVM);
            }
        }
    }
}

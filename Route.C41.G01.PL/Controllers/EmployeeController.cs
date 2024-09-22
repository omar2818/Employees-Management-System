using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
using System.Threading.Tasks;

namespace Route.C41.G01.PL.Controllers
{
    [Authorize]
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
        public async Task<IActionResult> Index(string searching)
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
                Employees = await employeeRepo.GetAllAsync();
            }else
            {
                Employees = await employeeRepo.GetEmployeesByNameAsync(searching);
            }

            var mappedEmps = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeResponseViewModel>>(Employees);
            return View(mappedEmps);
        }

        public async Task<IActionResult> Search(string searchInput)
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
            if (string.IsNullOrEmpty(searchInput))
            {
                Employees = await employeeRepo.GetAllAsync();
            }
            else
            {
                Employees = await employeeRepo.GetEmployeesByNameAsync(searchInput);
            }

            var result = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeResponseViewModel>>(Employees);

            return PartialView("EmployeeTablePartialView", result);
        }

        // /Employee/Create
        //[HttpGet]
        public IActionResult Create()
        {
            //ViewData["Departments"] = _departmentRepo.GetAllAsync();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(EmployeeViewModel employeevm)
        {
            if (ModelState.IsValid) // server side validation
            {
                // Manual Mapping
                // 1. using object initializer
                // 2. overload casting operator

                // Automatic Mappings

                employeevm.ImageName = await DocumentSettings.UploadFile(employeevm.Image, "Images");
                var mappedEmp = _mapper.Map<EmployeeViewModel, Employee>(employeevm);

                _unitOfWork.Repository<Employee>().Add(mappedEmp);

                var count = await _unitOfWork.Complete();

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
        public async Task<IActionResult> Details(int? id, string viewName = "Details")
        {
            if (id is null)
            {
                return BadRequest(); // 400
            }

            var employee = await _unitOfWork.Repository<Employee>().GetAsync(id.Value);

            if (employee is null)
            {
                return NotFound();  // 404
            }

            if(viewName == "Edit")
            {
                var mapperEmp1 = _mapper.Map<Employee, EmployeeViewModel>(employee);
                TempData["ImageName"] = employee.ImageName;
                return View(viewName, mapperEmp1);
            }

            if(viewName == "Delete")
            {
                TempData["ImageName"] = employee.ImageName;
            }

            var mapperEmp = _mapper.Map<Employee, EmployeeResponseViewModel>(employee);
            
            return View(viewName, mapperEmp);
        }

        // /Employee/Edit/10
        // /Employee/Edit
        //[HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            //ViewData["Departments"] = _departmentRepo.GetAllAsync();

            return await Details(id, "Edit");
            ///if (!id.HasValue)
            ///{
            ///    return BadRequest(); // 400
            ///}
            ///var Employee = _EmployeesRebo.GetAsync(id.Value);
            ///if (Employee is null)
            ///{
            ///    return NotFound(); // 404
            ///}
            ///return View(Employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, EmployeeViewModel employeeVM)
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
                employeeVM.ImageName = await DocumentSettings.UploadFile(employeeVM.Image, "Images");
                var mappedEmp = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);

                _unitOfWork.Repository<Employee>().Update(mappedEmp);
                var count = await _unitOfWork.Complete();
                if (count > 0)
                {
                    DocumentSettings.DeleteFile(TempData["ImageName"] as string, "Images");
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    DocumentSettings.DeleteFile(employeeVM.ImageName, "Images");
                }
                return View(employeeVM);
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
        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EmployeeResponseViewModel employeeVM)
        {
            try
            {
                employeeVM.ImageName = TempData["ImageName"] as string;
                var mappedEmp = _mapper.Map<EmployeeResponseViewModel, Employee>(employeeVM);


                _unitOfWork.Repository<Employee>().Delete(mappedEmp);
                var count = await _unitOfWork.Complete();
                if(count > 0)
                {
                    DocumentSettings.DeleteFile(employeeVM.ImageName, "Images");
                    return RedirectToAction(nameof(Index));
                }
                return View(employeeVM);
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

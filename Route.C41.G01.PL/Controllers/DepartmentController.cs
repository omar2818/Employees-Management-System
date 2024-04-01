using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Route.C41.G01.BLL.Interfaces;
using Route.C41.G01.BLL.Repositories;
using Route.C41.G01.DAL.Models;
using Route.C41.G01.PL.ViewModels;
using System;
using System.Linq;

namespace Route.C41.G01.PL.Controllers
{
    // Inheritance : DepartmentController is  a Controller
    // Association : DepartmentController has a DepartmentRepository
    public class DepartmentController : Controller
    {
        //private readonly IDepartmentRepository _departmentsRebo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _env;

        public DepartmentController(
            //IDepartmentRepository departmentRepository,
            IUnitOfWork unitOfWork,
            IWebHostEnvironment env)
        {
            //_departmentsRebo = departmentRepository;
            _unitOfWork = unitOfWork;
            _env = env;
        }

        // /Department/Index
        public IActionResult Index(string searching)
        {
            //var departments = _unitOfWork.Repository<Department>().GetAll();
            var departments = Enumerable.Empty<Department>();
            var DepartmentRepo = _unitOfWork.Repository<Department>() as DepartmentRepository;
            if (string.IsNullOrEmpty(searching))
            {
                departments = DepartmentRepo.GetAll();
            }
            else
            {
                departments = DepartmentRepo.GetDepartmentsByName(searching);
            }

            return View(departments);
        }
        /*var Employees = Enumerable.Empty<Employee>();
            var employeeRepo = _unitOfWork.Repository<Employee>() as EmployeeRepository;
            if (string.IsNullOrEmpty(searching)) {
                Employees = _unitOfWork.Repository<Employee>().GetAll();
            }else
            {
                Employees = employeeRepo.GetEmployeesByName(searching);
            }

            var mappedEmps = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(Employees);
            return View(mappedEmps);*/

        // /Department/Create
        //[HttpGet]
        public IActionResult Create()
        {
            return View();  
        }

        [HttpPost]
        public IActionResult Create(Department department)
        {
            if(ModelState.IsValid) // server side validation
            {
                _unitOfWork.Repository<Department>().Add(department);
                var count = _unitOfWork.Complete();
                if(count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(department);
        }

        // /Department/Details/10
        // /Department/Details
        //[HttpGet]
        public IActionResult Details(int? id, string viewName = "Details")
        {
            if(id is null)
            {
                return BadRequest(); // 400
            }

            var department = _unitOfWork.Repository<Department>().Get(id.Value);

            if(department is null)
            {
                return NotFound();  // 404
            }

            return View(viewName,department);
        }

        // /Department/Edit/10
        // /Department/Edit
        //[HttpGet]
        public IActionResult Edit(int? id)
        {
            return Details(id, "Edit");
            ///if (!id.HasValue)
            ///{
            ///    return BadRequest(); // 400
            ///}
            ///var department = _departmentsRebo.Get(id.Value);
            ///if (department is null)
            ///{
            ///    return NotFound(); // 404
            ///}
            ///return View(department);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, Department department)
        {
            if(id != department.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(department);
            }

            try
            {
                _unitOfWork.Repository<Department>().Update(department);
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
                    ModelState.AddModelError(string.Empty, "An Error Has Occured During Updating the Department");
                }

                return View(department);
                
            }
        }

        // /Department/Delete/10
        // /Department/Delete
        //[HttpGet]
        public IActionResult Delete(int? id)
        {
            return Details(id, "Delete");
        }

        [HttpPost]
        public IActionResult Delete(Department department)
        {
            try
            {
                _unitOfWork.Repository<Department>().Delete(department);
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
                    ModelState.AddModelError(string.Empty, "An Error Has Occured During Updating the Department");
                }

                return View(department);
            }
        }
    }
}

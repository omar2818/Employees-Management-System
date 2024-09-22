using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Route.C41.G01.BLL.Interfaces;
using Route.C41.G01.BLL.Repositories;
using Route.C41.G01.DAL.Models;
using Route.C41.G01.PL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Route.C41.G01.PL.Controllers
{
    // Inheritance : DepartmentController is  a Controller
    // Association : DepartmentController has a DepartmentRepository
    [Authorize]
    public class DepartmentController : Controller
    {
        //private readonly IDepartmentRepository _departmentsRebo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;

        public DepartmentController(
            //IDepartmentRepository departmentRepository,
            IUnitOfWork unitOfWork,
            IWebHostEnvironment env,
            IMapper mapper)
        {
            //_departmentsRebo = departmentRepository;
            _unitOfWork = unitOfWork;
            _env = env;
            _mapper = mapper;
        }

        // /Department/Index
        public async Task<IActionResult> Index(string searching)
        {
            //var departments = _unitOfWork.Repository<Department>().GetAllAsync();
            var departments = Enumerable.Empty<Department>();
            var DepartmentRepo = _unitOfWork.Repository<Department>() as DepartmentRepository;
            if (string.IsNullOrEmpty(searching))
            {
                departments = await DepartmentRepo.GetAllAsync();
            }
            else
            {
                departments = await DepartmentRepo.GetDepartmentsByNameAsync(searching);
            }

            var mappedDepartments = _mapper.Map<IEnumerable<Department>, IEnumerable<DepartmentViewModel>>(departments);
            return View(mappedDepartments);
        }
        /*var Employees = Enumerable.Empty<Employee>();
            var employeeRepo = _unitOfWork.Repository<Employee>() as EmployeeRepository;
            if (string.IsNullOrEmpty(searching)) {
                Employees = _unitOfWork.Repository<Employee>().GetAllAsync();
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
        public async Task<IActionResult> Create(DepartmentViewModel department)
        {
            var mappedDep = _mapper.Map<DepartmentViewModel, Department>(department);
            if (ModelState.IsValid) // server side validation
            {
                _unitOfWork.Repository<Department>().Add(mappedDep);
                var count = await _unitOfWork.Complete();
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
        public async Task<IActionResult> Details(int? id, string viewName = "Details")
        {
            if(id is null)
            {
                return BadRequest(); // 400
            }

            var department = await _unitOfWork.Repository<Department>().GetAsync(id.Value);

            if(department is null)
            {
                return NotFound();  // 404
            }
            var mappedDep = _mapper.Map<Department, DepartmentViewModel>(department);
            return View(viewName,mappedDep);
        }

        // /Department/Edit/10
        // /Department/Edit
        //[HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            return await Details(id, "Edit");
            ///if (!id.HasValue)
            ///{
            ///    return BadRequest(); // 400
            ///}
            ///var department = _departmentsRebo.GetAsync(id.Value);
            ///if (department is null)
            ///{
            ///    return NotFound(); // 404
            ///}
            ///return View(department);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, DepartmentViewModel department)
        {
            var mappedDep = _mapper.Map<DepartmentViewModel, Department>(department);
            if (id != mappedDep.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(department);
            }

            try
            {
                _unitOfWork.Repository<Department>().Update(mappedDep);
                await _unitOfWork.Complete();
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
        public  async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(DepartmentViewModel department)
        {
            try
            {
                var mappedDep = _mapper.Map<DepartmentViewModel, Department>(department);
                _unitOfWork.Repository<Department>().Delete(mappedDep);
                await _unitOfWork.Complete();
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

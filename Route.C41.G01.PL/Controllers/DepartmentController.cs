using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Route.C41.G01.BLL.Interfaces;
using Route.C41.G01.BLL.Repositories;
using Route.C41.G01.DAL.Models;
using System;

namespace Route.C41.G01.PL.Controllers
{
    // Inheritance : DepartmentController is  a Controller
    // Association : DepartmentController has a DepartmentRepository
    public class DepartmentController : Controller
    {
        private readonly IDepartmentRepository _departmentsRebo;
        private readonly IWebHostEnvironment _env;

        public DepartmentController(IDepartmentRepository departmentRepository, IWebHostEnvironment env)
        {
            _departmentsRebo = departmentRepository;
            _env = env;
        }

        // /Department/Index
        public IActionResult Index()
        {
            var departments = _departmentsRebo.GetAll();

            return View(departments);
        }

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
                var count = _departmentsRebo.Add(department);
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

            var department = _departmentsRebo.Get(id.Value);

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
                _departmentsRebo.Update(department);
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
    }
}

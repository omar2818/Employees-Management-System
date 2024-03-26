using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Route.C41.G01.BLL.Interfaces;
using Route.C41.G01.DAL.Models;
using System;

namespace Route.C41.G01.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeesRebo;
        private readonly IWebHostEnvironment _env;

        public EmployeeController(IEmployeeRepository employeeRepository, IWebHostEnvironment env)
        {
            _employeesRebo = employeeRepository;
            _env = env;
        }

        // /Employee/Index
        public IActionResult Index()
        {
            // Binding Through View's Dictionary : Transfer Data from Action to View [One Way]

            // 1. ViewData
            ViewData["Message"] = "Hello Data";

            // 2. ViewBag
            ViewBag.Message = "Hello Bag";

            var Employees = _employeesRebo.GetAll();

            return View(Employees);
        }

        // /Employee/Create
        //[HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Employee employee)
        {
            if (ModelState.IsValid) // server side validation
            {
                var count = _employeesRebo.Add(employee);
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(employee);
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

            var employee = _employeesRebo.Get(id.Value);

            if (employee is null)
            {
                return NotFound();  // 404
            }

            return View(viewName, employee);
        }

        // /Employee/Edit/10
        // /Employee/Edit
        //[HttpGet]
        public IActionResult Edit(int? id)
        {
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
        public IActionResult Edit([FromRoute] int id, Employee employee)
        {
            if (id != employee.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(employee);
            }

            try
            {
                _employeesRebo.Update(employee);
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

                return View(employee);

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
        public IActionResult Delete(Employee employee)
        {
            try
            {
                _employeesRebo.Delete(employee);
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

                return View(employee);
            }
        }
    }
}

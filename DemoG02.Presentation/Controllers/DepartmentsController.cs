using DemoG02.BusinessLogic.DataTransferObjects.Departments;
using DemoG02.BusinessLogic.Services.Interfaces;
using DemoG02.Presentation.ViewModels.Departments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DemoG02.Presentation.Controllers
{
    [Authorize]
    public class DepartmentsController : Controller
    {
        private readonly IDepartmentService _departmentService;
        private readonly ILogger<DepartmentsController> _logger;
        private readonly IWebHostEnvironment _env;

        public DepartmentsController(IDepartmentService departmentService,
                                     ILogger<DepartmentsController> logger,
                                     IWebHostEnvironment env)
        {
            _departmentService = departmentService;
            _logger = logger;
            _env = env;
        }

        // Action => Master Action
        // Get: baseUrl/Departments/Index
        [HttpGet]
        public IActionResult Index()
        {
            //ViewData["Message"] = new DepartmentDto() { Name = "Hello from ViewData"};
            //ViewBag.Message = new DepartmentDto() { Name = "Hello from ViewBag" };
            var departments = _departmentService.GetAllDepartments();
            return View(departments);
        }

        #region Create
        // Get: baseUrl/Departments/Create
        // Show the form
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(DepartmentViewModel departmentVM)
        {
            // Server-side Validation
            if (!ModelState.IsValid)
            {
                return View(departmentVM);
            }
            var message = string.Empty;
            try
            {
                var departmentDto = new CreatedDepartmentDto()
                {
                    Code = departmentVM.Code,
                    DateofCreation = departmentVM.DateofCreation,
                    Name = departmentVM.Name,
                    Descripation = departmentVM.Description
                };
                var result = _departmentService.AddDepartment(departmentDto);
                if (result > 0)
                {
                    message = "Department Created Successfully :)";
                }
                else
                {
                    message = "Department Can't be created right now, please try again later :(";
                }
                TempData["Message"] = message;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Log Exception
                _logger.LogError(ex, ex.Message);
                if (_env.IsDevelopment())
                {
                    message = ex.Message;
                    return View(departmentVM);
                }
                else
                {
                    message = "Department Can't be created";
                    return View("Error", message);
                }
            }
        }
        #endregion

        // Get: baseUrl/Departments/Details/{id}
        [HttpGet]
        public IActionResult Details(int? id)
        {
            if(id is null)
            {
                return BadRequest(); // 400
            }
            var department = _departmentService.GetDepartmentById(id.Value);
            if(department is null)
            {
                return NotFound(); // 404
            }
            return View(department);
        }

        #region Edit
        // Get: baseUrl/Departments/Edit/{id}
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id is null)
            {
                return BadRequest(); // 400
            }
            var department = _departmentService.GetDepartmentById(id.Value);
            if (department is null)
            {
                return NotFound(); // 404
            }
            return View(new DepartmentViewModel()
            {
                Name = department.Name,
                Code = department.Code,
                Description = department.Descripation,
                DateofCreation = department.DateofCreation
            });
        }

        [HttpPost]
        public IActionResult Edit([FromRoute] int id, DepartmentViewModel departmentVM)
        {
            if (!ModelState.IsValid)
            {
                return View(departmentVM);
            }
            var message = string.Empty;
            try
            {
                var result = _departmentService.UpdateDepartment(new UpdatedDepartmentDto()
                {
                    Id = id,
                    Code = departmentVM.Code,
                    Name = departmentVM.Name,
                    Description = departmentVM.Description,
                    DateofCreation = departmentVM.DateofCreation
                });
                if (result > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    message = "Department Can't be updated";
                }
            }
            catch (Exception ex)
            {
                message = _env.IsDevelopment() ? ex.Message : "Department Can't be Updated";
            }
            return View(departmentVM);
        }
        #endregion

        #region Delete
        // Get: baseUrl/Departments/Delete/{id}
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id is null)
            {
                return BadRequest(); // 400
            }
            var department = _departmentService.GetDepartmentById(id.Value);
            if (department is null)
            {
                return NotFound(); // 404
            }
            return View(department);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var message = string.Empty;
            try
            {
                var result = _departmentService.DeleteDepartment(id);
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    message = "an error happened when deleting the department";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                message = _env.IsDevelopment() ? ex.Message : "an error happened when deleting the department";
            }
            ModelState.AddModelError(string.Empty, message);
            return View("Index");
        } 
        #endregion
    }
}

using DemoG02.BusinessLogic.DataTransferObjects.Employees;
using DemoG02.BusinessLogic.Services.Interfaces;
using DemoG02.DataAccess.Models.Employees;
using DemoG02.Presentation.ViewModels.Employees;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DemoG02.Presentation.Controllers
{
    [Authorize]
    public class EmployeesController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly ILogger<EmployeesController> _logger;
        private readonly IWebHostEnvironment _env;

        public EmployeesController(IEmployeeService employeeService,
                                   ILogger<EmployeesController> logger,
                                   IWebHostEnvironment env)
        {
            _employeeService = employeeService;
            _logger = logger;
            _env = env;
        }
        // Get: baseUrl/Employees/Index
        public IActionResult Index(string? EmployeeSearchName)
        {
            var employees = _employeeService.GetAllEmployees(EmployeeSearchName);
            return View(employees);
        }

        #region Create
        // Get: baseUrl/Employees/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult Create(EmployeeViewModel employeeVM)
        {
            // Server-Side Validation
            if (ModelState.IsValid)
            {
                try
                {
                    var employeeDto = new CreatedEmployeeDto()
                    {
                        Name = employeeVM.Name,
                        Email = employeeVM.Email,
                        Age = employeeVM.Age,
                        Address = employeeVM.Address,
                        PhoneNumber = employeeVM.PhoneNumber,
                        Salary = employeeVM.Salary,
                        IsActive = employeeVM.IsActive,
                        HiringDate = employeeVM.HiringDate,
                        Gender = employeeVM.Gender,
                        EmployeeType = employeeVM.EmployeeType,
                        DepartmentId = employeeVM.DepartmentId,
                        Image = employeeVM.Image
                    };
                    var result = _employeeService.CreateEmployee(employeeDto);
                    if (result > 0)
                        return RedirectToAction(nameof(Index));
                    else
                        ModelState.AddModelError(string.Empty, "Can't Create Employee Right Now");
                }
                catch (Exception ex)
                {
                    if (_env.IsDevelopment())
                        ModelState.AddModelError(string.Empty, ex.Message);
                    else
                        _logger.LogError(ex.Message);
                }
            }
            return View(employeeVM);
        }
        #endregion

        // Get: baseUrl/Employees/Details/{id}
        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id is null) return BadRequest(); // 400
            var employee = _employeeService.GetEmployeeById(id.Value);
            if (employee is null) return NotFound(); // 404

            return View(employee);
        }

        #region Edit
        // Get: baseUrl/Employees/Edit/{id}
        public IActionResult Edit(int? id)
        {
            if (id is null) return BadRequest(); // 400
            var employee = _employeeService.GetEmployeeById(id.Value);
            if (employee is null) return NotFound(); // 404

            return View(new EmployeeViewModel()
            {
                Name = employee.Name,
                Email = employee.Email,
                Address = employee.Address,
                Age = employee.Age,
                PhoneNumber = employee.PhoneNumber,
                Salary = employee.Salary,
                IsActive = employee.IsActive,
                HiringDate = employee.HiringDate,
                Gender = Enum.Parse<Gender>(employee.Gender),
                EmployeeType = Enum.Parse<EmployeeType>(employee.EmployeeType),
                DepartmentId = employee.DepartmentId
            });
        }

        [HttpPost]
        public IActionResult Edit([FromRoute] int? id, EmployeeViewModel employeeVM)
        {
            if (id is null) return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    var employeeDto = new UpdatedEmployeeDto()
                    {
                        Id = id.Value,
                        Name = employeeVM.Name,
                        Address = employeeVM.Address,
                        Email = employeeVM.Email,
                        Age = employeeVM.Age,
                        Salary = employeeVM.Salary,
                        PhoneNumber = employeeVM.PhoneNumber,
                        IsActive = employeeVM.IsActive,
                        HiringDate = employeeVM.HiringDate,
                        Gender = employeeVM.Gender,
                        EmployeeType = employeeVM.EmployeeType,
                        DepartmentId = employeeVM.DepartmentId
                    };
                    var result = _employeeService.UpdateEmployee(employeeDto);
                    if (result > 0)
                        return RedirectToAction(nameof(Index));
                    else
                        ModelState.AddModelError(string.Empty, "Employee Can't be updated, try again later");
                }
                catch (Exception ex)
                {
                    if (_env.IsDevelopment())
                        ModelState.AddModelError(string.Empty, ex.Message);
                    else
                    {
                        _logger.LogError(ex.Message);
                        return View("Error", ex);
                    }

                }
            }
            return View(employeeVM);
        }
        #endregion

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id is null) return BadRequest(); // 400
            var employee = _employeeService.GetEmployeeById(id.Value);
            if (employee is null) return NotFound(); // 404

            return View(employee);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (id == 0) return BadRequest();
            try
            {
                var result = _employeeService.DeleteEmployee(id);
                if (result)
                    return RedirectToAction(nameof(Index));
                else
                {
                    ModelState.AddModelError(string.Empty, "Department not deleted");
                    return RedirectToAction(nameof(Delete), new { id });
                }
            }catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                if (_env.IsDevelopment())
                    return RedirectToAction(nameof(Index));
                else
                    return View("Error", ex);
            }
        }
    }
}

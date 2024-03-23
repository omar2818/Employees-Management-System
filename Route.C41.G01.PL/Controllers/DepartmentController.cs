using Microsoft.AspNetCore.Mvc;
using Route.C41.G01.BLL.Interfaces;
using Route.C41.G01.BLL.Repositories;
using Route.C41.G01.DAL.Models;

namespace Route.C41.G01.PL.Controllers
{
    // Inheritance : DepartmentController is  a Controller
    // Association : DepartmentController has a DepartmentRepository
    public class DepartmentController : Controller
    {
        private readonly IDepartmentRepository _departmentsRebo;

        public DepartmentController(IDepartmentRepository departmentRepository)
        {
            _departmentsRebo = departmentRepository;
        }

        // /Department/Index
        public IActionResult Index()
        {
            var departments = _departmentsRebo.GetAll();

            return View(departments);
        }

        // /Department/Create
        [HttpGet]
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
        [HttpGet]
        public IActionResult Details(int? id)
        {
            if(id is null)
            {
                return BadRequest();
            }

            var department = _departmentsRebo.Get(id.Value);

            if(department is null)
            {
                return NotFound();
            }

            return View(department);
        }
    }
}

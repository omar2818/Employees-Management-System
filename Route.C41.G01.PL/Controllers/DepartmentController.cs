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
            //var departments = _departmentsRebo.GetAll();
            return View();
        }
    }
}

using System.ComponentModel.DataAnnotations;

namespace DemoG02.Presentation.ViewModels.Departments
{
    public class DepartmentViewModel
    {
        public string Name { get; set; }
        [Range(10, int.MaxValue)]
        public string Code { get; set; }
        public string? Description { get; set; }
        public DateOnly DateofCreation { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Route.C41.G01.DAL.Models
{
    // Model
    public class Department
    {
        public int Id { get; set; }

        //[Required(ErrorMessage = "Code is Required Ya Hamada")]
        public string Code { get; set; }

        //[Required]
        public string Name { get; set; }
        public DateTime DateOfCreation { get; set; }
    }
}

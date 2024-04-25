using System.ComponentModel.DataAnnotations;

namespace Route.C41.G01.PL.ViewModels.User
{
    public class ForgetPasswordViewModel
    {
        [Required]
        [EmailAddress] 
        public string Email { get; set; }
    }
}

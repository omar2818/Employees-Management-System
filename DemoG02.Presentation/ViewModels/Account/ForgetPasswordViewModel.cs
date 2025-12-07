using System.ComponentModel.DataAnnotations;

namespace DemoG02.Presentation.ViewModels.Account
{
    public class ForgetPasswordViewModel
    {
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}

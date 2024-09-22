using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using Route.C41.G01.DAL.Models;
using Route.C41.G01.PL.Services.EmailSender;
using Route.C41.G01.PL.ViewModels.User;
using System.Threading.Tasks;

namespace Route.C41.G01.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;

		public AccountController(IEmailSender emailSender, IConfiguration configuration, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _emailSender = emailSender;
            _configuration = configuration;
			_userManager = userManager;
			_signInManager = signInManager;
		}

        #region Sign Up

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel model)
        {
            if(ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.UserName);

                if (user is null)
                {
                    user = new ApplicationUser()
                    {
                        FName = model.FirstName,
                        LName = model.LastName,
                        UserName = model.UserName,
                        Email = model.Email,
                        IsAgree = model.IsAgree
                    };

                    var Result = await _userManager.CreateAsync(user, model.Password);

                    if(Result.Succeeded)
                    {
                        return RedirectToAction(nameof(SignIn));
                    }

                    foreach(var error in Result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);

					}

                }
                ModelState.AddModelError(string.Empty, "This userName is Already in use for another Account");
                
            }
            return View(model);
        }

        #endregion

        #region Sign In

        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user is not null)
                {
                    var password = await _userManager.CheckPasswordAsync(user, model.Password);
                    if (password)
                    {
                        var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);

                        if(result.IsLockedOut)
                        {
                            ModelState.AddModelError(string.Empty, "Your Account is Locked!!");
                        }

                        if (result.Succeeded)
                        {
                            return RedirectToAction(nameof(HomeController.Index), "Home");
                        }

						if (result.IsNotAllowed)
						{
							ModelState.AddModelError(string.Empty, "Your Account is Not Confirmed Yet!!");
						}

					}
                }
				ModelState.AddModelError(string.Empty, "Invalid Login");
			}
			return View(model);
        }

        #endregion

        public async new Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(SignIn));
        }

        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendResetPasswordEmail(ForgetPasswordViewModel model)
        {
            if(ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user is not null)
                {
                    var resetPasswordToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var PasswordURL = Url.Action("ResetPassword", "Account", new { email = user.Email, token = resetPasswordToken }, "localhost:5001");
                    await _emailSender.SendAsync(
                        from: _configuration["EmailSettings:SenderEmail"],
                        recipients: model.Email,
                        subject: "reset your password",
                        body: PasswordURL
                        );
                    return Redirect(nameof(CheckYourInbox));
                }
                ModelState.AddModelError(string.Empty, "There is No Account With this Email!!");
            }
            return View(model);
        }

        public IActionResult CheckYourInbox()
        {
            return View();
        }
        public IActionResult ResetPassword(string email, string token)
        {
            TempData["Email"] = email;
            TempData["token"] = token;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var email = TempData["Email"] as string;
                var token = TempData["token"] as string;
                var user = await _userManager.FindByNameAsync(email);
                if (user is not null)
                {
                    _userManager.ResetPasswordAsync(user, token, model.NewPassword);
                    return RedirectToAction(nameof(SignIn));
                }
                ModelState.AddModelError(string.Empty, "Url is not valid");
            }
            return View(model);
        }
    }
}

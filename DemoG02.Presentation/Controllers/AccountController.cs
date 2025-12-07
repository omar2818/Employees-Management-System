using DemoG02.DataAccess.Models.Identity;
using DemoG02.Presentation.Utilities;
using DemoG02.Presentation.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DemoG02.Presentation.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager,
                                 SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        #region Register
        // Register
        public IActionResult Register()
        {
            return View();
        }

        // P@ssw0rd
        [HttpPost]
        public IActionResult Register(RegisterViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = _userManager.FindByNameAsync(viewModel.UserName).Result;
                if (user is null)
                {
                    user = new ApplicationUser()
                    {
                        FirstName = viewModel.FirstName,
                        LastName = viewModel.LastName,
                        Email = viewModel.Email,
                        UserName = viewModel.UserName
                    };
                    var result = _userManager.CreateAsync(user, viewModel.Password).Result;
                    if (result.Succeeded)
                    {
                        return RedirectToAction("LogIn");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "This User Name Already exist, Please try another one :(");
                }
            }
            return View(viewModel);
        }
        #endregion

        #region LogIn
        // Login
        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LogIn(LogInViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = _userManager.FindByEmailAsync(viewModel.Email).Result;
                if (user is not null)
                {
                    var flag = _userManager.CheckPasswordAsync(user, viewModel.Password).Result;
                    if (flag)
                    {
                        var result = _signInManager.PasswordSignInAsync(user, viewModel.Password, viewModel.RememberMe, false).Result;
                        if (result.IsLockedOut)
                        {
                            ModelState.AddModelError(string.Empty, "This Account is Locked Out");
                        }
                        if (result.IsNotAllowed)
                        {
                            ModelState.AddModelError(string.Empty, "This Account is Not Allowed to sign in");
                        }
                        if (result.Succeeded)
                        {
                            return RedirectToAction(nameof(HomeController.Index), "Home");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid Email or Password");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "This Email is not Registered");
                }
            }
            return View(viewModel);
        }
        #endregion

        #region LogOut
        [Authorize]
        public IActionResult SignOut()
        {
            _signInManager.SignOutAsync().GetAwaiter().GetResult();
            return RedirectToAction(nameof(LogIn));
        } 
        #endregion

        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SendResetPasswordLink(ForgetPasswordViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = _userManager.FindByEmailAsync(viewModel.Email).Result;
                if (user is not null)
                {
                    var token = _userManager.GeneratePasswordResetTokenAsync(user).Result;
                    var ResetPasswordLink = Url.Action("ResetPassword", "Account", new {email = viewModel.Email, token}, Request.Scheme);
                    // 1. Generate Email
                    // To, Subject, Body
                    var email = new Email()
                    {
                        To = viewModel.Email,
                        Subject = "Reset Password",
                        Body = ResetPasswordLink
                    };
                    // 2. Send Email
                    EmailSettings.SendEmail(email);
                    return RedirectToAction("CheckYourInbox");
                }
            }
            ModelState.AddModelError(string.Empty, "Invalid Operation");
            return View(nameof(ForgetPassword), viewModel);
        }

        public IActionResult CheckYourInbox()
        {
            return View();
        }

        public IActionResult ResetPassword(string email, string token)
        {
            TempData["email"] = email;
            TempData["token"] = token;
            return View();
        }

        [HttpPost]
        public IActionResult ResetPassword(ResetPasswordViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // User
                var email = TempData["email"] as string ?? string.Empty;
                var user = _userManager.FindByEmailAsync(email).Result;
                // Token Done
                var token = TempData["token"] as string ?? string.Empty;
                // New Password Done
                if(user is not null)
                {
                    var result = _userManager.ResetPasswordAsync(user, token, viewModel.Password).Result;
                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(LogIn));
                    }
                    else
                    {
                        foreach(var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
            }
            return View(viewModel);
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}

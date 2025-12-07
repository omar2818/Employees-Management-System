using DemoG02.DataAccess.Models.Identity;
using DemoG02.Presentation.ViewModels.Role;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DemoG02.Presentation.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public RoleController(RoleManager<IdentityRole> roleManager,
                              UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        
        public IActionResult Index(string? searchInput)
        {
            var roles = new List<RoleViewModel>();
            if (string.IsNullOrEmpty(searchInput))
            {
                roles = _roleManager.Roles.Select(R => new RoleViewModel()
                                        {
                                            Id = R.Id,
                                            Name = R.Name
                                        }).ToList();
            }
            else
            {
                roles = _roleManager.Roles.Where(R => R.NormalizedName.Contains(searchInput.ToUpper()))
                                        .Select(R => new RoleViewModel()
                                        {
                                            Id = R.Id,
                                            Name = R.Name
                                        }).ToList();
            }
            return View(roles);
        }

        #region Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(RoleViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var role = new IdentityRole()
                {
                    Name = viewModel.Name
                };
                var result = _roleManager.CreateAsync(role).Result;
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(viewModel);
        } 
        #endregion

        public IActionResult Details(string? id, string viewName = "Details")
        {
            if (id is null) return BadRequest();

            var role = _roleManager.FindByIdAsync(id).Result;
            if(role is null) return NotFound();

            var roleVM = new RoleViewModel()
            {
                Id = role.Id,
                Name = role.Name
            };
            return View(viewName, roleVM);
        }

        #region Edit
        public IActionResult Edit(string? id)
        {
            return Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] string? id, RoleViewModel viewModel)
        {
            if (id is null || id != viewModel.Id) return BadRequest();

            if (ModelState.IsValid)
            {
                var role = _roleManager.FindByIdAsync(id).Result;
                if (role is null) return NotFound();

                role.Name = viewModel.Name;
                var result = _roleManager.UpdateAsync(role).Result;
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(viewModel);
        }
        #endregion

        #region Delete
        public IActionResult Delete(string? id)
        {
            return Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromRoute] string? id, RoleViewModel viewModel)
        {
            if (id is null || id != viewModel.Id) return BadRequest();

            if (ModelState.IsValid)
            {
                var role = _roleManager.FindByIdAsync(id).Result;
                if (role is null) return NotFound();
                var result = _roleManager.DeleteAsync(role).Result;
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(viewModel);
        } 
        #endregion

        public IActionResult AddOrRemoveUsers(string? roleId)
        {
            if(roleId is null) return BadRequest();

            var role = _roleManager.FindByIdAsync(roleId).Result;
            if (role is null) return NotFound();

            ViewData["RoleId"] = roleId;

            var usersInRole = new List<UserInRoleViewModel>();
            var users = _userManager.Users.ToList();
            foreach (var user in users)
            {
                var userInRole = new UserInRoleViewModel()
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };
                if (_userManager.IsInRoleAsync(user, role.Name).Result)
                {
                    userInRole.IsSelected = true;
                }
                else
                {
                    userInRole.IsSelected = false;
                }
                usersInRole.Add(userInRole);
            }
            return View(usersInRole);
        }

        [HttpPost]
        public IActionResult AddOrRemoveUsers(string? roleId, List<UserInRoleViewModel> users)
        {
            if (roleId is null) return BadRequest();

            var role = _roleManager.FindByIdAsync(roleId).Result;
            if (role is null) return NotFound();

            if (ModelState.IsValid)
            {
                bool flag = true;
                IdentityResult result = null;
                foreach (var user in users)
                {
                    var appUser = _userManager.FindByIdAsync(user.UserId).Result;
                    if(appUser is not null)
                    {
                        if(user.IsSelected && !_userManager.IsInRoleAsync(appUser, role.Name).Result)
                        {
                            result = _userManager.AddToRoleAsync(appUser, role.Name).Result;
                        }else if(!user.IsSelected && _userManager.IsInRoleAsync(appUser, role.Name).Result)
                        {
                            result = _userManager.RemoveFromRoleAsync(appUser, role.Name).Result;
                        }
                        if(result is not null && !result.Succeeded)
                        {
                            foreach(var error in result.Errors)
                            {
                                ModelState.AddModelError(string.Empty, error.Description);
                            }
                            flag = false;
                            result = null;
                        }
                    }
                }
                if (flag)
                {
                    return RedirectToAction(nameof(Edit), new {id = roleId});
                }
            }
            return View(users);
        }

    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Route.C41.G01.DAL.Models;
using Route.C41.G01.PL.ViewModels.User;
using System.Linq;
using System.Threading.Tasks;

namespace Route.C41.G01.PL.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string searchInput)
        {
            var users = Enumerable.Empty<UserViewModel>();


            if (string.IsNullOrEmpty(searchInput))
            {
                users = await _userManager.Users.Select(U => new UserViewModel()
                {
                    Id = U.Id,
                    FirstName = U.FName,
                    LastName = U.LName,
                    Email = U.Email,
                    Roles = _userManager.GetRolesAsync(U).Result
                }).ToListAsync();
            }
            else
            {

                users = await _userManager.Users.Where(U => U.Email
                                  .ToLower()
                                  .Contains(searchInput.ToLower()))
                                  .Select(U => new UserViewModel()
                                  {
                                      Id = U.Id,
                                      FirstName = U.FName,
                                      LastName = U.LName,
                                      Email = U.Email,
                                      Roles = _userManager.GetRolesAsync(U).Result
                                  }).ToListAsync();

            }

            return View(users);
        }

        public async Task<IActionResult> Details(string? id, string ViewName = "Details")
        {

            if (id is null)
                return BadRequest(); // 400



            var userFromDb = await _userManager.FindByIdAsync(id);
            if (userFromDb is null)
                return NotFound(); // 404

            var user = new UserViewModel()
            {
                Id = userFromDb.Id,
                FirstName = userFromDb.FName,
                LastName = userFromDb.LName,
                Email = userFromDb.Email,
                Roles = _userManager.GetRolesAsync(userFromDb).Result
            };

            return View(ViewName, user);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string? id)
        {
            return await Details(id, "Edit");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string id, UserViewModel model)
        {
            if (id != model.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {


                var userFromDb = await _userManager.FindByIdAsync(id);
                if (userFromDb is null)
                    return NotFound(); // 404

                userFromDb.FName = model.FirstName;
                userFromDb.LName = model.LastName;
                userFromDb.Email = model.Email;

                await _userManager.UpdateAsync(userFromDb);

                return RedirectToAction(nameof(Index));
            }


            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string? id)
        {
            return await Details(id, "Delete");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Delete([FromRoute] string id, UserViewModel model)
        {
            if (id != model.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                var userFromDb = await _userManager.FindByIdAsync(id);
                if (userFromDb is null)
                    return NotFound(); // 404


                await _userManager.DeleteAsync(userFromDb);

                return RedirectToAction(nameof(Index));
            }


            return View(model);
        }
    }
}

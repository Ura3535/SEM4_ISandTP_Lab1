using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PostOfficeDomain.Model;
using PostOfficeInfrastructure.ViewModel;

namespace LibraryWebApplication.Controllers
{
    public class RolesController : Controller
    {
        RoleManager<IdentityRole> _roleManager;
        UserManager<User> _userManager;
        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        [Authorize(Roles = ("admin"))]
        public IActionResult Index() => View(_roleManager.Roles.ToList());
        [Authorize(Roles = ("admin"))]
        public IActionResult UserList() => View(_userManager.Users.ToList());

        [Authorize(Roles = ("admin"))]
        public async Task<IActionResult> Edit(string userId)
        {
            // отримуємо користувача
            User user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                //список ролей користувача
                var userRoles = await _userManager.GetRolesAsync(user);
                var allRoles = _roleManager.Roles.ToList();
                ChangeRoleViewModel model = new ChangeRoleViewModel
                {
                    UserId = user.Id,
                    UserContactNumber = user.ContactNumber,
                    UserRoles = userRoles,
                    AllRoles = allRoles
                };
                return View(model);
            }

            return NotFound();
        }
        [HttpPost]
        [Authorize(Roles = ("admin"))]
        public async Task<IActionResult> Edit(string userId, List<string> roles)
        {
            // отримуємо користувача
            User user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var allRoles = _roleManager.Roles.ToList();
                var addedRoles = roles.Except(userRoles);
                var removedRoles = userRoles.Except(roles);

                await _userManager.AddToRolesAsync(user, addedRoles);
                await _userManager.RemoveFromRolesAsync(user, removedRoles);

                return RedirectToAction("UserList");
            }

            return NotFound();
        }

    }
}

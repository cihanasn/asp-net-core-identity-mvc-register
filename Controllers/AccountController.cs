using asp_net_core_identity_mvc.Models;
using asp_net_core_identity_mvc.Models.AccountViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace asp_net_core_identity_mvc.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;

        public AccountController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }
            User user = new User();
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Email = request.Email;
            user.UserName = request.Email;

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, err.Code + " " + err.Description);
                }
                return View(request);
            }
            await _userManager.AddToRoleAsync(user, "User");

            return RedirectToAction("Index", "Home");
        }
    }
}

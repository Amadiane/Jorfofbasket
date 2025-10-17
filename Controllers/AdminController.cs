using Jorfofbasket.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Jorfofbasket.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        // GET: Admin/Login
        [HttpGet]
        public IActionResult Login()
        {
            // Ne pas rediriger automatiquement, juste afficher la vue
            return View();
        }

        // POST: Admin/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Veuillez remplir tous les champs correctement.";
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    // Vérifie si l’utilisateur est dans le rôle Admin avant de rediriger
                    if (await _userManager.IsInRoleAsync(user, "Admin"))
                    {
                        return RedirectToAction("DashboardAdmin", "Admin");
                    }
                    else
                    {
                        await _signInManager.SignOutAsync();
                        TempData["ErrorMessage"] = "Vous n'êtes pas autorisé à accéder au panneau Admin.";
                        return View(model);
                    }
                }
            }

            TempData["ErrorMessage"] = "Email ou mot de passe incorrect";
            return View(model);
        }

        // GET: Admin/DashboardAdmin
        [HttpGet]
        public IActionResult DashboardAdmin()
        {
            // Optionnel : Vérifier que l’utilisateur est connecté et admin
            if (!User.Identity.IsAuthenticated || !User.IsInRole("Admin"))
            {
                return RedirectToAction("Login");
            }

            return View();
        }

        // POST: Admin/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WorkSphereHRMS.Data;
using WorkSphereHRMS.ViewModels;

namespace WorkSphereHRMS.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Login
        [HttpGet]
        public IActionResult Login()
        {
            // If already logged in, redirect to Home
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        // POST: Login
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = _context.Users.FirstOrDefault(x =>
                x.Username == model.Username &&
                x.Password == model.Password);

            if (user == null)
            {
                ViewBag.Error = "Invalid Username or Password";
                return View(model);
            }

            // Create Claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username!)
            };

            // Create Identity
            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            // Create Principal
            var principal = new ClaimsPrincipal(identity);

            // Sign In
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal);

            return RedirectToAction("Index", "Home");
        }

        // Logout
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login");
        }
    }
}
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        // ========================= LOGIN (GET) =========================

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Admin") || User.IsInRole("HR"))
                {
                    return RedirectToAction("Index", "Home");
                }
                else if (User.IsInRole("Employee"))
                {
                    return RedirectToAction("Index", "EmployeeDashboard");
                }
            }

            return View();
        }

        // ========================= LOGIN (POST) =========================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var employee = await _context.Employees
                .Include(e => e.Role)
                .FirstOrDefaultAsync(e =>
                    e.Username == model.Username &&
                    e.Password == model.Password &&
                    e.IsActive);

            if (employee == null)
            {
                ViewBag.Error = "Invalid Username or Password";
                return View(model);
            }

            // Create Claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, employee.Username),
                new Claim(ClaimTypes.Role, employee.Role?.RoleName ?? "Employee"),
                new Claim("EmployeeId", employee.EmployeeId.ToString()),
                new Claim("EmployeeCode", employee.EmployeeCode),
                new Claim("Email", employee.Email ?? "")
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

            // Redirect according to Role
            switch (employee.Role?.RoleName)
            {
                case "Admin":
                    return RedirectToAction("Index", "Home");

                case "HR":
                    return RedirectToAction("Index", "Home");

                case "Employee":
                    return RedirectToAction("Index", "EmployeeDashboard");

                default:
                    return RedirectToAction("Login");
            }
        }

        // ========================= LOGOUT =========================

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login");
        }

        // ========================= ACCESS DENIED =========================

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
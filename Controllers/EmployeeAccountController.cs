using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkSphereHRMS.Data;

namespace WorkSphereHRMS.Controllers
{
    [Authorize(Roles = "Employee")]
    public class EmployeeAccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeeAccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET
        public IActionResult ChangePassword()
        {
            return View();
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(string currentPassword,
                                                        string newPassword,
                                                        string confirmPassword)
        {
            int employeeId = Convert.ToInt32(User.FindFirst("EmployeeId")?.Value);

            var employee = await _context.Employees
                .FirstOrDefaultAsync(e => e.EmployeeId == employeeId);

            if (employee == null)
                return NotFound();

            if (employee.Password != currentPassword)
            {
                ModelState.AddModelError("", "Current password is incorrect.");
                return View();
            }

            if (newPassword != confirmPassword)
            {
                ModelState.AddModelError("", "New password and Confirm password do not match.");
                return View();
            }

            employee.Password = newPassword;

            _context.Update(employee);
            await _context.SaveChangesAsync();

            ViewBag.Success = "Password changed successfully.";

            return View();
        }
    }
}
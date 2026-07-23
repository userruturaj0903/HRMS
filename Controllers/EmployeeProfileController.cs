using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WorkSphereHRMS.Data;

namespace WorkSphereHRMS.Controllers
{
    [Authorize(Roles = "Employee")]
    public class EmployeeProfileController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeeProfileController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var employeeIdClaim = User.FindFirst("EmployeeId");

            if (employeeIdClaim == null)
            {
                return RedirectToAction("Login", "Account");
            }

            int employeeId = Convert.ToInt32(employeeIdClaim.Value);

            var employee = await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Role)
                .FirstOrDefaultAsync(e => e.EmployeeId == employeeId);

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

    }
}
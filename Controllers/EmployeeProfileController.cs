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
            int userId = Convert.ToInt32(User.FindFirst("UserId")?.Value);

            var employee = await _context.Employees
                .Include(e => e.Department)
                .FirstOrDefaultAsync(e => e.RoleId == userId);

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }
    }
}
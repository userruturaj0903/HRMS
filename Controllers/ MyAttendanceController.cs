using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WorkSphereHRMS.Data;

namespace WorkSphereHRMS.Controllers
{
    [Authorize(Roles = "Employee")]
    public class MyAttendanceController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MyAttendanceController(ApplicationDbContext context)
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

    var attendance = await _context.Attendances
        .Where(a => a.EmployeeId == employeeId)
        .OrderByDescending(a => a.AttendanceDate)
        .ToListAsync();

    return View(attendance);
}

    }
}
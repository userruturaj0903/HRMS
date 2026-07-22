using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkSphereHRMS.Data;
using WorkSphereHRMS.Models;

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
            // Temporary: Replace 1 with your EmployeeId
            int employeeId = 2;

            var attendance = await _context.Attendances
                .Where(a => a.EmployeeId == employeeId)
                .OrderByDescending(a => a.AttendanceDate)
                .ToListAsync();

            return View(attendance);
        }
    }
}
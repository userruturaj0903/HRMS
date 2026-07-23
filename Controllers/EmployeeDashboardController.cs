using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkSphereHRMS.Data;
using WorkSphereHRMS.Models;

namespace WorkSphereHRMS.Controllers
{
    [Authorize(Roles = "Employee")]
    public class EmployeeDashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeeDashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            int employeeId = Convert.ToInt32(User.FindFirst("EmployeeId")?.Value);

            var todayAttendance = await _context.Attendances
                .FirstOrDefaultAsync(a =>
                    a.EmployeeId == employeeId &&
                    a.AttendanceDate == DateTime.Today);

            ViewBag.TodayAttendance = todayAttendance;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CheckIn()
        {
            int employeeId = Convert.ToInt32(User.FindFirst("EmployeeId")?.Value);

            var attendance = await _context.Attendances
                .FirstOrDefaultAsync(a =>
                    a.EmployeeId == employeeId &&
                    a.AttendanceDate == DateTime.Today);

            if (attendance == null)
            {
                attendance = new Attendance
                {
                    EmployeeId = employeeId,
                    AttendanceDate = DateTime.Today,
                    CheckIn = DateTime.Now.TimeOfDay,
                    Status = "Present"
                };

                _context.Attendances.Add(attendance);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> CheckOut()
        {
            int employeeId = Convert.ToInt32(User.FindFirst("EmployeeId")?.Value);

            var attendance = await _context.Attendances
                .FirstOrDefaultAsync(a =>
                    a.EmployeeId == employeeId &&
                    a.AttendanceDate == DateTime.Today);

            if (attendance != null && attendance.CheckOut == null)
            {
                attendance.CheckOut = DateTime.Now.TimeOfDay;

                _context.Attendances.Update(attendance);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
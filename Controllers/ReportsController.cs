using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkSphereHRMS.Data;

namespace WorkSphereHRMS.Controllers
{
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReportsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Reports Dashboard
        public IActionResult Index()
        {
            return View();
        }

        // Employee Report
        public IActionResult EmployeeReport(string search)
        {
            var employees = _context.Employees
                .Include(e => e.Department)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                employees = employees.Where(e =>
                    e.FirstName.Contains(search) ||
                    e.LastName.Contains(search));
            }

            return View(employees.ToList());
        }

        // Attendance Report
        public IActionResult AttendanceReport(DateTime? fromDate, DateTime? toDate)
        {
            var attendance = _context.Attendances
                .Include(a => a.Employee)
                .AsQueryable();

            if (fromDate.HasValue)
                attendance = attendance.Where(a => a.AttendanceDate >= fromDate);

            if (toDate.HasValue)
                attendance = attendance.Where(a => a.AttendanceDate <= toDate);

            return View(attendance.ToList());
        }

        // Leave Report
        public IActionResult LeaveReport(string status)
        {
            var leaves = _context.LeaveRequests
                .Include(l => l.Employee)
                .Include(l => l.LeaveType)
                .AsQueryable();

            if (!string.IsNullOrEmpty(status))
                leaves = leaves.Where(l => l.Status == status);

            return View(leaves.ToList());
        }

        // Payroll Report
        public IActionResult PayrollReport(string month)
        {
            var payroll = _context.Payrolls
                .Include(p => p.Employee)
                .AsQueryable();

            if (!string.IsNullOrEmpty(month))
                payroll = payroll.Where(p => p.Month == month);

            return View(payroll.ToList());
        }
    }
}
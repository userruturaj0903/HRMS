using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkSphereHRMS.Data;
using WorkSphereHRMS.Models;
using WorkSphereHRMS.ViewModels;

namespace WorkSphereHRMS.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        var dashboard = new DashboardViewModel
        {
            TotalEmployees = _context.Employees.Count(),

            TotalDepartments = _context.Departments.Count(),

            TodayAttendance = _context.Attendances
                .Count(a => a.AttendanceDate == DateTime.Today),

            PendingLeaveRequests = _context.LeaveRequests
                .Count(l => l.Status == "Pending"),

            RecentEmployees = _context.Employees
                .OrderByDescending(e => e.EmployeeId)
                .Take(3)
                .ToList(),

            RecentLeaveRequests = _context.LeaveRequests
                .Include(l => l.Employee)
                .OrderByDescending(l => l.LeaveRequestId)
                .Take(5)
                .ToList()
        };

        return View(dashboard);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        });
    }
}
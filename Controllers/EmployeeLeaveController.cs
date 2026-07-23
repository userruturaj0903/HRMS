using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WorkSphereHRMS.Data;
using WorkSphereHRMS.Models;

namespace WorkSphereHRMS.Controllers
{
    [Authorize(Roles = "Employee")]
    public class EmployeeLeaveController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeeLeaveController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Leave History
        public async Task<IActionResult> Index()
        {
            int employeeId = Convert.ToInt32(User.FindFirst("EmployeeId")?.Value);

            var leaves = await _context.LeaveRequests
                .Include(l => l.LeaveType)
                .Where(l => l.EmployeeId == employeeId)
                .OrderByDescending(l => l.AppliedDate)
                .ToListAsync();

            return View(leaves);
        }

        // GET: Apply Leave
        public async Task<IActionResult> Create()
        {
            ViewBag.LeaveTypes = new SelectList(
                await _context.LeaveTypes.ToListAsync(),
                "LeaveTypeId",
                "LeaveTypeName");

            return View();
        }

        // POST: Apply Leave
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LeaveRequest leaveRequest)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.LeaveTypes = new SelectList(
                    await _context.LeaveTypes.ToListAsync(),
                    "LeaveTypeId",
                    "LeaveTypeName");

                return View(leaveRequest);
            }

            leaveRequest.EmployeeId = Convert.ToInt32(User.FindFirst("EmployeeId")?.Value);
            leaveRequest.Status = "Pending";
            leaveRequest.AppliedDate = DateTime.Now;

            _context.LeaveRequests.Add(leaveRequest);

            await _context.SaveChangesAsync();

            TempData["Success"] = "Leave applied successfully.";

            return RedirectToAction(nameof(Index));
        }
    }
}
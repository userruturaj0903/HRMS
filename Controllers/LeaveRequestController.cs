using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WorkSphereHRMS.Data;
using WorkSphereHRMS.Models;

namespace WorkSphereHRMS.Controllers
{
    public class LeaveRequestController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LeaveRequestController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ===========================
        // Leave Request List
        // ===========================
        public IActionResult Index(string searchString)
        {
            var leaveRequests = _context.LeaveRequests
                .Include(l => l.Employee)
                .Include(l => l.LeaveType)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                leaveRequests = leaveRequests.Where(l =>
                    l.Employee.FirstName.Contains(searchString) ||
                    l.Employee.LastName.Contains(searchString));
            }

            return View(leaveRequests
                .OrderByDescending(l => l.AppliedDate)
                .ToList());
        }

        // ===========================
        // Create
        // ===========================
        public IActionResult Create()
        {
            ViewBag.EmployeeId = new SelectList(_context.Employees,
                "EmployeeId", "FirstName");

            ViewBag.LeaveTypeId = new SelectList(_context.LeaveTypes,
                "LeaveTypeId", "LeaveTypeName");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(LeaveRequest leaveRequest)
        {
            if (ModelState.IsValid)
            {
                leaveRequest.AppliedDate = DateTime.Now;
                leaveRequest.Status = "Pending";

                _context.LeaveRequests.Add(leaveRequest);
                _context.SaveChanges();

                TempData["Success"] = "Leave request submitted successfully.";

                return RedirectToAction(nameof(Index));
            }

            ViewBag.EmployeeId = new SelectList(_context.Employees,
                "EmployeeId", "FirstName", leaveRequest.EmployeeId);

            ViewBag.LeaveTypeId = new SelectList(_context.LeaveTypes,
                "LeaveTypeId", "LeaveTypeName", leaveRequest.LeaveTypeId);

            return View(leaveRequest);
        }

        // ===========================
        // Details
        // ===========================
        public IActionResult Details(int? id)
        {
            if (id == null)
                return NotFound();

            var leave = _context.LeaveRequests
                .Include(l => l.Employee)
                .Include(l => l.LeaveType)
                .FirstOrDefault(l => l.LeaveRequestId == id);

            if (leave == null)
                return NotFound();

            return View(leave);
        }

        // ===========================
        // Edit
        // ===========================
        public IActionResult Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var leave = _context.LeaveRequests.Find(id);

            if (leave == null)
                return NotFound();

            ViewBag.EmployeeId = new SelectList(_context.Employees,
                "EmployeeId", "FirstName", leave.EmployeeId);

            ViewBag.LeaveTypeId = new SelectList(_context.LeaveTypes,
                "LeaveTypeId", "LeaveTypeName", leave.LeaveTypeId);

            return View(leave);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, LeaveRequest leaveRequest)
        {
            if (id != leaveRequest.LeaveRequestId)
                return NotFound();

            if (ModelState.IsValid)
            {
                _context.LeaveRequests.Update(leaveRequest);
                _context.SaveChanges();

                TempData["Success"] = "Leave request updated successfully.";

                return RedirectToAction(nameof(Index));
            }

            ViewBag.EmployeeId = new SelectList(_context.Employees,
                "EmployeeId", "FirstName", leaveRequest.EmployeeId);

            ViewBag.LeaveTypeId = new SelectList(_context.LeaveTypes,
                "LeaveTypeId", "LeaveTypeName", leaveRequest.LeaveTypeId);

            return View(leaveRequest);
        }

        // ===========================
        // Delete
        // ===========================
        public IActionResult Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var leave = _context.LeaveRequests
                .Include(l => l.Employee)
                .Include(l => l.LeaveType)
                .FirstOrDefault(l => l.LeaveRequestId == id);

            if (leave == null)
                return NotFound();

            return View(leave);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var leave = _context.LeaveRequests.Find(id);

            if (leave != null)
            {
                _context.LeaveRequests.Remove(leave);
                _context.SaveChanges();

                TempData["Success"] = "Leave request deleted successfully.";
            }

            return RedirectToAction(nameof(Index));
        }

        // ===========================
        // Approve Leave
        // ===========================
        public IActionResult Approve(int id)
        {
            var leave = _context.LeaveRequests.Find(id);

            if (leave == null)
                return NotFound();

            leave.Status = "Approved";
            _context.SaveChanges();

            TempData["Success"] = "Leave approved successfully.";

            return RedirectToAction(nameof(Index));
        }

        // ===========================
        // Reject Leave
        // ===========================
        public IActionResult Reject(int id)
        {
            var leave = _context.LeaveRequests.Find(id);

            if (leave == null)
                return NotFound();

            leave.Status = "Rejected";
            _context.SaveChanges();

            TempData["Success"] = "Leave rejected successfully.";

            return RedirectToAction(nameof(Index));
        }
    }
}
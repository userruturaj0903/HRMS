using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WorkSphereHRMS.Data;
using WorkSphereHRMS.Models;

namespace WorkSphereHRMS.Controllers
{
    public class AttendanceController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AttendanceController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Attendance List
        public IActionResult Index()
        {
            var attendance = _context.Attendances
                .Include(a => a.Employee)
                .OrderByDescending(a => a.AttendanceDate)
                .ToList();

            return View(attendance);
        }

        // Create
        public IActionResult Create()
        {
            ViewBag.EmployeeId = new SelectList(
                _context.Employees,
                "EmployeeId",
                "FirstName");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Attendance attendance)
        {
            if (ModelState.IsValid)
            {
                _context.Attendances.Add(attendance);
                _context.SaveChanges();

                TempData["Success"] = "Attendance added successfully.";

                return RedirectToAction(nameof(Index));
            }

            ViewBag.EmployeeId = new SelectList(
                _context.Employees,
                "EmployeeId",
                "FirstName",
                attendance.EmployeeId);

            return View(attendance);
        }

        // Details
        public IActionResult Details(int id)
        {
            var attendance = _context.Attendances
                .Include(a => a.Employee)
                .FirstOrDefault(a => a.AttendanceId == id);

            if (attendance == null)
                return NotFound();

            return View(attendance);
        }

        // Edit
        public IActionResult Edit(int id)
        {
            var attendance = _context.Attendances.Find(id);

            if (attendance == null)
                return NotFound();

            ViewBag.EmployeeId = new SelectList(
                _context.Employees,
                "EmployeeId",
                "FirstName",
                attendance.EmployeeId);

            return View(attendance);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Attendance attendance)
        {
            if (id != attendance.AttendanceId)
                return NotFound();

            if (ModelState.IsValid)
            {
                _context.Attendances.Update(attendance);
                _context.SaveChanges();

                TempData["Success"] = "Attendance updated successfully.";

                return RedirectToAction(nameof(Index));
            }

            return View(attendance);
        }

        // Delete
        public IActionResult Delete(int id)
        {
            var attendance = _context.Attendances
                .Include(a => a.Employee)
                .FirstOrDefault(a => a.AttendanceId == id);

            if (attendance == null)
                return NotFound();

            return View(attendance);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var attendance = _context.Attendances.Find(id);

            if (attendance != null)
            {
                _context.Attendances.Remove(attendance);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }

        // Test
        public IActionResult Test()
        {
            return Content("Attendance Controller Working");
        }
    }
}
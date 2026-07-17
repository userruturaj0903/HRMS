using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkSphereHRMS.Data;
using WorkSphereHRMS.Models;

namespace WorkSphereHRMS.Controllers
{
    [Authorize]
    public class DepartmentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DepartmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ==========================
        // Display All Departments
        // ==========================
      public IActionResult Index(string searchString)
{
    var departments = _context.Departments.AsQueryable();

    if (!string.IsNullOrWhiteSpace(searchString))
    {
        departments = departments.Where(d =>
            d.DepartmentName.Contains(searchString));
    }

    return View(departments.ToList());
}
        // ==========================
        // Show Create Form
        // ==========================
        public IActionResult Create()
        {
            return View();
        }

        // ==========================
        // Save New Department
        // ==========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Department department)
        {
            if (ModelState.IsValid)
            {
                _context.Departments.Add(department);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return View(department);
        }

        // ==========================
        // Show Edit Form
        // ==========================
        public IActionResult Edit(int id)
        {
            var department = _context.Departments.Find(id);

            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // ==========================
        // Update Department
        // ==========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Department department)
        {
            if (ModelState.IsValid)
            {
                _context.Departments.Update(department);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return View(department);
        }

        // ==========================
        // Show Delete Confirmation
        // ==========================
        public IActionResult Delete(int id)
        {
            var department = _context.Departments.Find(id);

            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // ==========================
        // Delete Department
        // ==========================
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var department = _context.Departments.Find(id);

            if (department != null)
            {
                _context.Departments.Remove(department);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
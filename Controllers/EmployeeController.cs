using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WorkSphereHRMS.Data;
using WorkSphereHRMS.Models;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace WorkSphereHRMS.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ==========================
        // Display All Employees
        // ==========================
        public IActionResult Index(string searchString)
        {
            var employees = _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Role)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                employees = employees.Where(e =>
                    e.EmployeeCode.Contains(searchString) ||
                    e.FirstName.Contains(searchString) ||
                    e.LastName.Contains(searchString) ||
                    e.Email.Contains(searchString));
            }

            return View(employees.ToList());
        }

        // ==========================
        // Employee Details
        // ==========================
        public IActionResult Details(int id)
        {
            var employee = _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Role)
                .FirstOrDefault(e => e.EmployeeId == id);

            if (employee == null)
                return NotFound();

            return View(employee);
        }

        // ==========================
        // Show Create Form
        // ==========================
        public IActionResult Create()
        {
            ViewBag.Departments = new SelectList(
                _context.Departments,
                "DepartmentId",
                "DepartmentName");

            ViewBag.Roles = new SelectList(
                _context.Roles,
                "RoleId",
                "RoleName");

            return View();
        }

        // ==========================
        // Save Employee
        // ==========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Employee employee, IFormFile? photo)
        {
            if (ModelState.IsValid)
            {
                if (photo != null && photo.Length > 0)
                {
                    string uploadFolder = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot",
                        "uploads");

                    if (!Directory.Exists(uploadFolder))
                    {
                        Directory.CreateDirectory(uploadFolder);
                    }

                    string fileName =
                        Guid.NewGuid().ToString() +
                        Path.GetExtension(photo.FileName);

                    string filePath = Path.Combine(uploadFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        photo.CopyTo(stream);
                    }

                    employee.ProfileImage = fileName;
                }

                _context.Employees.Add(employee);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Departments = new SelectList(
                _context.Departments,
                "DepartmentId",
                "DepartmentName",
                employee.DepartmentId);

            ViewBag.Roles = new SelectList(
                _context.Roles,
                "RoleId",
                "RoleName",
                employee.RoleId);

            return View(employee);
        }
                // ==========================
        // Show Edit Form
        // ==========================
        public IActionResult Edit(int id)
        {
            var employee = _context.Employees.Find(id);

            if (employee == null)
            {
                return NotFound();
            }

            ViewBag.Departments = new SelectList(
                _context.Departments,
                "DepartmentId",
                "DepartmentName",
                employee.DepartmentId);

            ViewBag.Roles = new SelectList(
                _context.Roles,
                "RoleId",
                "RoleName",
                employee.RoleId);

            return View(employee);
        }

        // ==========================
        // Update Employee
        // ==========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Employee employee, IFormFile? photo)
        {
            if (ModelState.IsValid)
            {
                var existingEmployee = _context.Employees
                    .AsNoTracking()
                    .FirstOrDefault(e => e.EmployeeId == employee.EmployeeId);

                if (existingEmployee == null)
                {
                    return NotFound();
                }

                if (photo != null && photo.Length > 0)
                {
                    string uploadFolder = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot",
                        "uploads");

                    if (!Directory.Exists(uploadFolder))
                    {
                        Directory.CreateDirectory(uploadFolder);
                    }

                    // Delete old image if it exists
                    if (!string.IsNullOrEmpty(existingEmployee.ProfileImage))
                    {
                        string oldImagePath = Path.Combine(uploadFolder, existingEmployee.ProfileImage);

                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    string fileName = Guid.NewGuid().ToString() +
                                      Path.GetExtension(photo.FileName);

                    string filePath = Path.Combine(uploadFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        photo.CopyTo(stream);
                    }

                    employee.ProfileImage = fileName;
                }
                else
                {
                    employee.ProfileImage = existingEmployee.ProfileImage;
                }

                _context.Employees.Update(employee);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Departments = new SelectList(
                _context.Departments,
                "DepartmentId",
                "DepartmentName",
                employee.DepartmentId);

            ViewBag.Roles = new SelectList(
                _context.Roles,
                "RoleId",
                "RoleName",
                employee.RoleId);

            return View(employee);
        }

        // ==========================
        // Show Delete Confirmation
        // ==========================
        public IActionResult Delete(int id)
        {
            var employee = _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Role)
                .FirstOrDefault(e => e.EmployeeId == id);

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // ==========================
        // Delete Employee
        // ==========================
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var employee = _context.Employees.Find(id);

            if (employee != null)
            {
                // Delete employee photo if it exists
                if (!string.IsNullOrEmpty(employee.ProfileImage))
                {
                    string imagePath = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot",
                        "uploads",
                        employee.ProfileImage);

                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }

                _context.Employees.Remove(employee);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
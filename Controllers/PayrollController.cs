using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WorkSphereHRMS.Data;
using WorkSphereHRMS.Models;

namespace WorkSphereHRMS.Controllers
{
    public class PayrollController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PayrollController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ===================== INDEX =====================
        public async Task<IActionResult> Index(string searchString)
        {
            var payrolls = _context.Payrolls
                .Include(p => p.Employee)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                payrolls = payrolls.Where(p =>
                    p.Employee.FirstName.Contains(searchString) ||
                    p.Employee.LastName.Contains(searchString));
            }

            return View(await payrolls.ToListAsync());
        }

        // ===================== DETAILS =====================
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var payroll = await _context.Payrolls
                .Include(p => p.Employee)
                .FirstOrDefaultAsync(m => m.PayrollId == id);

            if (payroll == null)
                return NotFound();

            return View(payroll);
        }

        // ===================== CREATE =====================
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
        public async Task<IActionResult> Create(Payroll payroll)
        {
            if (ModelState.IsValid)
            {
                payroll.NetSalary =
                    payroll.BasicSalary
                    + payroll.HRA
                    + payroll.DA
                    + payroll.MedicalAllowance
                    + payroll.TravelAllowance
                    + payroll.Bonus
                    - payroll.PF
                    - payroll.ProfessionalTax
                    - payroll.IncomeTax;

                _context.Add(payroll);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.EmployeeId = new SelectList(
                _context.Employees,
                "EmployeeId",
                "FirstName",
                payroll.EmployeeId);

            return View(payroll);
        }

        // ===================== EDIT =====================
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var payroll = await _context.Payrolls.FindAsync(id);

            if (payroll == null)
                return NotFound();

            ViewBag.EmployeeId = new SelectList(
                _context.Employees,
                "EmployeeId",
                "FirstName",
                payroll.EmployeeId);

            return View(payroll);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Payroll payroll)
        {
            if (id != payroll.PayrollId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    payroll.NetSalary =
                        payroll.BasicSalary
                        + payroll.HRA
                        + payroll.DA
                        + payroll.MedicalAllowance
                        + payroll.TravelAllowance
                        + payroll.Bonus
                        - payroll.PF
                        - payroll.ProfessionalTax
                        - payroll.IncomeTax;

                    _context.Update(payroll);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Payrolls.Any(e => e.PayrollId == payroll.PayrollId))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewBag.EmployeeId = new SelectList(
                _context.Employees,
                "EmployeeId",
                "FirstName",
                payroll.EmployeeId);

            return View(payroll);
        }

        // ===================== DELETE =====================
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var payroll = await _context.Payrolls
                .Include(p => p.Employee)
                .FirstOrDefaultAsync(m => m.PayrollId == id);

            if (payroll == null)
                return NotFound();

            return View(payroll);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var payroll = await _context.Payrolls.FindAsync(id);

            if (payroll != null)
            {
                _context.Payrolls.Remove(payroll);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> SalarySlip(int id)
            {
                var payroll = await _context.Payrolls
                    .Include(p => p.Employee)
                    .ThenInclude(e => e.Department)
                    .FirstOrDefaultAsync(p => p.PayrollId == id);

                if (payroll == null)
                {
                    return NotFound();
                }

                return View(payroll);
            }
    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkSphereHRMS.Data;

namespace WorkSphereHRMS.Controllers
{
    [Authorize(Roles = "Employee")]
    public class EmployeePayrollController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeePayrollController(ApplicationDbContext context)
        {
            _context = context;
        }

        // My Payroll
        public async Task<IActionResult> Index()
        {
            int employeeId = Convert.ToInt32(User.FindFirst("EmployeeId")?.Value);

            var payrolls = await _context.Payrolls
                .Include(p => p.Employee)
                .Where(p => p.EmployeeId == employeeId)
                .OrderByDescending(p => p.Year)
                .ThenByDescending(p => p.Month)
                .ToListAsync();

            return View(payrolls);
        }

        // Salary Slip
        public async Task<IActionResult> SalarySlip(int id)
        {
            int employeeId = Convert.ToInt32(User.FindFirst("EmployeeId")?.Value);

            var payroll = await _context.Payrolls
                .Include(p => p.Employee)
                    .ThenInclude(e => e.Department)
                .FirstOrDefaultAsync(p =>
                    p.PayrollId == id &&
                    p.EmployeeId == employeeId);

            if (payroll == null)
                return NotFound();

            return View(payroll);
        }
    }
}
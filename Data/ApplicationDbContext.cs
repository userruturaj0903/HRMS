using Microsoft.EntityFrameworkCore;
using WorkSphereHRMS.Models;

namespace WorkSphereHRMS.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Department> Departments { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<Attendance> Attendances { get; set; }

        public DbSet<LeaveType> LeaveTypes { get; set; }

        public DbSet<LeaveRequest> LeaveRequests { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<Payroll> Payrolls { get; set; }

        public DbSet<CompanySettings> CompanySettings { get; set; }
    }
}
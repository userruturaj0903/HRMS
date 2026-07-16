using System.ComponentModel.DataAnnotations;

namespace WorkSphereHRMS.Models
{
    public class Department
    {
        [Key]
        public int DepartmentId { get; set; }

        [Required]
        public string DepartmentName { get; set; } = string.Empty;

        public ICollection<Employee>? Employees { get; set; }
    }
}
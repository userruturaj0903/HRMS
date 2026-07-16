using System.ComponentModel.DataAnnotations;

namespace WorkSphereHRMS.Models
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }

        [Required]
        public string RoleName { get; set; } = string.Empty;

        public ICollection<Employee>? Employees { get; set; }
    }
}
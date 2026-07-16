using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkSphereHRMS.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }

        [Required]
        [StringLength(20)]
        public string EmployeeCode { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Phone]
        public string Phone { get; set; } = string.Empty;

        public string? Gender { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [DataType(DataType.Date)]
        public DateTime JoiningDate { get; set; }

        [Required]
        public string Designation { get; set; } = string.Empty;

        [Column(TypeName = "decimal(10,2)")]
        public decimal Salary { get; set; }

        public string? Address { get; set; }

        public string? ProfileImage { get; set; }

        public string Status { get; set; } = "Active";

        // Foreign Keys
        public int DepartmentId { get; set; }

        public int RoleId { get; set; }

        // Navigation Properties
        public Department? Department { get; set; }

        public Role? Role { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkSphereHRMS.Models
{
    [Table("Payroll")]
    public class Payroll
    {
        [Key]
        public int PayrollId { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        [ForeignKey("EmployeeId")]
        public Employee? Employee { get; set; }

        [Required]
        [StringLength(20)]
        public string Month { get; set; } = string.Empty;

        [Required]
        public int Year { get; set; }

        [Required]
        public decimal BasicSalary { get; set; }

        public decimal HRA { get; set; }

        public decimal DA { get; set; }

        public decimal MedicalAllowance { get; set; }

        public decimal TravelAllowance { get; set; }

        public decimal Bonus { get; set; }

        public decimal PF { get; set; }

        public decimal ProfessionalTax { get; set; }

        public decimal IncomeTax { get; set; }

        public decimal NetSalary { get; set; }

        public DateTime PaymentDate { get; set; }

        public string Status { get; set; } = "Pending";
    }
}
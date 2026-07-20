using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkSphereHRMS.Models
{
    [Table("CompanySettings")]
    public class CompanySettings
    {
        [Key]
        public int SettingId { get; set; }

        [Required]
        public string CompanyName { get; set; } = string.Empty;

        public string? CompanyLogo { get; set; }

        public string? Address { get; set; }

        public string? City { get; set; }

        public string? State { get; set; }

        public string? Country { get; set; }

        public string? PinCode { get; set; }

        public string? Phone { get; set; }

        public string? Email { get; set; }

        public string? Website { get; set; }

        public string? FinancialYear { get; set; }

        public TimeSpan? OfficeStart { get; set; }

        public TimeSpan? OfficeEnd { get; set; }

        public int GraceMinutes { get; set; }
    }
}
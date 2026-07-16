using System.ComponentModel.DataAnnotations;

namespace WorkSphereHRMS.Models
{
    public class LeaveType
    {
        [Key]
        public int LeaveTypeId { get; set; }

        [Required]
        public string LeaveTypeName { get; set; } = string.Empty;

        public int TotalDays { get; set; }
    }
}
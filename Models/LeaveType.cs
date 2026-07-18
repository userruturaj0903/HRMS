using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkSphereHRMS.Models
{
      [Table("LeaveType")]
    public class LeaveType
    {
        [Key]
        public int LeaveTypeId { get; set; }

        [Required]
        [StringLength(100)]
        public string LeaveTypeName { get; set; } = string.Empty;

        [StringLength(250)]
        public string? Description { get; set; }

        public int MaxDays { get; set; }
    }
}
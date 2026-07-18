using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkSphereHRMS.Models
{
     [Table("LeaveRequest")]
    public class LeaveRequest
    {
        [Key]
        public int LeaveRequestId { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        [Required]
        public int LeaveTypeId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime FromDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime ToDate { get; set; }

        [Required]
        public string Reason { get; set; } = string.Empty;

        public string Status { get; set; } = "Pending";

        public DateTime AppliedDate { get; set; } = DateTime.Now;

        [ForeignKey("EmployeeId")]
        public Employee? Employee { get; set; }

        [ForeignKey("LeaveTypeId")]
        public LeaveType? LeaveType { get; set; }
    }
}
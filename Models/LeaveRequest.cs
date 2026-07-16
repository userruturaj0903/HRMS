using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkSphereHRMS.Models
{
    public class LeaveRequest
    {
        [Key]
        public int LeaveRequestId { get; set; }

        public int EmployeeId { get; set; }

        public int LeaveTypeId { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string? Reason { get; set; }

        public string Status { get; set; } = "Pending";

        [ForeignKey("EmployeeId")]
        public Employee? Employee { get; set; }

        [ForeignKey("LeaveTypeId")]
        public LeaveType? LeaveType { get; set; }
    }
}
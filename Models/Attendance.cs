using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkSphereHRMS.Models
{
    [Table("Attendance")]
    public class Attendance
    {
        [Key]
        public int AttendanceId { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        [ForeignKey("EmployeeId")]
        public Employee? Employee { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime AttendanceDate { get; set; }

        [DataType(DataType.Time)]
        public TimeSpan? CheckIn { get; set; }

        [DataType(DataType.Time)]
        public TimeSpan? CheckOut { get; set; }

        [Required]
        public string Status { get; set; } = string.Empty;

        public string? Remarks { get; set; }
    }
}
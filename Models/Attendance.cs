using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkSphereHRMS.Models
{
     [Table("Attendance")]
    public class Attendance
    {
        [Key]
        public int AttendanceId { get; set; }

        public int EmployeeId { get; set; }

        [DataType(DataType.Date)]
        public DateTime AttendanceDate { get; set; }

        public TimeSpan? CheckIn { get; set; }

        public TimeSpan? CheckOut { get; set; }

        public string Status { get; set; } = string.Empty;

        [ForeignKey("EmployeeId")]
        public Employee? Employee { get; set; }
    }
}
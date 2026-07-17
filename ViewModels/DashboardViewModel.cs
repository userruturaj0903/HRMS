using WorkSphereHRMS.Models;

namespace WorkSphereHRMS.ViewModels
{
    public class DashboardViewModel
    {
        // Dashboard Cards
        public int TotalEmployees { get; set; }
        public int TotalDepartments { get; set; }
        public int TodayAttendance { get; set; }
        public int PendingLeaveRequests { get; set; }

        // Recent Data
        public List<Employee> RecentEmployees { get; set; } = new();

        public List<LeaveRequest> RecentLeaveRequests { get; set; } = new();
    }
}
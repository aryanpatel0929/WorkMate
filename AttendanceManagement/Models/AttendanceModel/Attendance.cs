namespace AttendanceManagement.Models.AttendanceModel
{
    using System.ComponentModel.DataAnnotations;
    public class Attendance
    {
        [Key]
        public Guid AttendanceID { get; set; }
        [Required]
        public string? EmployeeID { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public bool IsPresent { get; set; }
        [Required]
        public decimal WorkingHours { get; set; }
        public string? Remarks { get; set; }
    }

}

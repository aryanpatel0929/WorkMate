using System.ComponentModel.DataAnnotations;

namespace AttendanceManagement.Models
{
    public class UserAttendanceDetailsModel
    {
        [Required]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,60}$", ErrorMessage = "User Name should be less than 60")]
        public string? UserName { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateOnly AttendanceDate { get; set; }
        [Required]
        public string? Status { get; set; }
        [Range(0,10)]
        public int Overtimehour { get; set; }
    }
}

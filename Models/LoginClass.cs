using System.ComponentModel.DataAnnotations;

namespace AttendanceManagement.Models
{
    public class LoginClass
    {
        [Required]
        [EmailAddress]
        public string? userEmail { get; set; }
        [Required]
        public string? password { get; set; }
    }
}

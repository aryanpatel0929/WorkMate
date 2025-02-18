using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
namespace AttendanceManagement.Models
{
    public class LoginClass
    {
        [Required(ErrorMessage = "Email cant't be blank")]
        [EmailAddress(ErrorMessage = "Email should be in a proper email address format")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }


        [Required(ErrorMessage = "Password can't be blank")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }

    public class RegisterClass
    {
        [Required]
        public string? PersonName { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [Remote(action: "IsEmailAlreadyAvailable", controller: "Account", ErrorMessage = "Email already exists")]
        public string? Email { get; set; }
        [Required]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Invalid Phone Number")]
        public string? Phone { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [MinLength(10, ErrorMessage = "Password should be minimum 10 characters")]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[^\\da-zA-Z]).{8,15}$", ErrorMessage = "Password should contain atleast one uppercase, one lowercase, one digit and one special character")]
        public string? Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [MinLength(10, ErrorMessage = "Password should be minimum 10 characters")]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[^\\da-zA-Z]).{8,15}$", ErrorMessage = "Password should contain atleast one uppercase, one lowercase, one digit and one special character")]
        public string? ConfirmPassword { get; set; }
        public string? Role { get; set; } = "user";
    }
}

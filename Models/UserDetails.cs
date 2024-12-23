using System.ComponentModel.DataAnnotations;

namespace AttendanceManagement.Models
{
    public class UserDetails:IValidatableObject
    {
        [Required]
        public string? UserId { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,60}$", ErrorMessage = "User Name should be less than 60")]
        public string? UserName { get; set; }
        [Required]
        [MaxLength(200)]
        public string? Address { get; set; }
        [Required]
        public string? AadhaarCardNumber { get; set; }
        [Required]
        public string? ProfileImage {  get; set; }
        [Required]
        public string? Gender { get; set; }
        [Required]
        [Range(18,50)]
        public int? Age { get; set; }
        [Required]
        public float? Salary { get; set; }
        [Required]
        public DateOnly? JoiningDate { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (JoiningDate != null && JoiningDate != DateOnly.FromDateTime(DateTime.Now.Date))
            {
                yield return new ValidationResult("Joining date should be the same as today's date.", new[] { nameof(JoiningDate) });
            }
        }
    }
}

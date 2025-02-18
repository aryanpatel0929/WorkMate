using System.ComponentModel.DataAnnotations;

namespace AttendanceManagement.Models.EmployeeModel
{
    public class EmployeeOverview
    {
        [Required(ErrorMessage = "Employee can't be empty")]
        [Key]
        public string? EmployeeID { get; set; }
        [Required(ErrorMessage = "Employee Name can't be empty")]
        public string? EmployeeName { get; set; }
        [Required(ErrorMessage = "Employee Address can't be empty")]
        public string? Address { get; set; }
        [Required]
        public string? AadhaarCardNumber { get; set; }

        public string? ProfileImage { get; set; }
        public string? Gender { get; set; }
        [Required]
        public int? Age { get; set; }
        [Required]
        public decimal Salary { get; set; }
        [Required]
        public DateTime JoiningDate { get; set; }
    }


    public class EmployeeSalaryOverview
    {
        public Guid SalaryId { get; set; }
        public string? EmployeeID { get; set; }
        public int TotalWorkingDays { get; set; }
        public decimal TotalSalary { get; set; }
        public decimal TotalSalaryDue { get; set; }
        public decimal BasicSalary { get; set; }
        public string? BasicSalaryStatus { get; set; }
        public DateTime? CreatedOrUpdatedDate { get; set; }
    }

    public class EmployeeSalaryPayment
    {
        public Guid PaymentId { get; set; }
        public string? EmployeeID { get; set; }
        public decimal PaidAmount { get; set; }
        public string? PaymentMode { get; set; } 
        public DateTime PaymentDate { get; set; }
        public string? PaymentReceivedBy { get; set; }
    }

}

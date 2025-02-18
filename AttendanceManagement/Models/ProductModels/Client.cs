using System.ComponentModel.DataAnnotations;

namespace AttendanceManagement.Models.ProductModels
{
    public class Client
    {
        [Required]
        [Key]
        public int ClientId { get; set; }
        [Required(ErrorMessage = "Client Name can't be empty")]
        public string? ClientName { get; set; }
        [Required(ErrorMessage = "Client Address can't be empty")]
        public string? Address { get; set; }
        [Required(ErrorMessage = "Contact Number can't be empty")]
        public string? ContactNumber { get; set; }
        [Required(ErrorMessage = "Email Address can't be empty")]
        public string? EmailAddress { get; set; }
    }

    public class ClientPayment
    {
        [Required]
        [Key]
        public int PaymentId { get; set; }
        [Required]
        public int ClientId { get; set; }
        [Required]
        public decimal? AmountPaid { get; set; }
        [Required]
        public DateOnly? PaymentDate { get; set; }
        [Required]
        public string? PaymentMode { get; set; }
        [Required]
        public string? Remarks {  get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace AttendanceManagement.Models
{
    public class ProductAmountPaidClass
    {
        [Required]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,100}$", ErrorMessage = "Client Name should be less than 100 and only contain characters")]
        public string? ClientName { get; set; }
        [Required]
        public int? AmountPaid { get; set; }
        [Required]
        public DateOnly? Date { get; set; }
    }

    public class ChatDataofAmountPaid
    {
        public List<string>? ClientName { get; set; }
        public List<int>? AmountPaid { get; set; }
        public int? TotalPendingAmount { get; set; }
    }
}

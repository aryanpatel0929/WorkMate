using System.ComponentModel.DataAnnotations;

namespace AttendanceManagement.Models
{
    public class RegisterProduct : IValidatableObject
    {
        public string? ProductID { get; set; }
        [Required]
        public string? ProductName { get; set; }
        [MaxLength(150)]
        public string? ProductDescription { get; set; }
        [Required]
        public string? ProductType { get; set; }
        [Required]
        public float? ProductSize { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,100}$", ErrorMessage = "Client Name should be less than 100 and only contain characters")]
        public string? ClientName { get; set; }
        [Required]
        public int? Price { get; set; }
        [Required]
        public DateOnly? OrderDate { get; set; }
        [Required]
        public DateOnly? DeliveryDate { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (OrderDate > DeliveryDate)
            {
                yield return new ValidationResult("Order Date and Delivery Date will not be same");
            }
        }
    }

    public class ChartProductByMonth()
    {
        public List<string> MonthName { get; set; }
        public List<int> ProductCount { get; set; }
    }
    public class ChartProductByClient()
    {
        public List<string> ClientName { get; set; }
        public List<int> ProductPriceSum { get; set; }
    }

    public class ProductModel()
    {
        public List<RegisterProduct> Products { get; set; }
        public ChartProductByMonth ChartProductByMonth { get; set; }
        public ChartProductByClient ChartProductByClient { get; set; }
        public ChatDataofAmountPaid chatDataofAmountPaid { get; set; }
    }
}

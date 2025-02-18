using System.ComponentModel.DataAnnotations;

namespace AttendanceManagement.Models.ProductModels
{
    public class Products:IValidatableObject
    {
        [Required]
        [Key]
        public int ProductId { get; set; }
        [Required]
        public string? ProductName { get; set; }
        public string? ProductDescription { get; set; }
        [Required]
        public string? ProductType { get; set; }
        [Required]
        public int ProductSize { get; set; }
        [Required]
        public decimal? Price { get; set; }
        [Required]
        public int? ClientId { get; set; }
        [Required]
        public DateOnly OrderDate { get; set; }
        [Required]
        public DateOnly DeliveryDate { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (OrderDate > DeliveryDate)
            {
                yield return new ValidationResult("Order Date and Delivery Date will not be same");
            }
        }
    }
}

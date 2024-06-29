using System.ComponentModel.DataAnnotations;

namespace ProductManagement.DTOs.OrderItemDTOs
{
    public class OrderItemCreateDto
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "UnitPrice must be greater than zero")]
        public decimal UnitPrice { get; set; }
    }
}

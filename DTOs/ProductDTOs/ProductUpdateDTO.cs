using System.ComponentModel.DataAnnotations;

namespace ProductManagement.DTOs.ProductDTOs
{
    public class ProductUpdateDTO
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero")]
        public decimal Price { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Stock must be a non-negative integer")] 
        public int Stock { get; set; }
    }
}

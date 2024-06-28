using System.ComponentModel.DataAnnotations;

namespace ProductManagement.DTOs.ProductDTOs.CustomerDTOs
{
    public class CustomerCreateDTO
    {
        [Required]
        [StringLength(maximumLength: 50)]
        public string Name { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address format")]
        [StringLength(maximumLength:100)]
        public string? Email { get; set; }
    }
}

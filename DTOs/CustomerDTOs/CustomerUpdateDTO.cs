using System.ComponentModel.DataAnnotations;

namespace ProductManagement.DTOs.CustomerDTOs
{
    public class CustomerUpdateDTO
    {
        [Required]
        [StringLength(maximumLength: 50)]
        public string Name { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address format")]
        [StringLength(maximumLength: 100)]
        public string? Email { get; set; }
    }
}

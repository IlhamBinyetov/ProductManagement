using ProductManagement.DTOs.OrderItemDTOs;
using System.ComponentModel.DataAnnotations;

namespace ProductManagement.DTOs.OrderDTOs
{
    public class OrderUpdateDto
    {
       

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        public List<OrderItemUpdateDto> OrderItems { get; set; }
    }
}

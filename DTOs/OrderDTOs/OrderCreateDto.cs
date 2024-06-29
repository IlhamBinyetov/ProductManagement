using ProductManagement.DTOs.OrderItemDTOs;
using System.ComponentModel.DataAnnotations;

namespace ProductManagement.DTOs.OrderDTOs
{
    public class OrderCreateDto
    {
        [Required]
        public DateTime OrderDate { get; set; }
        public int CustomerId { get; set; }
        public List<OrderItemCreateDto> OrderItems { get; set; }
    }
}

using ProductManagement.DTOs.CustomerDTOs;
using ProductManagement.DTOs.OrderItemDTOs;

namespace ProductManagement.DTOs.OrderDTOs
{
    public class OrderDto
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public CustomerDto Customer { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
    }
}

using ProductManagement.DTOs.ProductDTOs;

namespace ProductManagement.DTOs.OrderItemDTOs
{
    public class OrderItemDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public ProductDto Product { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}

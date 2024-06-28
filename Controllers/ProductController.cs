using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductManagement.Data;
using ProductManagement.DTOs.ProductDTOs;
using ProductManagement.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProductManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
           _context = context;
        }

        [HttpGet("GetAllProducts")]
        [ProducesResponseType(typeof(SysResponse<List<Product>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SysResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetAllProducts()
        {
           
            return Ok(SysResponse.Success(await _context.Products.ToListAsync()));
        }

        
        [HttpGet("GetProduct/{id}")]
        [ProducesResponseType(typeof(SysResponse<Product>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SysResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound(SysResponse.Error(null,"Product not found"));
            }

            return Ok(SysResponse.Success(product));
        }

        
        [HttpPost("AddProduct")]
        [ProducesResponseType(typeof(SysResponse<Product>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(SysResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Product>> AddProduct([FromBody] ProductCreateDTO productDto)
        {

            var product = new Product
            {
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                Stock = productDto.Stock
            };

            try
            {
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                return NotFound(SysResponse.Error(null, "Product cannot created"));
            }
           
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }


        [HttpPut("UpdateProduct/{id}")]
        [ProducesResponseType(typeof(SysResponse<Product>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SysResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductUpdateDTO productDto)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound(SysResponse.Error(product, "Product not found"));
            }
         

            product.Name = productDto.Name;
            product.Description = productDto.Description;
            product.Price = productDto.Price;
            product.Stock = productDto.Stock;

            _context.Entry(product).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound(SysResponse.Error(product, "Cannot update the product"));
            }
            return Ok(SysResponse.Success(product));
        }


        [HttpDelete("DeleteProduct/{id}")]
        [ProducesResponseType(typeof(SysResponse<Product>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SysResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound(SysResponse.Error(product, "Product not found"));
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return Ok(SysResponse.Success(product));
        }


        //private bool ProductExists(int id)
        //{
        //    return _context.Products.Any(e => e.Id == id);
        //}
    }
}

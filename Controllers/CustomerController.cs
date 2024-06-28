using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductManagement.Data;
using ProductManagement.DTOs.ProductDTOs;
using ProductManagement.DTOs.ProductDTOs.CustomerDTOs;
using ProductManagement.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProductManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CustomerController(ApplicationDbContext context)
        {
                _context = context;
        }

        [HttpGet("GetAllCustomers")]
        [ProducesResponseType(typeof(SysResponse<List<Customer>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SysResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetAllCustomers()
        {
            return Ok(SysResponse.Success(await _context.Customers.ToListAsync()));
        }


        [HttpGet("GetCustomer/{id}")]
        [ProducesResponseType(typeof(SysResponse<Customer>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SysResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);

            if (customer == null)
            {
                return NotFound(SysResponse.Error(null, "Customer not found"));
            }

            return Ok(SysResponse.Success(customer));
        }


        [HttpPost("AddCustomer")]
        [ProducesResponseType(typeof(SysResponse<Customer>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(SysResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(SysResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Product>> AddCustomer([FromBody] CustomerCreateDTO customerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(SysResponse.Error(ModelState, "Invalid input"));
            }
            var customer = new Customer
            {
                Name = customerDto.Name,
                Email = customerDto.Email
            };

            try
            {
                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                return NotFound(SysResponse.Error(null, "Customer cannot created"));
            }

            return CreatedAtAction(nameof(GetCustomer), new { id = customer.Id }, customer);
        }


        [HttpPut("UpdateCustomer/{id}")]
        [ProducesResponseType(typeof(SysResponse<Customer>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SysResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(SysResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateCustomer(int id, [FromBody] CustomerUpdateDTO updateDto)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound(SysResponse.Error(customer, "Customer not found"));
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(SysResponse.Error(ModelState, "Invalid input"));
            }


            customer.Name = updateDto.Name;
            customer.Email = updateDto.Email;


            _context.Entry(customer).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound(SysResponse.Error(customer, "Cannot update the customer"));
            }
            return Ok(SysResponse.Success(customer));
        }


        [HttpDelete("DeleteCustomer/{id}")]
        [ProducesResponseType(typeof(SysResponse<Customer>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SysResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound(SysResponse.Error(customer, "Customer not found"));
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            return Ok(SysResponse.Success(customer));
        }
    }
}

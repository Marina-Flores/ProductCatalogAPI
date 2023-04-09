using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using ProductCatalog.Domain.Entities;
using ProductCatalog.Domain.Interfaces;
using System.Data;

namespace ProductCatalog.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {           
            _productService = productService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAllAsync();

            if (!products.Any())
                return NoContent();

            return Ok(products);
        }

        [HttpGet("GetByID/{id}")]
        public async Task<IActionResult> GetByID([FromRoute] int id)
        {
            var product = await _productService.GetByIdAsync(id);

            if(product == null) 
                return NotFound();

            return Ok(product);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] Product product)
        {
            try
            {                
                var updatedProduct = await _productService.UpdateAsync(product);

                return StatusCode(200, "The product was successfuly updated!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occured while updating the product: {ex}");
            }           
        }

        [HttpPost("Insert")]
        public async Task<ActionResult<Product>> Insert([FromBody] Product product)
        {
            try
            {
                var insertedProduct = await _productService.InsertAsync(product);

                return CreatedAtAction(nameof(GetByID), new { id = insertedProduct }, insertedProduct);
            }
            catch (Exception ex)
            {
                return Problem(
                            detail: $"An error ocurred while creating the product: {ex}", 
                            title: "Internal Server Error", 
                            statusCode: 500);
            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<int> Delete([FromRoute] int id)
        {
            return await _productService.DeleteByIdAsync(id); 
        }


    }
}

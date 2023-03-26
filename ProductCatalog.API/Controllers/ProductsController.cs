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
        public async Task<List<Product>> GetAll()
        {
            return await _productService.GetAllAsync();
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
        public async Task<int> Update([FromBody] Product product)
        {
            return await _productService.UpdateAsync(product);
        }

        [HttpPost("Insert")]
        public async Task<int> Insert([FromBody] Product product)
        {
            return await _productService.InsertAsync(product);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<int> Delete([FromRoute] int id)
        {
            return await _productService.DeleteByIdAsync(id); 
        }


    }
}

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

    }
}

using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductCatalog.API.Controllers;
using ProductCatalog.Domain.Entities;
using ProductCatalog.Domain.Interfaces;

namespace ProductCatalog.API.Test
{
    public class ProductsControllerTest
    {
        private readonly Mock<IProductService> _productServiceMock;
        private readonly ProductsController _controller;

        public ProductsControllerTest()
        {
            _productServiceMock = new Mock<IProductService>();
            _controller = new ProductsController( _productServiceMock.Object );
        }

        [Fact]
        public async void GetByID_ReturnsProductWhenProductExist()
        {
            // Arrange
            int productId = 1;
            Product expectedProduct = new Product { ID = productId, Name = "Cookie" };
            _productServiceMock.Setup(x => x.GetByIdAsync(productId)).ReturnsAsync(expectedProduct);

            // Act
            var result = await _controller.GetByID(productId);

            // Assert
            Assert.IsType<OkObjectResult>( result );    

            var okObjectResult = result as OkObjectResult;
            Assert.IsType<Product>( okObjectResult?.Value );
           
            var product = okObjectResult?.Value as Product;
            Assert.Equal(expectedProduct.ID, productId);
        }

        [Fact]
        public async Task GetByID_ReturnsNotFoundWhenProductsDoesNotExist()
        {
            // Arrange
            int productId = 9999;

            // Act
            var result = await _controller.GetByID(productId);

            // Assert
            Assert.IsType<NotFoundResult>( result );
        }
    }
}
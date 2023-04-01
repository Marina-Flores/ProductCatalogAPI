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
        private readonly Mock<List<Product>> _listProductMock;
        private readonly ProductsController _controller;

        public ProductsControllerTest()
        {
            _productServiceMock = new Mock<IProductService>();
            _listProductMock = new Mock<List<Product>>();
            _controller = new ProductsController( _productServiceMock.Object );
        }

        [Fact]
        public async void GetByID_ReturnsProductWhenProductExist()
        {
            // Arrange
            int productId = 1;
            Product expectedProduct = new() { ID = productId, Name = "Cookie" };
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

        [Fact]
        public async Task GetAll_ReturnsNoContentWhenListIsEmpty()
        {
            // Arrange
            var expectedList = new List<Product>();
            _productServiceMock.Setup(x => x.GetAllAsync()).ReturnsAsync(expectedList);

            // Act
            var result = await _controller.GetAll();

            // Assert 
            Assert.IsType<NoContentResult>( result );
        }

        [Fact]
        public async Task GetAll_ReturnsProductsListWhenListIsNotEmpty()
        {
            // Arrange
            var expectedList = _listProductMock.Object;
            _productServiceMock.Setup(x => x.GetAllAsync()).ReturnsAsync(expectedList);

            // Act
            var result = await _controller.GetAll();

            // Assert 
            Assert.IsType<OkObjectResult>( result );

            var okObjectResult = result as OkObjectResult;
            Assert.IsType<List<Product>>( okObjectResult?.Value );

            var productList = okObjectResult?.Value as List<Product>; 
            Assert.NotNull( productList );
            Assert.NotEmpty( productList );        
        }

    }
}
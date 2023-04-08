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
        private readonly Mock<Product> _productMock;
        private readonly ProductsController _controller;

        public ProductsControllerTest()
        {
            _productServiceMock = new Mock<IProductService>();
            _listProductMock = new Mock<List<Product>>();
            _productMock = new Mock<Product>();
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
            var list = new List<Product> { new Product { ID = 1, Name = "Product 1" } };           
            _productServiceMock.Setup(x => x.GetAllAsync()).ReturnsAsync(list);
           
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

        [Fact]
        public async Task Update_ReturnsOkWhenTheProductWasSuccessfullyUpdated()
        {
            // Arrage
            var product = _productMock.Object;

            // Act
            var result = await _controller.Update(product);

            // Assert
            var objectResult = (ObjectResult)result;

            Assert.IsType<ObjectResult>(result);
            Assert.Equal(200, objectResult.StatusCode);
        }

        [Fact]
        public async Task Update_Returns500WhenAnExceptionIsThrown()
        {
            // Arrange 
            _productServiceMock
                     .Setup(x => x.UpdateAsync(It.IsAny<Product>()))
                     .ThrowsAsync(new Exception("Something went wrong."));

            // Act
            var result = await _controller.Update(new Product());

            // Assert
            var objectResult = (ObjectResult)result;

            Assert.IsType<ObjectResult>( result );
            Assert.Equal(500, objectResult.StatusCode);
        }

    }
}
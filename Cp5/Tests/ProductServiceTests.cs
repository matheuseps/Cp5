using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cp5.Data; // Namespace para MongoDbContext
using Cp5.Models; // Namespace para Product
using Cp5.Services; // Namespace para ProductService
using Moq; // Para simulação de dependências
using MongoDB.Driver; // Para IMongoCollection
using Xunit; // Para o framework de testes

namespace Cp5.Tests
{
    public class ProductServiceTests
    {
        private readonly Mock<IMongoCollection<Product>> _mockCollection;
        private readonly Mock<MongoDbContext> _mockContext;
        private readonly ProductService _productService;

        public ProductServiceTests()
        {
            // Cria um mock para a coleção de produtos
            _mockCollection = new Mock<IMongoCollection<Product>>();
            _mockContext = new Mock<MongoDbContext>();
            _mockContext.Setup(c => c.Products).Returns(_mockCollection.Object);
            
            // Instancia o ProductService com o contexto mockado
            _productService = new ProductService(_mockContext.Object);
        }

        [Fact]
        public async Task GetAllProducts_ReturnsAllProducts()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = "1", Name = "Product 1" },
                new Product { Id = "2", Name = "Product 2" }
            };

            // Setup do mock para retornar os produtos
            var cursorMock = new Mock<IAsyncCursor<Product>>();
            cursorMock.SetupSequence(_ => _.MoveNext(It.IsAny<CancellationToken>()))
                .Returns(true)
                .Returns(false);
            cursorMock.SetupGet(_ => _.Current).Returns(products);

            _mockCollection.Setup(c => c.Find(It.IsAny<FilterDefinition<Product>>(), null))
                .Returns(cursorMock.Object);

            // Act
            var result = await _productService.GetAllProducts();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal("Product 1", result[0].Name);
            Assert.Equal("Product 2", result[1].Name);
        }

        [Fact]
        public async Task GetProductById_ReturnsCorrectProduct()
        {
            // Arrange
            var productId = "1";
            var product = new Product { Id = productId, Name = "Product 1" };

            // Setup do mock para retornar um produto específico
            var cursorMock = new Mock<IAsyncCursor<Product>>();
            cursorMock.SetupSequence(_ => _.MoveNext(It.IsAny<CancellationToken>()))
                .Returns(true)
                .Returns(false);
            cursorMock.SetupGet(_ => _.Current).Returns(new List<Product> { product });

            _mockCollection.Setup(c => c.Find(It.IsAny<FilterDefinition<Product>>(), null))
                .Returns(cursorMock.Object);

            // Act
            var result = await _productService.GetProductById(productId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(productId, result.Id);
        }

        [Fact]
        public async Task CreateProduct_InsertsProduct()
        {
            // Arrange
            var product = new Product { Id = "1", Name = "Product 1" };

            // Act
            await _productService.CreateProduct(product);

            // Assert
            _mockCollection.Verify(c => c.InsertOneAsync(product, null, default), Times.Once);
        }

        [Fact]
        public async Task UpdateProduct_UpdatesExistingProduct()
        {
            // Arrange
            var product = new Product { Id = "1", Name = "Updated Product" };

            // Act
            await _productService.UpdateProduct(product);

            // Assert
            _mockCollection.Verify(c => c.ReplaceOneAsync(
                It.IsAny<FilterDefinition<Product>>(),
                product,
                It.IsAny<ReplaceOptions>(),
                default), Times.Once);
        }

        [Fact]
        public async Task DeleteProduct_DeletesProduct()
        {
            // Arrange
            var productId = "1";

            // Act
            await _productService.DeleteProduct(productId);

            // Assert
            _mockCollection.Verify(c => c.DeleteOneAsync(It.IsAny<FilterDefinition<Product>>(), default), Times.Once);
        }
    }
}

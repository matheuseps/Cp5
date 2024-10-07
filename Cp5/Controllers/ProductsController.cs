using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cp5.Models; // Para o seu modelo Product
using Cp5.Services; // Para o seu serviço ProductService

namespace Cp5.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductsController(ProductService productService)
        {
            _productService = productService;
        }

        // GET: /products
        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetAllProducts()
        {
            var products = await _productService.GetAllProducts();
            return Ok(products);
        }

        // GET: /products/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductById(string id)
        {
            var product = await _productService.GetProductById(id);
            if (product == null)
            {
                return NotFound(); // Retorna 404 se o produto não for encontrado
            }
            return Ok(product);
        }

        // POST: /products
        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
        {
            await _productService.CreateProduct(product);
            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product); // Retorna 201 com a URL do novo recurso
        }

        // PUT: /products/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProduct(string id, [FromBody] Product product)
        {
            if (id != product.Id)
            {
                return BadRequest(); // Retorna 400 se o ID no URL não corresponde ao ID do produto
            }

            await _productService.UpdateProduct(product);
            return NoContent(); // Retorna 204 sem conteúdo
        }

        // DELETE: /products/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(string id)
        {
            await _productService.DeleteProduct(id);
            return NoContent(); // Retorna 204 sem conteúdo
        }
    }
}

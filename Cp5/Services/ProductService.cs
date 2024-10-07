using System.Collections.Generic;
using System.Threading.Tasks;
using Cp5.Data; // Para acessar o contexto do MongoDB
using Cp5.Models; // Para acessar o modelo de Produto
using MongoDB.Driver; // Para usar a funcionalidade do MongoDB

namespace Cp5.Services
{
    public class ProductService
    {
        private readonly MongoDbContext _context;

        public ProductService(MongoDbContext context)
        {
            _context = context; // Inicializa o contexto do MongoDB
        }

        // Método para obter todos os produtos
        public async Task<List<Product>> GetAllProducts()
        {
            return await _context.Products.Find(_ => true).ToListAsync(); // Retorna todos os produtos
        }

        // Método para obter um produto por ID
        public async Task<Product> GetProductById(string id)
        {
            return await _context.Products.Find(x => x.Id == id).FirstOrDefaultAsync(); // Retorna um produto com base no ID
        }

        // Método para criar um novo produto
        public async Task CreateProduct(Product product)
        {
            await _context.Products.InsertOneAsync(product); // Insere um novo produto no banco de dados
        }

        // Método para atualizar um produto existente
        public async Task UpdateProduct(Product product)
        {
            await _context.Products.ReplaceOneAsync(x => x.Id == product.Id, product); // Atualiza o produto existente
        }

        // Método para excluir um produto por ID
        public async Task DeleteProduct(string id)
        {
            await _context.Products.DeleteOneAsync(x => x.Id == id); // Exclui um produto com base no ID
        }
    }
}

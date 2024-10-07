using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Cp5.Models
{
    public class Product
    {
        [BsonId] // Indica que esta propriedade é a chave primária
        [BsonRepresentation(BsonType.ObjectId)] // Converte a string em ObjectId
        public string Id { get; set; }

        [BsonRequired] // Indica que o nome é obrigatório
        public string Name { get; set; }

        [BsonRequired]
        public string Description { get; set; }

        [BsonRequired]
        public decimal Price { get; set; }
    }
}

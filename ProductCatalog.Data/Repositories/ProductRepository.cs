using ProductCatalog.Domain.Entities;
using ProductCatalog.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Data;
using Microsoft.Data.SqlClient;
using Dapper;


namespace ProductCatalog.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IDbConnection _db;

        public ProductRepository(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            var connectionString = _configuration.GetConnectionString("ProductCatalog");
            _db = new SqlConnection(connectionString);
        }

        public async Task<int> DeleteByIdAsync(int id)
        {
            using IDbConnection db = _db;

            string query = "DELETE FROM Product WHERE ID = @Id";
            var rowsAffected = await db.ExecuteAsync(query, new { Id = id });

            return rowsAffected;
        }

        public async Task<List<Product>> GetAllAsync()
        {
            using IDbConnection db = _db;

            string query = "SELECT * FROM Product";

            return (await db.QueryAsync<Product>(query)).ToList();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            using IDbConnection db = _db;

            string query = "SELECT * FROM Product WHERE ID = @Id";

            return await db.QueryFirstOrDefaultAsync<Product>(query);
        }

        public async Task<int> InsertAsync(Product product)
        {
            string query = @"INSERT INTO
                                    Product (
                                        Name,
                                        Description,
                                        Price,
                                        Category,
                                        Stock
                                    )
                                VALUES
                                    (
                                        @Name,
                                        @Description,
                                        @Price,
                                        @Category,
                                        @Stock
                                    )";

            var parameters = new
            {
                product.Name,
                product.Description,
                product.Price,
                product.Category,
                product.Stock
            };

            using IDbConnection db = _db;

            return await db.ExecuteAsync(query, parameters);
        }

        public async Task<int> UpdateAsync(Product product)
        {

            string query = @"UPDATE Product SET ";

            if (!string.IsNullOrEmpty(product.Name))
                query += "Name = @Name, ";

            if (!string.IsNullOrEmpty(product.Description))
                query += "Description = @Description, ";
            
            if (product.Price > 0)
                query += "Price = @Price, ";

            if (!string.IsNullOrEmpty(product.Category))
                query += "Category = @Category, ";

            if (product.Stock > 0)
                query += "Stock = @Stock, ";

            query += "UpdatedOn = GETDATE() WHERE ID = @ID";

            var parameters = new
            {
                product.ID,
                product.Name,
                product.Description,
                product.Price,
                product.Category,
                product.Stock
            };

            using IDbConnection db = _db; 
            return await db.ExecuteAsync(query, parameters);
        }
    }
}

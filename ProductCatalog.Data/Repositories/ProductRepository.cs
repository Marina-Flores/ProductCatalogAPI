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
            _configuration = configuration;
            var connectionString = configuration.GetConnectionString("ProductCatalog");
            _db = new SqlConnection(connectionString);
        }

        public Task<int> DeleteByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Product>> GetAllAsync()
        {
            using IDbConnection db = _db;

            string query = "SELECT * FROM Product";

            return (await db.QueryAsync<Product>(query)).ToList();
        }

        public Task<Product> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> InsertAsync(Product product)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAsync(Product product)
        {
            throw new NotImplementedException();
        }
    }
}

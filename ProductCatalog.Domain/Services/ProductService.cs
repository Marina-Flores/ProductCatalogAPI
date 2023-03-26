using ProductCatalog.Domain.Entities;
using ProductCatalog.Domain.Interfaces;

namespace ProductCatalog.Domain.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> DeleteByIdAsync(int id)
            => await _repository.DeleteByIdAsync(id);

        public async Task<List<Product>> GetAllAsync()
            => await _repository.GetAllAsync();

        public async Task<Product> GetByIdAsync(int id)
            => await _repository.GetByIdAsync(id);

        public async Task<int> InsertAsync(Product product)
            => await _repository.InsertAsync(product);

        public async Task<int> UpdateAsync(Product product)
            => await _repository.UpdateAsync(product);
    }
}

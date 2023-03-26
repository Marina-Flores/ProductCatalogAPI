using ProductCatalog.Domain.Entities;

namespace ProductCatalog.Domain.Interfaces
{
    public interface IProductService
    {
        Task<List<Product>> GetAllAsync();
        Task<Product> GetByIdAsync(int id);
        Task<int> InsertAsync(Product product);
        Task<int> UpdateAsync(Product product);
        Task<int> DeleteByIdAsync(int id);
    }
}

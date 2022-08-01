using TransactionAPI.Database.Entities;

namespace TransactionAPI.Database.Repositories
{
    public interface ICategoryRepository
    {
        Task<CategoryEntity> GetAsync(string categoryId);
        Task<List<CategoryEntity>> CreateAll(List<CategoryEntity> entity);
        IQueryable<CategoryEntity> GetCategories();

    }
}
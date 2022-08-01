using System.Text;
using TransactionAPI.Database.Entities;

namespace TransactionAPI.Database.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly PfmDbContext _dbContext;
        public CategoryRepository(PfmDbContext context)
        {
            _dbContext = context;
        }

        public async Task<List<CategoryEntity>> CreateAll(List<CategoryEntity> entityList)
        {
            try
            {
                var selectedAllCodesToInsert = entityList.Select(e => e.Code).Distinct();

                //Check list, delete duplicates. We need last valid
                //Proveri listu obrisi duplikate treba zadnji validan
                
                var duplicateCodeList = entityList.GroupBy(g => g.Code).Where(w => w.Count() > 1).Select(s => s.Key);
                var insertEntityList = entityList.Where(e => !duplicateCodeList.Contains(e.Code)).ToList();
                foreach (var duplicateCode in duplicateCodeList)
                {
                    var validDuplicateEntity = entityList.LastOrDefault(e => e.Code == duplicateCode);
                    insertEntityList.Add(validDuplicateEntity);
                }

                //Check database, delete duplicates
                //Proveri bazu obrisi duplikate;
                var deleteDatabseDuplicateEntityList = _dbContext.Categories.Where(e => selectedAllCodesToInsert.Contains(e.Code));
                _dbContext.Categories.RemoveRange(deleteDatabseDuplicateEntityList);

                //Insert
                await _dbContext.AddRangeAsync(insertEntityList);
                await _dbContext.SaveChangesAsync();

            }
            catch(Exception e)
            {

            }
            return null;
        }

        public Task<CategoryEntity> GetAsync(string categoryId)
        {
            throw new NotImplementedException();
        }

        public IQueryable<CategoryEntity> GetCategories()
        {
            return _dbContext.Categories;
        }
    }
}

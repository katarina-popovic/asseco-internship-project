using TransactionAPI.Database.Entities;

namespace TransactionAPI.Database.Repositories
{
    public class CategorizedTransactionRepository : ICategorizedTransactionRepository
    {
        private readonly PfmDbContext _dbContext;
        public CategorizedTransactionRepository(PfmDbContext context)
        {
            _dbContext = context;
        }
        public async Task<SplitEntity> CreateAll(string transactionId,List<SplitEntity> entityList)
        {
            
            try
            {
                var transactionWithSplits = _dbContext.CategorizedTransactions.Where(t => t.TransactionId == transactionId);
               
                _dbContext.CategorizedTransactions.RemoveRange(transactionWithSplits);
                
                var dummy = _dbContext;
                _dbContext.CategorizedTransactions.AddRange(entityList);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {

            }
            return null;
        }
    }
}

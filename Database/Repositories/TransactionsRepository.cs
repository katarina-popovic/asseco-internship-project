using Microsoft.EntityFrameworkCore;
using TransactionAPI.Database.Entities;

namespace TransactionAPI.Database.Repositories
{
    public class TransactionsRepository: ITransactionsRepository
    {
        private readonly TransactionsDbContext _dbContext;
        public TransactionsRepository(TransactionsDbContext context)
        {
            _dbContext = context;
        }
        public async Task<TransactionEntity> Create(TransactionEntity entity)
        {
            _dbContext.Transactions.Add(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<TransactionEntity> Get(string transactionId)
        {
            return await _dbContext.Transactions.FirstOrDefaultAsync(t => t.Id == transactionId);
        }
    }
}
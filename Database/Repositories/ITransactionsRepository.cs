using TransactionAPI.Database.Entities;

namespace TransactionAPI.Database.Repositories
{
    public interface ITransactionsRepository
    {
        Task<TransactionEntity> Get(string transactionId);
        Task<TransactionEntity> Create(TransactionEntity entity);
    }
}
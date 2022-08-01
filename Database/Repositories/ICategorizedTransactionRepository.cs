using TransactionAPI.Database.Entities;

namespace TransactionAPI.Database.Repositories
{
    public interface ICategorizedTransactionRepository
    {
        Task<SplitEntity> CreateAll(string transactionId, List<SplitEntity> entityList);
    }
}

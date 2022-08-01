using Asseco.Contracts;
using Asseco.Rest.PersonalFinanceManagementAPI.Contracts.V1.DataContracts.HttpParams;
using Asseco.Rest.PersonalFinanceManagementAPI.Contracts.V1.DataContracts.Models;
using Asseco.REST.Contracts.Generics;
using TransactionAPI.Database.Entities;

namespace TransactionAPI.Database.Repositories
{
    public interface ITransactionsRepository
    {
        Task<TransactionEntity> GetAsync(string transactionId);
        Task<TransactionEntity> Create(TransactionEntity entity);
        Task<List<TransactionEntity>> CreateAll(List<TransactionEntity> entity);
        IQueryable<TransactionEntity> GetTransactions();
        Task<TransactionEntity> CategorizedTransaction(TransactionEntity transactionEntity);
        
    }
}
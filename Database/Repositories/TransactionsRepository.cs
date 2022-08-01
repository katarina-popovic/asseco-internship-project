using System.Net;
using Asseco.Contracts;
using Asseco.Rest.PersonalFinanceManagementAPI.Contracts.V1.DataContracts.HttpParams;
using Asseco.Rest.PersonalFinanceManagementAPI.Contracts.V1.DataContracts.Models;
using Asseco.REST.Contracts.Generics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TransactionAPI.Database.Entities;

namespace TransactionAPI.Database.Repositories
{
    public class TransactionsRepository: ITransactionsRepository
    {
        private readonly PfmDbContext _dbContext;
        public TransactionsRepository(PfmDbContext context)
        {
            _dbContext = context;
        }

        public async Task<TransactionEntity> CategorizedTransaction(TransactionEntity transactionEntity)
        {
            var deleteDatabseDuplicateEntityList = _dbContext.Transactions.Where(e => e.Id == transactionEntity.Id);
            _dbContext.Transactions.Remove(deleteDatabseDuplicateEntityList.First());
            _dbContext.Transactions.Add(transactionEntity);
            await _dbContext.SaveChangesAsync();
            
            return null;
        }

        public async Task<TransactionEntity> Create(TransactionEntity entity)
        {
            try
            {
                var dummy = _dbContext;
                _dbContext.Transactions.Add(entity);
                await _dbContext.SaveChangesAsync();
            }catch(Exception e)
            {

            }
            return entity;
        }

        public async Task<List<TransactionEntity>> CreateAll(List<TransactionEntity> entity)
        {
            try
            {
                var dummy = _dbContext;
                _dbContext.Transactions.AddRange(entity);

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {

            }
            return entity;
        }

        public async Task<TransactionEntity> GetAsync(string transactionId)
        {
            return await _dbContext.Transactions.FirstOrDefaultAsync(t => t.Id == transactionId);
        }

        public IQueryable<TransactionEntity> GetTransactions()
        {
            return _dbContext.Transactions.Include(e => e.SplitTransactionList);
        }

    }
}
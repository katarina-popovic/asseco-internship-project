using System.Net;
using Asseco.Contracts;
using Asseco.Rest.PersonalFinanceManagementAPI.Contracts.V1.DataContracts.HttpParams;
using Asseco.Rest.PersonalFinanceManagementAPI.Contracts.V1.DataContracts.Models;
using Asseco.Rest.PersonalFinanceManagementAPI.Contracts.V1.ServiceContracts;
using Asseco.REST.Contracts.Generics;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TransactionAPI.Command;
using TransactionAPI.Database.Entities;
using TransactionAPI.Database.Repositories;
using System.Linq;
using Asseco.Persistence.Extensions;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel;
using System.Linq.Expressions;
using TransactionAPI.Extensions;

namespace TransactionAPI.Implementations
{
    public class PersonalFinanceManagementAPITransactionsQueryServiceEF : IPersonalFinanceManagementAPITransactionsQueryService
    {
        private readonly ITransactionsRepository _transactionsRepository;
        private readonly IMapper _mapper;

        public PersonalFinanceManagementAPITransactionsQueryServiceEF(ITransactionsRepository transactionsRepository, IMapper mapper)
        {
            _transactionsRepository = transactionsRepository;
            _mapper = mapper;
        }

        public async Task<TransactionPagedList> CreateTransaction(CreateTransactionCommand command)
        {
            var entity = _mapper.Map<TransactionEntity>(command);
            var existingTransaction = await _transactionsRepository.GetAsync(command.Id);

            if (existingTransaction != null)
            {
                return null;
            }
            var result = _transactionsRepository.Create(entity);

            return _mapper.Map<TransactionPagedList>(result);
        }

        public async Task<Result<TransactionPagedList>> GetTransaction(string transactionId)
        {
            var transactionEntity = await _transactionsRepository.GetAsync(transactionId);

            if (transactionEntity == null)
            {
                return new Result<TransactionPagedList>() { StatusCodeResponse = new NotFoundResult(), StatusCode = (int)HttpStatusCode.NotFound };
            }

            var res = _mapper.Map<TransactionPagedList>(transactionEntity);

            return await Task.FromResult(new Result<TransactionPagedList>()
            {
                StatusCodeResponse = new OkObjectResult(res),
                StatusCode = (int)HttpStatusCode.OK,
                Response = res
            });
        }

        public async Task<Result<TransactionPagedList>> TransactionsGetListAsync(TransactionsGetListHttpParams transactionsGetListHttpParams, Paging paging, Sorting sorting)
        {
            var transactionIQueryable = _transactionsRepository.GetTransactions();
            var _transaction = new Transaction();

            //Filtriranje
            if (!string.IsNullOrEmpty(transactionsGetListHttpParams.TransactionKind)) {
                transactionIQueryable = transactionIQueryable.Where(e => e.Kind == transactionsGetListHttpParams.TransactionKind);
            }
            if (transactionsGetListHttpParams.StartDate != null)
            {
                transactionIQueryable = transactionIQueryable.Where(e => e.Date.ToUniversalTime() >= transactionsGetListHttpParams.StartDate.Value.ToUniversalTime());
            }
            if (transactionsGetListHttpParams.EndDate != null)
            {
                transactionIQueryable = transactionIQueryable.Where(e => e.Date.ToUniversalTime() <= transactionsGetListHttpParams.EndDate.Value.ToUniversalTime());
            }

            //Sort
            var column = sorting.SortType;
            var order = sorting.SortOrder;
            
            if (order == "asc")
            {
                transactionIQueryable = transactionIQueryable.SortFunc(column);
            }else if(order == "desc")
            {
                transactionIQueryable = transactionIQueryable.SortFuncDesc(column);
            }

            //Paging
            var pageCount = Math.Ceiling(transactionIQueryable.Count() / (float)paging.PageSize);
            
            var trIQuery = await transactionIQueryable.Skip((paging.PageNumber - 1) * paging.PageSize).Take(paging.PageSize).ToListAsync();
            var count = transactionIQueryable.Count();
            var result = new TransactionPagedList()
            {
                Items = _mapper.Map<List<TransactionWithSplits>>(trIQuery),
                PageSize = paging.PageSize,
                Page = paging.PageNumber,
                TotalCount = count,
                TotalPages = (int?)Math.Ceiling((double)count/paging.PageSize),
                SortOrder = order,
                SortBy = column
            };
            
            return new Result<TransactionPagedList>()
            {
                StatusCodeResponse = new OkObjectResult(result),
                StatusCode = (int)HttpStatusCode.OK,
                Response = result
            };
        }
    }
}

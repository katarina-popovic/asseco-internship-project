using System.Net;
using Asseco.Contracts;
using Asseco.Rest.PersonalFinanceManagementAPI.Contracts.V1.DataContracts.HttpParams;
using Asseco.Rest.PersonalFinanceManagementAPI.Contracts.V1.DataContracts.Models;
using Asseco.Rest.PersonalFinanceManagementAPI.Contracts.V1.ServiceContracts;
using Asseco.REST.Contracts.Generics;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TransactionAPI.Database.Entities;
using TransactionAPI.Database.Repositories;

namespace BankProject.Implementations.PersonalFinanceManagementApiTransactionQueryServiceMock{
    public class PersonalFinanceManagementAPITransactionsQueryServiceMock : IPersonalFinanceManagementAPITransactionsQueryService
    {
        private readonly ITransactionsRepository _transactionsRepository;
        private readonly IMapper _mapper;

        public PersonalFinanceManagementAPITransactionsQueryServiceMock(ITransactionsRepository transactionsRepository, IMapper mapper){
            _transactionsRepository = transactionsRepository;
            _mapper = mapper;
        }

        public async Task<Transaction> CreateTransaction(CreateTransactionCommand command){
            var entity = _mapper.Map<TransactionEntity>(command);
            var existingTransaction = await _transactionsRepository.Get(command.TransactionId);

            if(existingTransaction != null){
                return null;
            }
            var result = _transactionsRepository.Create(entity);

            return _mapper.Map<Transaction>(result);
        }
        public Task<Result<TransactionPagedList>> TransactionsGetListAsync(TransactionsGetListHttpParams transactionsGetListHttpParams, Paging paging, Sorting sorting)
        {
            var transactionSplitsList = new List<TransactionWithSplits>();

            transactionSplitsList.Add(new TransactionWithSplits{
                Id = "12345",
                BeneficiaryName = "Mobile",
                Date = "01.02.2020",
                Direction = "d",
                Kind = "dep"
            });

            var transactionPagedListRes = Task.FromResult(new TransactionPagedList{
                Items = transactionSplitsList,
                PageSize = 10,
                TotalPages = 14,
                TotalCount = 8
            });

            var transactionPagedList = new TransactionPagedList{
                Items = transactionSplitsList,
                PageSize = 10,
                TotalPages = 14,
                TotalCount = 8
            };

            return Task.FromResult(new Result<TransactionPagedList>() { StatusCodeResponse = new OkObjectResult(transactionPagedList),
            StatusCode = (int)HttpStatusCode.OK, Response = transactionPagedList});

        }
    }

}
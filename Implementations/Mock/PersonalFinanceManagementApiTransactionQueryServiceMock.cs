using System.Net;
using Asseco.Contracts;
using Asseco.Rest.PersonalFinanceManagementAPI.Contracts.V1.DataContracts.HttpParams;
using Asseco.Rest.PersonalFinanceManagementAPI.Contracts.V1.DataContracts.Models;
using Asseco.Rest.PersonalFinanceManagementAPI.Contracts.V1.ServiceContracts;
using Asseco.REST.Contracts.Generics;
using Microsoft.AspNetCore.Mvc;
using TransactionAPI.Command;

namespace BankProject.Implementations.PersonalFinanceManagementApiTransactionQueryServiceMock
{
    public class PersonalFinanceManagementAPITransactionsQueryServiceMock : IPersonalFinanceManagementAPITransactionsQueryService
    {
        public Task<TransactionPagedList> CreateTransaction(CreateTransactionCommand command)
        {
            throw new NotImplementedException();
        }

        public Task<Result<TransactionPagedList>> GetTransaction(string transactionId)
        {
            throw new NotImplementedException();
        }

        public Task<Result<TransactionPagedList>> TransactionsGetListAsync(TransactionsGetListHttpParams transactionsGetListHttpParams, Paging paging, Sorting sorting)
        {
            var transactionSplitsList = new List<TransactionWithSplits>();

            transactionSplitsList.Add(new TransactionWithSplits
            {
                Id = "12345",
                BeneficiaryName = "Mobile",
                Date = "01.02.2020",
                Direction = "d",
                Kind = "dep"
            });

            var transactionPagedListRes = Task.FromResult(new TransactionPagedList
            {
                Items = transactionSplitsList,
                PageSize = 10,
                TotalPages = 14,
                TotalCount = 8
            });

            var transactionPagedList = new TransactionPagedList
            {
                Items = transactionSplitsList,
                PageSize = 10,
                TotalPages = 14,
                TotalCount = 8
            };

            return Task.FromResult(new Result<TransactionPagedList>()
            {
                StatusCodeResponse = new OkObjectResult(transactionPagedList),
                StatusCode = (int)HttpStatusCode.OK,
                Response = transactionPagedList
            });

        }
    }

}
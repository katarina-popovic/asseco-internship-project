using Asseco.Contracts.Errors;
using Asseco.Rest.PersonalFinanceManagementAPI.Contracts.V1.DataContracts.HttpParams;
using Asseco.Rest.PersonalFinanceManagementAPI.Contracts.V1.DataContracts.Models;
using Asseco.Rest.PersonalFinanceManagementAPI.Contracts.V1.ServiceContracts;
using Asseco.REST.Contracts.Generics;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TransactionAPI.Database.Repositories;

namespace TransactionAPI.Implementations
{
    public class PersonalFinanceManagementAPIAnalyticsQueryServiceEF : IPersonalFinanceManagementAPIAnalyticsQueryService
    {
        private readonly ITransactionsRepository _transactionRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public PersonalFinanceManagementAPIAnalyticsQueryServiceEF(ITransactionsRepository transactionRepository, ICategoryRepository categoryRepository , IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }
        public async Task<Result<SpendingsByCategory>> SpendingsGetAsync(SpendingsGetHttpParams spendingsGetHttpParams)
        {

            //get transaction with required params
            var transactions = _transactionRepository.GetTransactions().Where(w => !string.IsNullOrEmpty(w.CatCode));
            var categories = _categoryRepository.GetCategories();

            //validate params
            if (spendingsGetHttpParams.Catcode != null) {
                transactions = transactions.Where(t => t.CatCode == spendingsGetHttpParams.Catcode 
                    || categories.Where(e => e.ParentCode == spendingsGetHttpParams.Catcode).Select(s => s.Code).Contains(t.CatCode));
            }
            if (spendingsGetHttpParams.Direction != null) {
                transactions = transactions.Where(t => t.Direction == spendingsGetHttpParams.Direction[0]);
            }

            var start = DateTime.Now;
            var end = DateTime.Now;
            if (spendingsGetHttpParams.StartDate != null) {
                start = (DateTime)spendingsGetHttpParams.StartDate;
                transactions = transactions.Where(t => t.Date.ToUniversalTime() >= start.ToUniversalTime());
            }
            if (spendingsGetHttpParams.EndDate != null) {
                end = (DateTime)spendingsGetHttpParams.EndDate;
                transactions = transactions.Where(t => t.Date.ToUniversalTime() <= end.ToUniversalTime());
            }

            //if get transactions failed then error
            var spendingTransactions = transactions.ToList();
            if (spendingTransactions.Count() == 0) {
                return new Result<SpendingsByCategory>()
                {
                    StatusCodeResponse = new NotFoundObjectResult(new ValidationError {
                        Error = "not-found-categorized-transactions",
                        Message = "not found categorized transactions",
                        Tag = "not-found-categorized-transactions"
                    }),
                    StatusCode = (int)HttpStatusCode.NotFound
                };
            }

            var spendings = spendingTransactions.GroupBy(s => s.CatCode).Select(s => new SpendingInCategory
            {
                Catcode = s.Key,
                Amount = s.Sum(e => e.Amount),
                Count = s.Count()
            }).ToList();

            var spendingList = new SpendingsByCategory
            {
                Groups = spendings
            };

            return new Result<SpendingsByCategory>()
            {
                StatusCodeResponse = new OkObjectResult(spendingList),
                StatusCode = (int)HttpStatusCode.OK,
                Response = spendingList
            };
        }
    }
}

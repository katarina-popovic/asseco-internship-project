using Asseco.Contracts.Errors;
using Asseco.Rest.PersonalFinanceManagementAPI.Contracts.V1.DataContracts.HttpParams;
using Asseco.Rest.PersonalFinanceManagementAPI.Contracts.V1.DataContracts.Models;
using Asseco.Rest.PersonalFinanceManagementAPI.Contracts.V1.ServiceContracts;
using Asseco.REST.Contracts.Generics;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TransactionAPI.Database.Entities;
using TransactionAPI.Database.Repositories;

namespace TransactionAPI.Implementations
{
    public class PersonalFinanceManagementAPICategoriesQueryServiceEF : IPersonalFinanceManagementAPICategoriesQueryService
    {
        private readonly ICategorizedTransactionRepository _categorizedTransactionRepository;
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ITransactionsRepository _transactionRepository;

        public PersonalFinanceManagementAPICategoriesQueryServiceEF(ICategorizedTransactionRepository categorizedTransactionRepository, IMapper mapper, ICategoryRepository categoryRepository, ITransactionsRepository transactionRepository)
        {
            _categorizedTransactionRepository = categorizedTransactionRepository;
            _mapper = mapper;
            _categoryRepository = categoryRepository;
            _transactionRepository = transactionRepository;
        }
        public async Task<Result<IActionResult>> TransactionsCategorizeAsync(TransactionsCategorizeHttpParams transactionsCategorizeHttpParams, TransactionCategorizeCommand transactionCategorizeCommand)
        {

            var transactions = _transactionRepository.GetTransactions();
            var categories = _categoryRepository.GetCategories();
            
            var tr = transactions.FirstOrDefault(t => t.Id == transactionsCategorizeHttpParams.Id);
            var cat = categories.FirstOrDefault(c => c.Code == transactionCategorizeCommand.Catcode);
            if (tr == null) {
                return await Task.FromResult(new Result<IActionResult>()
                {
                    StatusCodeResponse = new NotFoundObjectResult(new ValidationError
                    {
                        Error = "transaction-not-found",
                        Message = "transaction not found",
                        Tag = $"{transactionsCategorizeHttpParams.Id}"
                    }),
                    StatusCode = (int)HttpStatusCode.NotFound
                });
            }

            if (cat == null)
            {
                return await Task.FromResult(new Result<IActionResult>()
                {
                    StatusCodeResponse = new NotFoundObjectResult(new ValidationError
                    {
                        Error = "category-not-found",
                        Message = "category not found",
                        Tag = $"{transactionCategorizeCommand.Catcode}"
                    }),
                    StatusCode = (int)HttpStatusCode.NotFound
                });
            }

            var updatedCategory = new TransactionEntity
            {
                Id = transactionsCategorizeHttpParams.Id,
                BeneficiaryName = tr.BeneficiaryName,
                Date = tr.Date,
                Direction = tr.Direction,
                Amount = tr.Amount,
                Description = tr.Description,
                Currency = tr.Currency,
                Mcc = tr.Mcc,
                Kind = tr.Kind,
                CatCode = cat.Code
            };
            await _transactionRepository.CategorizedTransaction(updatedCategory);
            
            return await Task.FromResult(new Result<IActionResult>()
            {
                StatusCodeResponse = new OkResult(),
                StatusCode = (int)HttpStatusCode.OK
            });
        }
    }
}

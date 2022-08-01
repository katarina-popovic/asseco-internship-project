using Asseco.Contracts.Errors;
using Asseco.Rest.PersonalFinanceManagementAPI.Contracts.V1.DataContracts.HttpParams;
using Asseco.Rest.PersonalFinanceManagementAPI.Contracts.V1.DataContracts.Models;
using Asseco.Rest.PersonalFinanceManagementAPI.Contracts.V1.ServiceContracts;
using Asseco.REST.Contracts.Generics;
using AutoMapper;
using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using TransactionAPI.Database.Entities;
using TransactionAPI.Database.Repositories;

namespace TransactionAPI.Implementations
{
    public class PersonalFinanceManagementAPITransactionsCommandServiceEF : IPersonalFinanceManagementAPITransactionsCommandService
    {
        private readonly ITransactionsRepository _transactionsRepository;
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICategorizedTransactionRepository _categorizedTransactionRepository;

        public PersonalFinanceManagementAPITransactionsCommandServiceEF(ITransactionsRepository transactionsRepository, IMapper mapper,
            ICategoryRepository categoryRepository, ICategorizedTransactionRepository categorizedTransactionRepository)
        {
            _transactionsRepository = transactionsRepository;
            _mapper = mapper;
            _categoryRepository = categoryRepository;
            _categorizedTransactionRepository = categorizedTransactionRepository;
        }
        int i = 0;
        public async Task<Result<IActionResult>> TransactionsImportAsync(TransactionsImportHttpParams transactionsImportHttpParams, IFormFile file)
        { 
            var fileOpen = file.OpenReadStream();
            StreamReader sr = new StreamReader(fileOpen);
            string line;
            string[] row;
            sr.ReadLine();

            List<TransactionEntity> listEntities = new List<TransactionEntity>();
            List<ValidationError> errorsList = new List<ValidationError>();

            var i = 2;
            while ((line = sr.ReadLine()) != null) 
            {
                row = line.Split(',');

                if (row.Length > 9)
                {
                    errorsList.Add(new ValidationError { Error = "length-line-out-of-range", Message = "Length line is out of range", Tag = $"row[{i}]" });
                    
                }
                if (row[0].Length > 10)
                {
                    errorsList.Add(new ValidationError { Error = "length-line-out-of-range", Message = "Length of Id is out of range", Tag = $"row[{i}].Id" });
                    
                }
                if (row[1].Length > 30)
                {
                    errorsList.Add(new ValidationError { Error = "length-line-out-of-range", Message = "Length of beneficiary-name is out of range", Tag = $"row[{i}].BeneficiaryName" });
                    
                }
                
                if (row[3].Length > 1)
                {
                    errorsList.Add(new ValidationError { Error = "length-line-out-of-range", Message = "Length of direction is out of range", Tag = $"row[{i}].Direction" });
                    
                }
                if (row[5].Length > 30)
                {
                    errorsList.Add(new ValidationError { Error = "length-line-out-of-range", Message = "Length of description is out of range", Tag = $"row[{i}].Description" });
                    
                }
                if (row[6].Length > 5)
                {
                    errorsList.Add(new ValidationError { Error = "length-line-out-of-range", Message = "Length of currency is out of range", Tag = $"row[{i}].Currency" });
                    
                }
                if (row[8].Length > 5)
                {
                    errorsList.Add(new ValidationError { Error = "length-line-out-of-range", Message = "Length of kind is out of range", Tag = $"row[{i}].Kind" });
                    
                }
                
                    double parseDouble = default;
                    double.TryParse(row[4], out parseDouble);

                    MccCodeEnum enumParse = default;
                    Enum.TryParse<MccCodeEnum>(row[7], out enumParse);

                    listEntities.Add(new TransactionEntity
                    {
                        Id = row[0],
                        BeneficiaryName = !string.IsNullOrEmpty(row[1]) ? row[1] : "",
                        Date = DateTime.Parse(!string.IsNullOrEmpty(row[2]) ? row[2] : "").ToUniversalTime(),
                        Direction = (!string.IsNullOrEmpty(row[3]) ? row[3] : "")[0],
                        Amount = parseDouble,
                        Description = !string.IsNullOrEmpty(row[5]) ? row[5] : "",
                        Currency = !string.IsNullOrEmpty(row[6]) ? row[6] : "",
                        Mcc = enumParse,
                        Kind = !string.IsNullOrEmpty(row[8]) ? row[8] : ""
                    });
                
                i++;
            }
            if (errorsList != null && errorsList.Count() > 0)
            {
                return await Task.FromResult(new Result<IActionResult>()
                {
                    StatusCodeResponse = new BadRequestObjectResult(errorsList),
                    StatusCode = (int)HttpStatusCode.BadRequest
                });
            }

            await _transactionsRepository.CreateAll(listEntities);
            
            return await Task.FromResult(new Result<IActionResult>()
            {
                StatusCodeResponse = new OkResult(),
                StatusCode = (int)HttpStatusCode.OK
            });
        }

        public async Task<Result<IActionResult>> TransactionsSplitAsync(TransactionsSplitHttpParams transactionsSplitHttpParams, SplitTransactionCommand splitTransactionCommand)
        {
            var validationErrorList = new List<ValidationError>();

            var transactions = _transactionsRepository.GetTransactions();
            var categories = _categoryRepository.GetCategories().ToList();

            var tr = transactions.FirstOrDefault(t => t.Id == transactionsSplitHttpParams.Id);
            if (tr == null)
            {
                return await Task.FromResult(new Result<IActionResult>()
                {
                    StatusCodeResponse = new NotFoundObjectResult(new ValidationError
                    {
                        Error = "Transaction-id-not-found",
                        Message = "Transaction Id Not Found",
                        Tag = $"invalidAmount"
                    }),
                    StatusCode = (int)HttpStatusCode.NotFound
                });
            }

            var ammountSplitInvalid = splitTransactionCommand.Splits.Where(w => w.Amount <= 0 || (w.Amount > tr.Amount));
            foreach(var invalidAmmount in ammountSplitInvalid)
            {
                validationErrorList.Add(new ValidationError
                {
                    Error = "amount-out-of-range",
                    Message = "Amount is negative or greater than amount of transaction",
                    Tag = $"invalidAmount"
                });
            }

            var catCodesToValidate = splitTransactionCommand.Splits.Select(s => s.Catcode);
            var invalidCatCodes = catCodesToValidate.Where(e => !categories.Select(s => s.Code).Contains(e));
            foreach (var invalidCode in invalidCatCodes)
            {
                validationErrorList.Add(new ValidationError
                {
                    Error = "code-doesnt-exist",
                    Message = "Code doesnt exist in categories",
                    Tag = $"invalidCode"
                });
            }

            if (validationErrorList != null && validationErrorList.Count > 0)
            {
                return await Task.FromResult(new Result<IActionResult>()
                {
                    StatusCodeResponse = new BadRequestObjectResult(validationErrorList),
                    StatusCode = (int)HttpStatusCode.BadRequest
                });
            }

            var categorizedList = splitTransactionCommand.Splits.Select(s => new SplitEntity
            {
                Transaction = tr,
                Amount = s.Amount.Value,
                CategoryCode = s.Catcode,
            });

            await _categorizedTransactionRepository.CreateAll(tr.Id,categorizedList.ToList());
            return await Task.FromResult(new Result<IActionResult>()
            {
                StatusCodeResponse = new OkResult(),
                StatusCode = (int)HttpStatusCode.OK
            });
        }
    }
}

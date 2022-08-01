using Asseco.Contracts.Errors;
using Asseco.Rest.PersonalFinanceManagementAPI.Contracts.V1.DataContracts.HttpParams;
using Asseco.Rest.PersonalFinanceManagementAPI.Contracts.V1.DataContracts.Models;
using Asseco.Rest.PersonalFinanceManagementAPI.Contracts.V1.ServiceContracts;
using Asseco.REST.Contracts.Generics;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Net;
using TransactionAPI.Database.Entities;
using TransactionAPI.Database.Repositories;

namespace TransactionAPI.Implementations
{
    public class PersonalFinanceManagementAPICategoriesCommandService : IPersonalFinanceManagementAPICategoriesCommandService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public PersonalFinanceManagementAPICategoriesCommandService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<Result<IActionResult>> CategoryImportAsync(CategoryImprotHttpParams categoryImprotHttpParams, IFormFile file)
        {

            var fileOpen = file.OpenReadStream();
            StreamReader sr = new StreamReader(fileOpen);
            string line;
            string[] row;
            sr.ReadLine();

            List<CategoryEntity> listEntities = new List<CategoryEntity>();
            List<ValidationError> errorsList = new List<ValidationError>();

            var i = 0;
            while ((line = sr.ReadLine()) != null)
            {
                row = line.Split(',');

                if (row.Length != 3)
                {
                    errorsList.Add(new ValidationError { Error = "length-line-out-of-range", Message = "Length line is out of range", Tag = $"row[{i}]" });
                }
                if (row[0].Length > 5)
                {
                    errorsList.Add(new ValidationError { Error = "code-out-of-range", Message = "Length of code is out of range", Tag = $"row[{i}].Code" });
                }
                if (row[1].Length > 5)
                {
                    errorsList.Add(new ValidationError { Error = "parent-code-out-of-range", Message = "Length of parent code is out of range", Tag = $"row[{i}].ParentCode" });
                }
                if (row[2].Length > 30)
                {
                    errorsList.Add(new ValidationError { Error = "name-out-of-range", Message = "Length of name is out of range", Tag = $"row[{i}].Name" });
                }
                else
                {
                    listEntities.Add(new CategoryEntity
                    {
                        Code = row[0],
                        ParentCode = !string.IsNullOrEmpty(row[1]) ? row[1] : "",
                        Name = !string.IsNullOrEmpty(row[2]) ? row[2] : ""
                    });
                }

                i++;
            }

            var categoryParent = _categoryRepository.GetCategories().Select(s => s.Code);
            var invalidParents = listEntities.Where(w => !categoryParent.Contains(w.ParentCode) && !listEntities.Select(s => s.Code).Contains(w.ParentCode) && !string.IsNullOrEmpty(w.ParentCode));
            
            if (invalidParents != null && invalidParents.Count() > 0)
            {
                errorsList.AddRange(invalidParents.Select(s => new ValidationError 
                { 
                    Error = "category-parent-not-found", 
                    Message = "category parent not found", 
                    Tag = $"{s.ParentCode}" 
                }));
            }

            if (errorsList != null && errorsList.Count() > 0)
            {
                return await Task.FromResult(new Result<IActionResult>()
                {
                    StatusCodeResponse = new BadRequestObjectResult(errorsList),
                    StatusCode = (int)HttpStatusCode.BadRequest
                });
            }

            await _categoryRepository.CreateAll(listEntities);

            return await Task.FromResult(new Result<IActionResult>()
            {
                StatusCodeResponse = new OkResult(),
                StatusCode = (int)HttpStatusCode.OK
            });
        }
    }
}

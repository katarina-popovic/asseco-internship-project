using Asseco.Rest.PersonalFinanceManagementAPI.Contracts.V1.DataContracts.Models;
using Asseco.Rest.PersonalFinanceManagementAPI.Contracts.V1.DataContracts.HttpParams;
using Asseco.Rest.PersonalFinanceManagementAPI.Contracts.V1.ServiceContracts;
using Asseco.Contracts;
using Asseco.Contracts.Abstractions;
using Asseco.Contracts.Common;
using Asseco.Contracts.Errors;
using Asseco.REST.Binding;
using Asseco.REST.Starter;
using Asseco.REST.Validations;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using ValidationProblem = Asseco.Contracts.Errors.ValidationError;

namespace Asseco.Rest.PersonalFinanceManagementAPI.Controller.V1
{
	[ValidateModel]
	public partial class PersonalFinanceManagementAPICategoriesController : InvocationContextBaseController
	{

		private readonly IPersonalFinanceManagementAPICategoriesCommandService _personalFinanceManagementAPICategoriesCommandService;
        private readonly IPersonalFinanceManagementAPICategoriesQueryService _personalFinanceManagementAPICategoriesQueryService;

        public PersonalFinanceManagementAPICategoriesController( IPersonalFinanceManagementAPICategoriesCommandService personalFinanceManagementAPICategoriesCommandService, IPersonalFinanceManagementAPICategoriesQueryService personalFinanceManagementAPICategoriesQueryService)
		{
					_personalFinanceManagementAPICategoriesCommandService = personalFinanceManagementAPICategoriesCommandService;
					_personalFinanceManagementAPICategoriesQueryService = personalFinanceManagementAPICategoriesQueryService;
		}

	  
		[HttpPost]
		[Route("/categories/import", Name = "Category_Import")]
		//[Consumes("application/csv")]
		[ProducesResponseType(typeof(IActionResult), 200)]
		[ProducesResponseType(typeof(BusinessProblem), 440)]
		[ProducesResponseType(typeof(ValidationProblem), 400)]
		public async System.Threading.Tasks.Task<IActionResult> CategoryImport ([ModelBinder(typeof(HttpParametersModelBinder))] CategoryImprotHttpParams categoryImprotHttpParams,
		 [FromForm] IFormFile file) 
		{
			return await _personalFinanceManagementAPICategoriesCommandService.CategoryImportAsync( categoryImprotHttpParams, file);
		}

		[HttpPost]
		[Route("/transaction/{id}/categorize", Name = "Transactions_Categorize")]
		//[Consumes("application/json")]
		[ProducesResponseType(typeof(IActionResult), 200)]
		[ProducesResponseType(typeof(BusinessProblem), 440)]
		[ProducesResponseType(typeof(ValidationProblem), 400)]
		public async System.Threading.Tasks.Task<IActionResult> TransactionsCategorize([ModelBinder(typeof(HttpParametersModelBinder))] TransactionsCategorizeHttpParams transactionsCategorizeHttpParams,
		 [FromBody] TransactionCategorizeCommand transactionCategorizeCommand)
		{
			return await _personalFinanceManagementAPICategoriesQueryService.TransactionsCategorizeAsync(transactionsCategorizeHttpParams, transactionCategorizeCommand);
		}
	}
}
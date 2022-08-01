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
	public partial class PersonalFinanceManagementAPITransactionsController : ControllerBase
	{
		
		private readonly IPersonalFinanceManagementAPITransactionsQueryService _personalFinanceManagementAPITransactionsQueryService; 

		private readonly IPersonalFinanceManagementAPITransactionsCommandService _personalFinanceManagementAPITransactionsCommandService; 

		public PersonalFinanceManagementAPITransactionsController(IPersonalFinanceManagementAPITransactionsQueryService personalFinanceManagementAPITransactionsQueryService, IPersonalFinanceManagementAPITransactionsCommandService personalFinanceManagementAPITransactionsCommandService)
		{
					_personalFinanceManagementAPITransactionsQueryService = personalFinanceManagementAPITransactionsQueryService;
					_personalFinanceManagementAPITransactionsCommandService = personalFinanceManagementAPITransactionsCommandService;
		}

	  
		[HttpGet]
		[Route("/transactions", Name = "Transactions_GetList")]
		[Consumes("application/json")]
		[ProducesResponseType(typeof(TransactionPagedList), 200)]
		[ProducesResponseType(typeof(ValidationProblem), 400)]
		public async System.Threading.Tasks.Task<IActionResult> TransactionsGetList ([ModelBinder(typeof(HttpParametersModelBinder))] TransactionsGetListHttpParams transactionsGetListHttpParams, [FromQuery] Paging paging, [FromQuery] Sorting sorting) 
		{
			return await _personalFinanceManagementAPITransactionsQueryService.TransactionsGetListAsync(transactionsGetListHttpParams, paging, sorting);
			
		}

		[HttpPost]
		[Route("/transactions/import", Name = "Transactions_Import")]
		//[Consumes("application/csv")]
		[ProducesResponseType(typeof(IActionResult), 200)]
		[ProducesResponseType(typeof(BusinessProblem), 440)]
		[ProducesResponseType(typeof(ValidationProblem), 400)]
		public async System.Threading.Tasks.Task<IActionResult> TransactionsImport ([ModelBinder(typeof(HttpParametersModelBinder))] TransactionsImportHttpParams transactionsImportHttpParams,
		 [FromForm] IFormFile file) 
		{
			return await _personalFinanceManagementAPITransactionsCommandService.TransactionsImportAsync( transactionsImportHttpParams, file);
		}

		[HttpPost]
		[Route("/transaction/{id}/split", Name = "Transactions_Split")]
		[Consumes("application/json")]
		[ProducesResponseType(typeof(IActionResult), 200)]
		[ProducesResponseType(typeof(BusinessProblem), 440)]
		[ProducesResponseType(typeof(ValidationProblem), 400)]
		public async System.Threading.Tasks.Task<IActionResult> TransactionsSplit([ModelBinder(typeof(HttpParametersModelBinder))] TransactionsSplitHttpParams transactionsSplitHttpParams,
		 [FromBody] SplitTransactionCommand splitTransactionCommand)
		{
			return await _personalFinanceManagementAPITransactionsCommandService.TransactionsSplitAsync(transactionsSplitHttpParams, splitTransactionCommand);
		}
	}
}
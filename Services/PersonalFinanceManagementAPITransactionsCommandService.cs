/*using Asseco.Rest.PersonalFinanceManagementAPI.Contracts.V1.DataContracts.Models;
using Asseco.Rest.PersonalFinanceManagementAPI.Contracts.V1.ServiceContracts;
using Asseco.Rest.PersonalFinanceManagementAPI.Contracts.V1.DataContracts.HttpParams;
using Asseco.Contracts;
using Asseco.Contracts.Common;
using Asseco.Contracts.Errors;
using Asseco.Messaging.Abstractions;
using Asseco.REST.Clients.Abstractions.Models.Content;
using Asseco.REST.Contracts.Generics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Text;

namespace Asseco.Rest.PersonalFinanceManagementAPI.Services.EF.V1
{
	[ServiceContract]
	public class PersonalFinanceManagementAPITransactionsCommandService: IPersonalFinanceManagementAPITransactionsCommandService
	{	
		
		private readonly ILogger<PersonalFinanceManagementAPITransactionsCommandService> _logger;

		private readonly IBrokerFacade _eventBus;

		public PersonalFinanceManagementAPITransactionsCommandService(ILogger<PersonalFinanceManagementAPITransactionsCommandService> logger, IBrokerFacade eventBus)
		{
			_logger = logger;
			_eventBus = eventBus;
		}

		public virtual System.Threading.Tasks.Task<Result<IActionResult>> TransactionsImportAsync (InvocationContext invocationContext, TransactionsImportHttpParams transactionsImportHttpParams, Transaction transaction )
		{
			throw new NotImplementedException();		 
    }

		public virtual System.Threading.Tasks.Task<Result<IActionResult>> TransactionsSplitAsync (InvocationContext invocationContext, TransactionsSplitHttpParams transactionsSplitHttpParams, SplitTransactionCommand splitTransactionCommand )
		{
			throw new NotImplementedException();		 
    }

		public virtual System.Threading.Tasks.Task<Result<IActionResult>> TransactionsCategorizeAsync (InvocationContext invocationContext, TransactionsCategorizeHttpParams transactionsCategorizeHttpParams, TransactionCategorizeCommand transactionCategorizeCommand )
		{
			throw new NotImplementedException();		 
    }

		public virtual System.Threading.Tasks.Task<Result<IActionResult>> TransactionsAutoCategorizeAsync (InvocationContext invocationContext, TransactionsAutoCategorizeHttpParams transactionsAutoCategorizeHttpParams)
		{
			throw new NotImplementedException();		 
    }

	}
}*/
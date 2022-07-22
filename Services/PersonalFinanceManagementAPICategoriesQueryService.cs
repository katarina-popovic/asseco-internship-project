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
	public class PersonalFinanceManagementAPICategoriesQueryService: IPersonalFinanceManagementAPICategoriesQueryService
	{	

		private readonly ILogger<PersonalFinanceManagementAPICategoriesQueryService> _logger;

		private readonly IBrokerFacade _eventBus;

		public PersonalFinanceManagementAPICategoriesQueryService(ILogger<PersonalFinanceManagementAPICategoriesQueryService> logger, IBrokerFacade eventBus)
		{
			_logger = logger;
			_eventBus = eventBus;
		}

		public virtual  System.Threading.Tasks.Task<Result<CategoryList>> CategoriesGetListAsync (InvocationContext invocationContext, CategoriesGetListHttpParams categoriesGetListHttpParams)
		{
			throw new NotImplementedException();		 
		}

	}
}*/
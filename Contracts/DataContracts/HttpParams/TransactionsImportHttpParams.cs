using Asseco.REST.Binding.Attributes;
using Asseco.REST.Validations;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace Asseco.Rest.PersonalFinanceManagementAPI.Contracts.V1.DataContracts.HttpParams
{
  [Serializable]
  [DataContract]
  public class TransactionsImportHttpParams: Asseco.Utilities.Reflection.RuntimeSafeExpando
  {
  }
}
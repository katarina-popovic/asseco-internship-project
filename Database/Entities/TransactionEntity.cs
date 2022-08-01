using Asseco.Rest.PersonalFinanceManagementAPI.Contracts.V1.DataContracts.Models;

namespace TransactionAPI.Database.Entities
{
    public class TransactionEntity
    {
        public string Id {get; set;}
        public string BeneficiaryName {get; set;}
        public DateTime Date {get; set;}
        public char Direction {get; set;}
        public double Amount {get; set;}
        public string Description {get; set;}
        public string Currency {get; set;}
        public MccCodeEnum Mcc {get; set;}
        public string Kind {get; set;}
        public string CatCode {get; set;}
        public virtual List<SplitEntity> SplitTransactionList { get; set; }
    }
}
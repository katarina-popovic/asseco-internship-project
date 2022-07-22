using Asseco.Rest.PersonalFinanceManagementAPI.Contracts.V1.DataContracts.Models;
using AutoMapper;
using TransactionAPI.Database.Entities;

namespace TransactionAPI.Mappins
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile(){
            CreateMap<TransactionEntity, Models.Transaction>()
                    .ForMember(d => d.transactionId, opts => opts.MapFrom(t => t.Id));

            CreateMap<CreateTransactionCommand, TransactionEntity>()
                    .ForMember(d => d.Id, opts => opts.MapFrom(t => t.TransactionId));
        }
    }
}


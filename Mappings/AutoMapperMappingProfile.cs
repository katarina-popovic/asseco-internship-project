using Asseco.Rest.PersonalFinanceManagementAPI.Contracts.V1.DataContracts.Models;
using AutoMapper;
using TransactionAPI.Command;
using TransactionAPI.Database.Entities;

namespace TransactionAPI.Mappins
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile(){
            CreateMap<TransactionEntity, TransactionWithSplits>()
                    .ForMember(d => d.Id, opts => opts.MapFrom(t => t.Id))
                    .ForMember(d => d.Splits, opts => opts.MapFrom(t => t.SplitTransactionList));

            CreateMap<CreateTransactionCommand, TransactionEntity>()
                    .ForMember(d => d.Id, opts => opts.MapFrom(t => t.Id));

            CreateMap<SplitEntity, SingleCategorySplit>()
                .ForMember(d => d.Catcode, opts => opts.MapFrom(t => t.CategoryCode))
                .ForMember(d => d.Amount, opts => opts.MapFrom(t => t.Amount));
        }
    }
}


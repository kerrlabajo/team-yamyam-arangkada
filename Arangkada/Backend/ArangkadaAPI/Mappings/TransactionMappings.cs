using AutoMapper;
using ArangkadaAPI.Dtos.Transaction;
using ArangkadaAPI.Models;

namespace ArangkadaAPI.Mappings
{
    public class TransactionMappings : Profile
    {
        public TransactionMappings()
        {
            CreateMap<Transaction, TransactionDto>();
            CreateMap<TransactionUpdateDto, Transaction>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.OperatorName, opt => opt.Ignore())
                .ForMember(dest => dest.DriverName, opt => opt.Ignore());
            CreateMap<TransactionCreationDto, Transaction>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Amount, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.EndDate, opt => opt.Ignore());
        }
    }
}

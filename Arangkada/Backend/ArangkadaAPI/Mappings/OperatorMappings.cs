using ArangkadaAPI.Dtos.Operator;
using ArangkadaAPI.Models;
using AutoMapper;

namespace ArangkadaAPI.Mappings
{
    public class OperatorMappings : Profile
    {
        public OperatorMappings()
        {
            CreateMap<Operator, OperatorDto>();
            CreateMap<OperatorCreationDto, Operator>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.IsVerified, opt => opt.Ignore())
                .ForMember(dest => dest.Vehicles, opt => opt.Ignore())
                .ForMember(dest => dest.Drivers, opt => opt.Ignore())

                .ForMember(dest => dest.VerificationCode, opt => opt.Ignore());
            CreateMap<OperatorUpdateDto, Operator>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.IsVerified, opt => opt.Ignore())
                .ForMember(dest => dest.Vehicles, opt => opt.Ignore())
                .ForMember(dest => dest.Drivers, opt => opt.Ignore())
                .ForMember(dest => dest.VerificationCode, opt=> opt.Ignore());
            
        }
    }
}
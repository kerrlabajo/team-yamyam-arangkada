using ArangkadaAPI.Dtos.Driver;
using ArangkadaAPI.Models;
using AutoMapper;

namespace ArangkadaAPI.Mappings
{
    public class DriverMappings : Profile
    {
        public DriverMappings()
        {
            CreateMap<Driver, DriverDto>();
            CreateMap<DriverCreationDto, Driver>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.VehicleAssigned, opt => opt.Ignore());
            CreateMap<DriverUpdateDto, Driver>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.OperatorName, opt => opt.Ignore())
                .ForMember(dest => dest.VehicleAssigned, opt => opt.Ignore());
        }
    }
}

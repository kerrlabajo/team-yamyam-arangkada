using ArangkadaAPI.Dtos.Driver;
using ArangkadaAPI.Dtos.Transaction;
using ArangkadaAPI.Dtos.Vehicle;
using ArangkadaAPI.Models;
using AutoMapper;

namespace ArangkadaAPI.Mappings
{
    public class VehicleMappings : Profile
    {
        public VehicleMappings()
        {
            CreateMap<Vehicle, VehicleDto>();
            CreateMap<VehicleCreationDto, Vehicle>()
              .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.RentStatus, opt => opt.Ignore());
        }
    }
}

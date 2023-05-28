using ArangkadaAPI.Dtos.Vehicle;
using ArangkadaAPI.Models;
using ArangkadaAPI.Repositories;
using AutoMapper;
using System.Diagnostics;

namespace ArangkadaAPI.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IMapper _mapper;

        public VehicleService(IVehicleRepository vehicleRepository, IMapper mapper)
        {
            _vehicleRepository = vehicleRepository;
            _mapper = mapper;
        }
        public async Task<VehicleDto?> AddVehicle(VehicleCreationDto vehicleToAdd)
        {
            try
            {               
                var vehicleModel = _mapper.Map<Vehicle>(vehicleToAdd);
                vehicleModel.Id = await _vehicleRepository.AddVehicle(vehicleModel);
                var vehiclDto = _mapper.Map<VehicleDto>(vehicleModel);
                return vehiclDto;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return null;
            }
        }

        public async Task<IEnumerable<VehicleDto>?> GetAll()
        {
            try
            {
                var vehicleModels = await _vehicleRepository.GetAll();
                return _mapper.Map<IEnumerable<VehicleDto>>(vehicleModels);
            }
            catch(Exception ex) 
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<IEnumerable<VehicleDto>?> GetAllByOperatorId(int operatorId)
        {
            try
            {
                var vehicleModel = await _vehicleRepository.GetAllByOperatorId(operatorId);

                return _mapper.Map<IEnumerable<VehicleDto>>(vehicleModel);
            }
            catch(Exception ex)
            {
                Debug.Write(ex.Message);
                return null;
            }
        }
        public async Task<VehicleDto?> GetById(int id)
        {
            try
            {
                var vehicleModel = await _vehicleRepository.GetById(id);
                if(vehicleModel == null)
                {
                    return null;
                }

                return _mapper.Map<VehicleDto?>(vehicleModel);

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }
        public async Task<VehicleDto?> GetByPlateNumber(string? plateNumber)
        {
            try
            {
                var vehicleModels = await _vehicleRepository.GetByPlateNumber(plateNumber);

                if(vehicleModels == null)
                {
                    return null;
                }

                return _mapper.Map<VehicleDto?>(vehicleModels);
            }catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);    
                return null;
            }
        }
        public async Task<VehicleDto?> UpdateRentStatus(int id, bool status)
        {
            try
            {
                var vehicleModel = await _vehicleRepository.UpdateRentStatus(id, status);
                return _mapper.Map<VehicleDto?>(vehicleModel);

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }
        public async Task<VehicleDto?> UpdateRentFee(int id, double rentFee)
        {
            try
            {
                var vehicleModel = await _vehicleRepository.UpdateRentFee(id, rentFee);
                return _mapper.Map<VehicleDto?>(vehicleModel);

            }catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }
        public async Task<bool> DeleteVehicle(int id)
        {
            try
            {
                return await _vehicleRepository.DeleteVehicle(id);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }
    }
}

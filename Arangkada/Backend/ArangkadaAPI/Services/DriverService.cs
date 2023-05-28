using ArangkadaAPI.Dtos.Driver;
using ArangkadaAPI.Dtos.Transaction;
using ArangkadaAPI.Models;
using ArangkadaAPI.Repositories;
using AutoMapper;
using System.Diagnostics;

namespace ArangkadaAPI.Services
{
    public class DriverService : IDriverService
    {
        private readonly IDriverRepository _repository;
        private readonly IMapper _mapper;
        public DriverService(IDriverRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<DriverDto?> CreateDriver(DriverCreationDto driverToCreate)
        {
            try
            {
                var driverModel = _mapper.Map<Driver>(driverToCreate);
                driverModel.Id = await _repository.CreateDriver(driverModel);
                var driverDto = _mapper.Map<DriverDto>(driverModel);
                return driverDto;
            } catch(Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
            
        }

        public async Task<IEnumerable<DriverDto>?> GetAll()
        {
            try
            {
                var driverModel = await _repository.GetAll();
                return _mapper.Map<IEnumerable<DriverDto>>(driverModel);
            } catch(Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }

        public async Task<IEnumerable<DriverDto>?> GetAllByOperatorId(int operatorId)
        {
            try
            {
                var driverModel = await _repository.GetAllByOperatorId(operatorId);

                if (driverModel == null) return null;
                return _mapper.Map<IEnumerable<DriverDto>>(driverModel);
            } catch(Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }

        public async Task<DriverDto?> GetById(int id)
        {
            try
            {
                var driverModel = await _repository.GetById(id);

                if (driverModel == null) return null;
                return _mapper.Map<DriverDto?>(driverModel);
            } catch(Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }

        public async Task<DriverDto?> GetByFullName(string? fullName)
        {
            try
            {
                var driverModel = await _repository.GetByFullName(fullName);

                if (driverModel == null) return null;
                return _mapper.Map<DriverDto?>(driverModel);
            }catch(Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }

        public async Task<DriverDto?> UpdateDriver(int id, DriverUpdateDto driverToUpdate)
        {
            try
            {
                var driverModel = _mapper.Map<Driver>(driverToUpdate);
                driverModel.Id = id;
                var updatedDriver = await _repository.UpdateDriver(id, driverModel);
                return _mapper.Map<DriverDto>(updatedDriver);
            } catch(Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }

        public async Task<DriverDto?> UpdateVehicleAssigned(int id, string? plateNumberAssigned)
        {
            try
            {
                var updatedDriver = await _repository.UpdateVehicleAssigned(id, plateNumberAssigned);
                return _mapper.Map<DriverDto>(updatedDriver);
            } catch(Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }

        public async Task<bool> DeleteDriver(int id)
        {
            try
            {
                return await _repository.DeleteDriver(id);
            } catch(Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }
    }
}

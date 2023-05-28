using ArangkadaAPI.Dtos.Operator;
using ArangkadaAPI.Models;
using ArangkadaAPI.Repositories;
using AutoMapper;
using System.Diagnostics;



namespace ArangkadaAPI.Services
{
    public class OperatorService : IOperatorService
    {
        private readonly IOperatorRepository _repository;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        public OperatorService(IOperatorRepository repository, IMapper mapper, IEmailService emailService)
        {
            _repository = repository;
            _mapper = mapper;
            _emailService = emailService;
        }

        public async Task<OperatorDto?> CreateOperator(OperatorCreationDto OperatorToCreate)
        {
            try
            {
                var OperatorModel = _mapper.Map<Operator>(OperatorToCreate);
                var operatorId = await _repository.CreateOperator(OperatorModel);

                OperatorModel = await _repository.GetById(operatorId);

                var OperatorDto = _mapper.Map<OperatorDto>(OperatorModel);

                var email = OperatorDto.Email;
                var verif = OperatorDto.VerificationCode;
                    
                await _emailService.SendEmailVerification(email!, verif!);

                return OperatorDto;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }

        public async Task<IEnumerable<OperatorDto>?> GetAll()
        {
            try
            {
                var OperatorModel = await _repository.GetAll();
                return _mapper.Map<IEnumerable<OperatorDto>>(OperatorModel);
            } catch(Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }

        public async Task<OperatorDto?> GetById(int id)
        {
            try
            {
                var OperatorModel = await _repository.GetById(id);
                if (OperatorModel == null) return null;
                return _mapper.Map<OperatorDto>(OperatorModel);
            } catch(Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }

        public async Task<OperatorDto?> GetByFullName(string? fullName)
        {
            try
            {
                var operatorModel = await _repository.GetByFullName(fullName);

                if (operatorModel == null) return null;
                return _mapper.Map<OperatorDto?>(operatorModel);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }

        public async Task<OperatorDto?> GetByUsername(string? username)
        {
            try
            {
                var operatorModel = await _repository.GetByUsername(username);
                if (operatorModel == null) return null;
                return _mapper.Map<OperatorDto?>(operatorModel);
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }

        public async Task<string?> GetPasswordById(int id)
        {
            try
            {
                var passwordString = await _repository.GetPasswordById(id);
                if(passwordString == null) return null;

                string encryptedPassword = BCrypt.Net.BCrypt.HashPassword(passwordString);
                return encryptedPassword;
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }

        public async Task<bool?> GetIsVerifiedById(int id)
        {
            try
            {
                var isVerifiedBool = await _repository.GetIsVerifiedById(id);

                return isVerifiedBool;
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }
 
        public async Task<OperatorDto?> UpdateOperator(int id, OperatorUpdateDto operatorToUpdate)
        {
            try
            {
                var operatorModel = _mapper.Map<Operator>(operatorToUpdate);
                operatorModel.Id = id;
                var updatedOperator = await _repository.UpdateOperator(id, operatorModel);
                return _mapper.Map<OperatorDto>(updatedOperator);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }

        public async Task<OperatorDto?> UpdateIsVerified(int id, bool isVerified)
        {
            try
            {
                var updatedOperator = await _repository.UpdateIsVerified(id, isVerified);
                return _mapper.Map<OperatorDto>(updatedOperator);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }

        public async Task<bool> DeleteOperator(int id)
        {
            try
            {
                return await _repository.DeleteOperator(id);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }
    }
}

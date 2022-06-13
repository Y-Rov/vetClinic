using Core.Entities;
using Core.Exceptions;
using Core.Interfaces;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;

namespace Application.Services
{
    public class SpecializationService : ISpecializationService
    {
        readonly ISpecializationRepository _repository;
        readonly ILoggerManager _logger;
        public SpecializationService(
            ISpecializationRepository repository, 
            ILoggerManager logger)
        {
            _repository = repository;
            _logger = logger;
        }
        public async Task<IEnumerable<Specialization>> GetAllSpecializationsAsync()
        {
            _logger.LogInfo($"specializations were recieved");
            return await _repository.GetAsync(asNoTracking: true, includeProperties:  "ProcedureSpecializations.Procedure");
        }

        public async Task<Specialization> GetSpecializationByIdAsync(int id)
        {
            Specialization specialization = await _repository.GetById(id, "ProcedureSpecializations.Procedure,ProcedureSpecializations");
            if (specialization is null)
            {
                _logger.LogWarn($"Specialization with id: {id} not found");
                throw new NotFoundException($"Specialization with id: {id} not found");
            }
            _logger.LogInfo($"Specialization with id: {id} found");
            return specialization;
        }

        public async Task<IEnumerable<Procedure>> GetSpecializationProcedures(int id)
        {
            Specialization specialization = await GetSpecializationByIdAsync(id);
            _logger.LogInfo($"Specialization's procedures found");
            return specialization.ProcedureSpecializations?.Select(ps => ps.Procedure);
        }

        public async Task<Specialization> AddSpecializationAsync(Specialization specialization)
        {
            await _repository.InsertAsync(specialization);
            await _repository.SaveChangesAsync();
            _logger.LogInfo("Specialization added");
            return specialization;
        }

        public async Task AddProcedureToSpecialization(int specializationId, int procedureId)
        {
            var specialization = 
                await GetSpecializationByIdAsync(specializationId);
            
            specialization.ProcedureSpecializations.Add(new ProcedureSpecialization 
            { 
                ProcedureId = procedureId, 
                SpecializationId = specializationId 
            });

            await UpdateSpecializationAsync(specializationId, specialization);

            await _repository.SaveChangesAsync();
        }

        public async Task RemoveProcedureFromSpecialization(int specializationId, int procedureId)
        {
            var specialization =
                await GetSpecializationByIdAsync(specializationId);

            var procedure = specialization.ProcedureSpecializations
                .FirstOrDefault(ps => ps.ProcedureId == procedureId && ps.SpecializationId == specializationId)
                    ?? throw new NotFoundException($"Specialization's procedure with id: {procedureId} not found");

            specialization.ProcedureSpecializations.Remove(procedure);

            await UpdateSpecializationAsync(specializationId, specialization);

            await _repository.SaveChangesAsync();
        }

        public async Task DeleteSpecializationAsync(int id) 
        {
            var specialization = await GetSpecializationByIdAsync(id);
            _repository.Delete(specialization);
            _logger.LogInfo($"Specialization with id: {id} deleted");
            await _repository.SaveChangesAsync();
        }

        public async Task UpdateSpecializationAsync(int id, Specialization updated)
        {
            Specialization specialization = await GetSpecializationByIdAsync(id);
            specialization.Name = updated.Name;
            _logger.LogInfo($"Specialization with id: {updated.Id} updated");
            await _repository.SaveChangesAsync();
        }
    }
}

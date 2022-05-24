using Core.Entities;
using Core.Exceptions;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;

namespace Application.Services
{
    public class SpecializationService : ISpecializationService
    {
        ISpecializationRepository _repository;
        public SpecializationService(ISpecializationRepository repository)
        {
            _repository = repository;
        }
        public async Task<IEnumerable<Specialization>> GetAllSpecializationsAsync()
        {
            return await _repository.GetAllSpecializationsAsync();
        }

        public async Task<Specialization> GetSpecializationByIdAsync(int id)
        {
            Specialization specialization = 
                await _repository.GetSpecializationByIdAsync(id);
            return specialization is null ?
                throw new NotFoundException($"Specialization with id: {id} not found"):
                specialization;
        }

        public async Task<Specialization> AddSpecializationAsync(Specialization specialization)
        {
            return await _repository.AddSpecializationAsync(specialization);
        }

        public async Task<int> DeleteSpecializationAsync(int id) 
        {
            return await _repository.DeleteSpecializationAsync(id); 
        }

        public async Task<Specialization> UpdateSpecializationAsync(int id, Specialization updated)
        {
            Specialization specializationToUpdate = await _repository.GetSpecializationByIdAsync(updated.Id);
            if (specializationToUpdate == null)
                throw new NotFoundException($"Specialization with id: {updated.Id} not found");

            return await _repository.UpdateSpecializationAsync(id, updated);
        }
    }
}

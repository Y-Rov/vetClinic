using Core.Entities;
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
            return await _repository.GetSpecializationByIdAsync(id);
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
            return await _repository.UpdateSpecializationAsync(id, updated);
        }
    }
}

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
            await _repository.AddSpecializationAsync(specialization);
            await _repository.SaveChangesAsync();
            return specialization;
        }

        public async Task DeleteSpecializationAsync(int id) 
        {
            var specialization = await _repository.GetSpecializationByIdAsync(id);
            if (specialization == null)
                throw new NotFoundException($"Specialization with id: {id} not found");
            await _repository.DeleteSpecializationAsync(specialization);
            await _repository.SaveChangesAsync();
        }

        public async Task UpdateSpecializationAsync(int id, Specialization updated)
        {
            Specialization specialization = await _repository.GetSpecializationByIdAsync(updated.Id);
            if (specialization == null) throw new NotFoundException($"Specialization with id: {updated.Id} not found");
            specialization.Name = updated.Name;
            await _repository.SaveChangesAsync();
        }
    }
}

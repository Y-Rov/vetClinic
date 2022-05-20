using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class SpecializationService : ISpecializationService
    {
        ISpecializationRepository _repository;
        public SpecializationService(ISpecializationRepository repository)
        {
            _repository = repository;
        }
        public async Task<IEnumerable<Specialization>> GetAllSpecializationsAsync() =>
            await _repository.GetAllSpecializationsAsync();

        public async Task<Specialization> GetSpecializationByIdAsync(int id) =>
            await _repository.GetSpecializationByIdAsync(id);

        public async Task<Specialization> AddSpecializationAsync(Specialization specialization) =>
            await _repository.AddSpecializationAsync(specialization);

        public async Task<int> DeleteSpecializationAsync(int id) =>
            await _repository.DeleteSpecializationAsync(id);

        public async Task<Specialization> UpdateSpecializationAsync(int id, Specialization updated) =>
            await _repository.UpdateSpecializationAsync(id, updated);
    }
}

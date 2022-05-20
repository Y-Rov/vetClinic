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
        public async Task<IEnumerable<Specialization>> GetAllSpecializations() =>
            await _repository.GetAllSpecializations();

        public async Task<Specialization> GetSpecializationById(int id) =>
            await _repository.GetSpecializationById(id);

        public async Task<Specialization> AddSpecialization(Specialization specialization) =>
            await _repository.AddSpecialization(specialization);

        public async Task<int> DeleteSpecialization(int id) =>
            await _repository.DeleteSpecialization(id);

        public async Task<Specialization> UpdateSpecialization(int id, Specialization updated) =>
            await _repository.UpdateSpecialization(id, updated);
    }
}

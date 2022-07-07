using Core.Entities;
using Core.Paginator;
using Core.Paginator.Parameters;

namespace Core.Interfaces.Services
{
    public interface ISpecializationService
    {
        Task<PagedList<Specialization>> GetAllSpecializationsAsync(SpecializationParameters parameters);
        Task<Specialization> GetSpecializationByIdAsync(int id);
        public Task<IEnumerable<User>> GetEmployeesAsync();
        Task<IEnumerable<Procedure>> GetSpecializationProcedures(int id);
        Task<Specialization> AddSpecializationAsync(Specialization specialization);
        Task AddProcedureToSpecialization(int specializationId, int procedureId);
        Task RemoveProcedureFromSpecialization(int specializationId, int procedureId);
        Task AddUserToSpecialization(int specializationId, int userId);
        Task RemoveUserFromSpecialization(int specializationId, int userId);
        Task UpdateSpecializationProceduresAsync(int specializationId, IEnumerable<int> procedureIds);
        Task UpdateSpecializationUsersAsync(int specializationId, IEnumerable<int> userIds);
        Task UpdateSpecializationAsync(int id,Specialization updated);
        Task DeleteSpecializationAsync(int id);
    }
}

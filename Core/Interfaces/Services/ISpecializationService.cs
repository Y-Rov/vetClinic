using Core.Entities;

namespace Core.Interfaces.Services
{
    public interface ISpecializationService
    {
        Task<IEnumerable<Specialization>> GetAllSpecializationsAsync();
        Task<Specialization> GetSpecializationByIdAsync(int id);
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

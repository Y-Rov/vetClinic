using Core.Entities;
using Core.Exceptions;
using Core.Interfaces;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Paginator;
using Core.Paginator.Parameters;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class SpecializationService : ISpecializationService
    {
        readonly ISpecializationRepository _repository;
        readonly IUserRepository _userRepository;
        readonly IProcedureSpecializationRepository _procedureSpecializationRepository;
        readonly IUserSpecializationRepository _userSpecializationRepository;

        readonly ILoggerManager _logger;

        bool IsProcedureExistsInSpecialization(Specialization specialization, int procedureId)
        {
            return specialization.ProcedureSpecializations
                .Any(pair => pair.SpecializationId == specialization.Id && pair.ProcedureId == procedureId);
        }

        public SpecializationService(
            ISpecializationRepository repository,
            IUserRepository userRepository,
            IProcedureSpecializationRepository procedureSpecializationRepository,
            IUserSpecializationRepository usersSpecializationRepository,
            ILoggerManager logger)
        {
            _repository = repository;
            _userRepository = userRepository;
            _logger = logger;
            _procedureSpecializationRepository = procedureSpecializationRepository;
            _userSpecializationRepository = usersSpecializationRepository;
        }

        public async Task<PagedList<Specialization>> GetAllSpecializationsAsync(SpecializationParameters parameters)
        {
            var specializations =
                await _repository.GetAllAsync(parameters, 
                includeProperties: query => query
                    .Include(specialization => specialization.UserSpecializations)
                        .ThenInclude(us => us.User)
                    .Include(specialization => specialization.ProcedureSpecializations)
                        .ThenInclude(ps => ps.Procedure));

            _logger.LogInfo($"specializations were recieved");
            return specializations;
            //return await _repository.GetAsync(asNoTracking: true, includeProperties: "ProcedureSpecializations.Procedure,UserSpecializations,UserSpecializations.User");
        }

        public async Task<IEnumerable<User>> GetEmployeesAsync()
        {
            return await _userRepository.GetByRolesAsync(
                roleIds: new List<int>{ 2, 3 });
        }

        public async Task<Specialization> GetSpecializationByIdAsync(int id)
        {
            Specialization specialization = await _repository.GetById(id, "ProcedureSpecializations.Procedure,ProcedureSpecializations,UserSpecializations.User");
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
            return specialization.ProcedureSpecializations
                .Select(ps => ps.Procedure);
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

            var relationship = new ProcedureSpecialization
            {
                ProcedureId = procedureId,
                SpecializationId = specializationId
            };

            if (!IsProcedureExistsInSpecialization(specialization, procedureId))
            {
                specialization.ProcedureSpecializations.Add(relationship);
                await UpdateSpecializationAsync(specializationId, specialization);
            }
            else
                throw new ArgumentException($"There already procedure with id: {procedureId}");
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
            _repository.Update(specialization);
            _logger.LogInfo($"Specialization with id: {updated.Id} updated");
            await _repository.SaveChangesAsync();
        }

        public async Task AddUserToSpecialization(int specializationId, int userId)
        {
            var specialization =
                await GetSpecializationByIdAsync(specializationId);

            var relationship = new UserSpecialization
            {
                UserId = userId,
                SpecializationId = specializationId
            };

            if (!specialization.UserSpecializations
                .Any(pair => pair.SpecializationId == specializationId && pair.UserId == userId))
            {
                specialization.UserSpecializations.Add(relationship);
                await UpdateSpecializationAsync(specializationId, specialization);
            }
            else
                throw new ArgumentException($"There already user with id: {userId}");
        }

        public async Task RemoveUserFromSpecialization(int specializationId, int userId)
        {
            var specialization =
                await GetSpecializationByIdAsync(specializationId);

            var user = specialization.UserSpecializations
                .FirstOrDefault(
                us => us.UserId == userId 
                && us.SpecializationId == specializationId)
                    ?? throw new NotFoundException($"Specialization's user with id: {userId} not found");

            specialization.UserSpecializations.Remove(user);

            await UpdateSpecializationAsync(specializationId, specialization);
        }

        public async Task UpdateSpecializationProceduresAsync(int specializationId, IEnumerable<int> proceduresIds)
        {
            var current = await _procedureSpecializationRepository.GetAsync(
                filter: relationship => relationship.SpecializationId == specializationId);

            foreach (var relationship in current)
                _procedureSpecializationRepository.Delete(relationship);

            await _procedureSpecializationRepository.SaveChangesAsync();

            foreach (var procedureId in proceduresIds)
            {
                await _procedureSpecializationRepository.InsertAsync(new ProcedureSpecialization()
                {
                    ProcedureId = procedureId,
                    SpecializationId = specializationId
                });
            }
            await _repository.SaveChangesAsync();
        }

        public async Task UpdateSpecializationUsersAsync(int specializationId, IEnumerable<int> userIds)
        {
            var related = await _userSpecializationRepository.GetAsync(
                filter: relationship => relationship.SpecializationId == specializationId);

            foreach (var relationship in related)
                _userSpecializationRepository.Delete(relationship);

            await _userSpecializationRepository.SaveChangesAsync();

            foreach (var userId in userIds)
                await _userSpecializationRepository.InsertAsync(new UserSpecialization
                {
                    UserId = userId,
                    SpecializationId = specializationId
                });

            await _repository.SaveChangesAsync();
        }
    }
}

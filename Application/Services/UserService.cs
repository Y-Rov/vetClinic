using Core.Entities;
using Core.Exceptions;
using Core.Interfaces;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Paginator;
using Core.Paginator.Parameters;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILoggerManager _loggerManager;
        private readonly IUserProfilePictureService _userProfilePictureService;

        public UserService(
            IUserRepository userRepository, 
            ILoggerManager loggerManager,
            IUserProfilePictureService userProfilePictureService)
        {
            _userRepository = userRepository;
            _loggerManager = loggerManager;
            _userProfilePictureService = userProfilePictureService;
        }

        public async Task AssignRoleAsync(User user, string role)
        {
            var assignResult = await _userRepository.AssignRoleAsync(user, role);

            if (!assignResult.Succeeded)
            {
                _loggerManager.LogWarn("Failed to assign the role of " +
                    $"{role} to the user with id {user.Id}");
                throw new BadRequestException(assignResult.Errors);
            }

            _loggerManager.LogInfo("Successfully assigned the role of " +
                $"{role} to the user with id {user.Id}");
        }

        public async Task CreateAsync(User user, string password)
        {
            var createResult = await _userRepository.CreateAsync(user, password);

            if (!createResult.Succeeded)
            {
                _loggerManager.LogWarn("Failed to create a user");
                throw new BadRequestException(createResult.Errors);
            }

            _loggerManager.LogInfo("Successfully created a user");
        }

        public async Task DeleteAsync(User user)
        {
            _userRepository.Delete(user);
            await _userRepository.UpdateAsync(user);
            await _userProfilePictureService.DeleteAsync(user.ProfilePicture!);

            _loggerManager.LogInfo($"Successfully deleted the user with id {user.Id}");
        }

        public async Task<PagedList<User>> GetAllUsersAsync(UserParameters userParameters)
        {
            var filterQuery = GetFilterQuery(userParameters.FilterParam);
            var orderByQuery = GetOrderByQuery(userParameters.OrderByParam);

            var users = await _userRepository.GetAllAsync(
                userParameters: userParameters,
                filter: filterQuery,
                orderBy: orderByQuery,
                includeProperties: query => query
                    .Include(u => u.Address)
                    .Include(u => u.Portfolio!));

            _loggerManager.LogInfo("Successfully retrieved all users");

            return users;
        }

        public async Task<IEnumerable<User>> GetDoctorsAsync(string specialization = "")
        {
            var doctors = await _userRepository.GetByRoleAsync(
                roleName: "Doctor",
                includeProperties: query => query
                    .Include(u => u.Address)
                    .Include(u => u.Portfolio)
                    .Include(u => u.UserSpecializations)
                        .ThenInclude(us => us.Specialization!));

            if (!string.IsNullOrEmpty(specialization))
            {
                doctors = _userRepository.FilterBySpecialization(doctors, specialization);
            }

            _loggerManager.LogInfo("Successfully retrieved all doctors");

            return doctors;
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id, 
                query => query
                    .Include(u => u.Address)
                    .Include(u => u.Portfolio!));

            if (user is null)
            {
                _loggerManager.LogWarn($"User with id {id} does not exist");
                throw new NotFoundException($"User with id {id} does not exist");
            }

            _loggerManager.LogInfo($"Successfully retrieved the user with id {id}");

            return user;
        }

        public async Task UpdateAsync(User user)
        {
            var updateResult = await _userRepository.UpdateAsync(user);

            if (!updateResult.Succeeded)
            {
                _loggerManager.LogWarn($"Failed to update the user with id {user.Id}");
                throw new BadRequestException(updateResult.Errors);
            }

            _loggerManager.LogInfo($"Successfully updated the user with id {user.Id}");
        }

        private static Expression<Func<User, bool>>? GetFilterQuery(string? filterParam)
        {
            Expression<Func<User, bool>>? filterQuery = null;

            if (filterParam is not null)
            {
                string formatedFilter = filterParam.Trim().ToLower();

                filterQuery = u => u.FirstName!.ToLower().Contains(formatedFilter)
                    || u.LastName!.ToLower().Contains(formatedFilter)
                    || u.Email.ToLower().Contains(formatedFilter)
                    || u.PhoneNumber.Contains(formatedFilter);
            }

            return filterQuery;
        }

        private static Func<IQueryable<User>, IOrderedQueryable<User>>? GetOrderByQuery(string? orderBy) => orderBy switch
        {
            "FirstName" => query => query.OrderBy(u => u.FirstName),
            "LastName" => query => query.OrderBy(u => u.LastName),
            "Email" => query => query.OrderBy(u => u.Email),
            "PhoneNumber" => query => query.OrderBy(u => u.PhoneNumber),
            "BirthDate" => query => query.OrderBy(u => u.BirthDate),
            _ => null
        };
    }
}

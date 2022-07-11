using System.Linq.Expressions;
using Core.Entities;
using Core.Exceptions;
using Core.Interfaces;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Paginator;
using Core.Paginator.Parameters;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class ProcedureService : IProcedureService
{
    private readonly IProcedureRepository _procedureRepository;
    private readonly ISpecializationService _specializationService;
    private readonly IProcedureSpecializationRepository _procedureSpecializationRepository;
    private readonly ILoggerManager _loggerManager;

    public ProcedureService(
        IProcedureRepository procedureRepository,
        ISpecializationService specializationService, 
        IProcedureSpecializationRepository procedureSpecializationRepository, 
        ILoggerManager loggerManager)
    {
        _procedureRepository = procedureRepository;
        _specializationService = specializationService;
        _procedureSpecializationRepository = procedureSpecializationRepository;
        _loggerManager = loggerManager;
    }

    public async Task CreateNewProcedureAsync(Procedure procedure, IEnumerable<int> specializationIds)
    {
        foreach (var specializationId in specializationIds)
        {
            procedure.ProcedureSpecializations.Add( new ProcedureSpecialization()
            {
                Procedure = procedure,
                Specialization = await _specializationService.GetSpecializationByIdAsync(specializationId)
            });
        }
        await _procedureRepository.InsertAsync(procedure);
        await _procedureRepository.SaveChangesAsync();
        _loggerManager.LogInfo($"Created new procedure with name: {procedure.Name}");
    }

    private async Task UpdateProcedureSpecializationsAsync(int newProcedureId, IEnumerable<int> specializationIds)
    {
        var existing = await _procedureSpecializationRepository.GetAsync(
            filter: pr => pr.ProcedureId == newProcedureId);
        foreach (var ps in existing)
        {
            _procedureSpecializationRepository.Delete(ps);
        }

        foreach (var specializationId in specializationIds)
        {
            var specialization = await _specializationService.GetSpecializationByIdAsync(specializationId);
            await _procedureSpecializationRepository.InsertAsync(new ProcedureSpecialization()
            {
                ProcedureId = newProcedureId,
                SpecializationId = specializationId
            });
        }
        await _procedureSpecializationRepository.SaveChangesAsync();
    }
    
    public async Task UpdateProcedureAsync(Procedure newProcedure, IEnumerable<int> specializationIds)
    {
        await UpdateProcedureSpecializationsAsync(newProcedure.Id, specializationIds);

        _procedureRepository.Update(newProcedure);
        await _procedureRepository.SaveChangesAsync();
        _loggerManager.LogInfo($"Updated procedure with id {newProcedure.Id}");
    }

    public async Task DeleteProcedureAsync(int procedureId)
    {
        var procedureToRemove = await GetByIdAsync(procedureId);
        
        _procedureRepository.Delete(procedureToRemove);
        await _procedureRepository.SaveChangesAsync();
        _loggerManager.LogInfo($"Deleted procedure with Id {procedureId}");
    }
    
    public async Task<Procedure> GetByIdAsync(int procedureId)
    {
        var procedure = await _procedureRepository.GetById(
            procedureId,
            "ProcedureSpecializations.Specialization");
        
        if (procedure is null)
        {
            _loggerManager.LogWarn($"Procedure with id {procedureId} does not exist");
            throw new NotFoundException($"Procedure with Id {procedureId} does not exist");
        }

        _loggerManager.LogInfo($"Found procedure with id {procedureId}");
        return procedure;
    }

    public async Task<PagedList<Procedure>> GetAllProceduresAsync(ProcedureParameters parameters)
    {
        var filterQuery = GetFilterQuery(parameters.FilterParam);
        var orderByQuery = GetOrderByQuery(parameters.OrderByParam);

        var procedures = await _procedureRepository.GetPaged(
            parameters: parameters,
            filter: filterQuery,
            orderBy: orderByQuery,
            includeProperties: "ProcedureSpecializations.Specialization");
        _loggerManager.LogInfo("Found all procedures");
        return procedures;
    }
    
    private static Expression<Func<Procedure, bool>>? GetFilterQuery(string? filterParam)
    {
        Expression<Func<Procedure, bool>>? filterQuery = null;

        if (filterParam is null) return filterQuery;
        
        var formattedFilter = filterParam.Trim().ToLower();

        filterQuery = u => u.Name!.ToLower().Contains(formattedFilter)
                           || u.Description!.ToLower().Contains(formattedFilter)
                           || u.Cost.ToString().Contains(formattedFilter);

        return filterQuery;
    }

    private static Func<IQueryable<Procedure>, IOrderedQueryable<Procedure>>? GetOrderByQuery(string? orderBy) => orderBy switch
    {
        "Cost" => query => query.OrderBy(procedure => procedure.Cost),
        "Name" => query => query.OrderBy(procedure => procedure.Name),
        "Description" => query => query.OrderBy(procedure => procedure.Description),
        _ => null
    };
}
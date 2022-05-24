using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Models;
using DataAccess.Context;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories;

public class ProcedureRepository : IProcedureRepository
{
    private readonly ClinicContext _clinicContext;
    private readonly ISpecializationRepository _specializationRepository;

    public ProcedureRepository(ClinicContext clinicContext, ISpecializationRepository specializationRepository)
    {
        _clinicContext = clinicContext;
        _specializationRepository = specializationRepository;
    }

    public async Task<IEnumerable<Procedure>> GetAllProceduresAsync()
    { 
        return await _clinicContext.Procedures
            .Include(procedure => procedure.ProcedureSpecializations)
            .ToListAsync();
    }

    public async Task<Procedure?> GetProcedureByIdAsync(int procedureId)
    {
        return await _clinicContext.Procedures
            .Include(procedure => procedure.ProcedureSpecializations)
            .SingleOrDefaultAsync(pr => pr.Id == procedureId);
    }

    public async Task<Procedure> AddProcedureAsync(Procedure procedure)
    {
        var result = await _clinicContext.Procedures.AddAsync(procedure);
        return result.Entity;
    }

    public async Task UpdateProcedureAsync(int procedureId, Procedure newProcedure)
    {
        var oldProcedure = await GetProcedureByIdAsync(procedureId);
        
        oldProcedure.Cost = newProcedure.Cost;
        oldProcedure.DurationInMinutes = newProcedure.DurationInMinutes;
        oldProcedure.Name = newProcedure.Name;
        oldProcedure.Description = newProcedure.Description;
    }

    public async Task UpdateProcedureSpecializationsAsync(int procedureId, IEnumerable<int> specializationIds)
    {
        var procedureSpecializationsToRemove = await _clinicContext
            .ProcedureSpecializations
            .Where(ps => ps.ProcedureId == procedureId)
            .ToListAsync();
        
        _clinicContext.ProcedureSpecializations.RemoveRange(procedureSpecializationsToRemove);
        await SaveChangesAsync();
        
        foreach (var specializationId in specializationIds)
        {
            await _specializationRepository.GetSpecializationByIdAsync(specializationId);
            await _clinicContext.ProcedureSpecializations.AddAsync(new ProcedureSpecialization()
            {
                ProcedureId = procedureId,
                SpecializationId = specializationId
            });
        }
    }

    public async Task DeleteProcedureAsync(Procedure procedure)
    {
        _clinicContext.Remove(procedure);
    }

    public async Task SaveChangesAsync()
    {
        await _clinicContext.SaveChangesAsync();
    }
}
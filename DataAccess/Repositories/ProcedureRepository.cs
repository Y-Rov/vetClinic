using Core.Entities;
using Core.Interfaces.Repositories;
using DataAccess.Context;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories;

public class ProcedureRepository : IProcedureRepository
{
    private readonly ClinicContext _clinicContext;
    
    public ProcedureRepository(ClinicContext clinicContext)
    {
        _clinicContext = clinicContext;
    }

    public async Task<IEnumerable<Procedure>> GetAllProceduresAsync()
    { 
        return await _clinicContext.Procedures.ToListAsync();
    }

    public async Task<Procedure?> GetProcedureByIdAsync(int procedureId)
    {
        return await _clinicContext.Procedures.SingleOrDefaultAsync(pr => pr.Id == procedureId);
    }

    public async void AddProcedureAsync(Procedure procedure)
    {
        await _clinicContext.Procedures.AddAsync(procedure);
        await SaveChangesAsync();
    }

    public async void UpdateProcedureAsync(Procedure newProcedure)
    {
        _clinicContext.Entry(newProcedure).State = EntityState.Modified;
        await SaveChangesAsync();
    }

    public async void DeleteProcedureAsync(int procedureId)
    {
        var procedureToRemove = await GetProcedureByIdAsync(procedureId);
        if (procedureToRemove is null) throw new NullReferenceException();
        
        _clinicContext.Remove(procedureToRemove);
        await SaveChangesAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _clinicContext.SaveChangesAsync();
    }
}
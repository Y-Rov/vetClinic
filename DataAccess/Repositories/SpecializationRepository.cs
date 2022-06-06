using Core.Entities;
using Core.Interfaces.Repositories;
using DataAccess.Context;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class SpecializationRepository : ISpecializationRepository
    {
        ClinicContext _context;
        public SpecializationRepository(ClinicContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Specialization>> GetAllSpecializationsAsync()
        {
           return await _context.Specializations
                .Include(specialization => specialization.UserSpecializations)
                    .ThenInclude(us => us.User)
                .Include(specialization => specialization.ProcedureSpecializations)
                    .ThenInclude(ps => ps.Procedure)
                .AsNoTracking()
                .AsSplitQuery()
                .ToListAsync();
        }

        public async Task<Specialization> GetSpecializationByIdAsync(int id)
        {
            Specialization? result = await _context.Specializations
                .Include(specialization => specialization.UserSpecializations)
                    .ThenInclude(us => us.User)
                .Include(specialization => specialization.ProcedureSpecializations)
                    .ThenInclude(ps => ps.Procedure)
                .AsSplitQuery()
                .FirstOrDefaultAsync(specialization => specialization.Id == id);
            return result;
        }

        public async Task<Specialization> AddSpecializationAsync(Specialization specialization)
        {
            await _context.Specializations.AddAsync(specialization);
            return specialization;
        }

        public async Task DeleteSpecializationAsync(Specialization specialization)
        {
             _context.Specializations
                .Remove(specialization);

            await Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}

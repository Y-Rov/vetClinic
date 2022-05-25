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
                .ToListAsync();
        }

        public async Task<Specialization> GetSpecializationByIdAsync(int id)
        {
            Specialization result = await _context.Specializations
                .Include(specialization => specialization.UserSpecializations)
                .FirstAsync(specialization => specialization.Id == id);
            return result ?? throw new Exception("Specialization not found");
        }

        public async Task<Specialization> AddSpecializationAsync(Specialization specialization)
        {
            await _context.Specializations.AddAsync(specialization);
            await _context.SaveChangesAsync();
            return specialization;
        }

        public async Task<int> DeleteSpecializationAsync(int id)
        {
            _context.Specializations
                .Remove(await _context.Specializations.FirstAsync(spec => spec.Id == id));
            int affectedColumns = await _context.SaveChangesAsync();
            return affectedColumns;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<Specialization> UpdateSpecializationAsync(int id, Specialization updatedSpecialization)
        {
            _context.Entry(updatedSpecialization).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return updatedSpecialization;
        }
    }
}

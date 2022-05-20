using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Configurations;

public static class ProcedureConfiguration
{
    public static void ConfigureProcedureEntity(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Procedure>()
            .HasMany<Procedure>();
    }
}
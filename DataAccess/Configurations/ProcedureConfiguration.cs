using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations;

public class ProcedureConfiguration : IEntityTypeConfiguration<Procedure>
{
    public void Configure(EntityTypeBuilder<Procedure> builder)
    {
        builder
            .HasKey(procedure => procedure.Id);

        builder
            .Property(procedure => procedure.Description)
            .HasMaxLength(250);
        
        builder
            .Property(procedure => procedure.Name)
            .HasMaxLength(100);

        builder
            .Property(procedure => procedure.Cost)
            .HasPrecision(8, 2);

        builder
            .ToTable("Procedures");
    }
}
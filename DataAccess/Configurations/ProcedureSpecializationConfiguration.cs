using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations;

public class ProcedureSpecializationConfiguration : IEntityTypeConfiguration<ProcedureSpecialization>
{
    public void Configure(EntityTypeBuilder<ProcedureSpecialization> builder)
    {
        builder
            .HasKey(ps => new
            {
                ps.ProcedureId,
                ps.SpecializationId
            });

        builder
            .HasOne<Procedure>(ps => ps.Procedure)
            .WithMany(procedure => procedure.ProcedureSpecializations)
            .HasForeignKey(ps => ps.ProcedureId);
        
        builder
            .HasOne<Specialization>(ps => ps.Specialization)
            .WithMany(specialization => specialization.ProcedureSpecializations)
            .HasForeignKey(ps => ps.SpecializationId);

        builder.ToTable("ProcedureSpecializations");
    }
}
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class SpecializationConfiguration : IEntityTypeConfiguration<Specialization>
    {
        public void Configure(EntityTypeBuilder<Specialization> builder)
        {
            builder
                .HasKey(spec => spec.Id);

            builder
                .Property(spec => spec.Name)
                .HasMaxLength(50);

            builder
                .HasMany(specialization => specialization.Doctors)
                .WithMany(doctor => doctor.Specializations)
                .UsingEntity<UserSpecialization>();

            builder.ToTable("Specializations");
        }
    }
}

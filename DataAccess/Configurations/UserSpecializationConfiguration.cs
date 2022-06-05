using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class UserSpecializationConfiguration : IEntityTypeConfiguration<UserSpecialization>
    {
        public void Configure(EntityTypeBuilder<UserSpecialization> builder)
        {
            builder
                .HasKey(userSpec =>
                    new
                    {
                        userSpec.UserId,
                        userSpec.SpecializationId
                    });

            builder
                .HasOne(userSpec => userSpec.User)
                .WithMany(user => user.UserSpecializations)
                .HasForeignKey(userSpec => userSpec.UserId);

            builder
                .HasOne(userSpec => userSpec.Specialization)
                .WithMany(specialization => specialization.UserSpecializations)
                .HasForeignKey(userSpec => userSpec.SpecializationId);

            builder.ToTable("UserSpecializations");
        }
    }
}
